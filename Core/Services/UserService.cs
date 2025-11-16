using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using EzeePdf.Core.Enums;
using EzeePdf.Core.Exceptions;
using EzeePdf.Core.Extensions;
using EzeePdf.Core.Model.Config;
using EzeePdf.Core.Model.Users;
using EzeePdf.Core.Repositories;
using EzeePdf.Core.Responses;
using EzeePdf.Core.Services.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EzeePdf.Core.Services
{
    public class UserService(IUserRepository userRepository,
            IUserSessionService userSessionService,
            IHttpContextAccessor httpContextAccessor,
            ILogService logService,
            IOptions<Jwt> jwt) : IUserService
    {
        private const string JWT_ALGORITHM = SecurityAlgorithms.HmacSha512;
        private const int REFRESHING_TOKEN = 1;
        private readonly Jwt jwt = jwt.Value;
        private readonly ILogService logService = logService;

        private readonly IUserRepository userRepository = userRepository;
        private readonly IUserSessionService userSessionService = userSessionService;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        public async Task<DataResponse> CreateUser(NewUser user)
        {
            user.Password = user.Password.Hash256();
            try
            {
                await userRepository.CreateUser(user);
                return DataResponse.OK;
            }
            catch (Exception exception)
            {
                if (exception.DuplicateKey())
                {
                    return DataResponse.From(EnumResponseCode.DuplicateEmailAddress);
                }
                return DataResponse.From(EnumResponseCode.RegistrationError);
            }
        }
        public async Task<DataResponse<string>> Login(LoginRequest request)
        {
            var responseCode = EnumResponseCode.Success;
            LoginResponse? loginResponse = default;
            try
            {
                request.Password = request.Password.Hash256();
                var response = await userRepository.Login(request);
                responseCode = response.Code;
                if (response.Code == EnumResponseCode.Success)
                {
                    var forcePasswordChange = false;
                    var user = response.User!;
                    var jwtToken = await CreateToken(new TokenData
                    {
                        UserId = user.UserId,
                        UserTypeId = user.UserTypeId,
                        UserName = user.EmailAddress,
                        SourceDevice = request.SourceDevice,
                        IpAddress = "127",
                    });

                    //Detect ForcePassword Change from Settings
                    loginResponse = new LoginResponse
                    {
                        FullName = $"{user.FirstName} {user.LastName}",
                        IsAdmin = user.UserTypeId == EnumUserType.Admin.Value(),
                        IsSupport = user.UserTypeId == EnumUserType.Support.Value(),
                        IsPublic = user.UserTypeId == EnumUserType.Public.Value(),
                        ForcePasswordChange = forcePasswordChange,
                        Token = jwtToken
                    };
                }
            }
            catch (Exception exception)
            {
                logService.Error($"Error in login for {request.EmailAddress}", exception);
                responseCode = EnumResponseCode.SigninError;
            }
            return new DataResponse<string>(responseCode)
            {
                Response = loginResponse?.AsJsonString(true)
            };
        }
        public void SaveLoginInfo(string json)
        {
            var login = json.FromJsonString<LoginResponse>(true);
            if (login is not null)
            {
                var tokenJson = login.Token.AsJsonString(true);
                httpContextAccessor.HttpContext!.Response.Cookies.Append(Constants.APP_TOKEN_COOKIE, tokenJson, new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(Constants.ACCESS_TOKEN_EXPIRY_MINUTES),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                var loginInfoJson = login.AsJsonString(true);
                httpContextAccessor.HttpContext!.Response.Cookies.Append(Constants.APP_USER_COOKIE, loginInfoJson, new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(jwt.RefreshTokenExpiryHours),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
            }
        }
        public async Task<bool> IsUserLoggedIn()
        {
            var user = await GetLoginInfo(false);
            return user.Claims?.Any() == true;
        }
        public async Task<ClaimsPrincipal> GetLoginInfo(bool saveInfo = true)
        {
            try
            {
                IEnumerable<Claim>? claims = null;
                if (httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(Constants.APP_TOKEN_COOKIE))
                {
                    var cookieValue = httpContextAccessor.HttpContext!.Request.Cookies[Constants.APP_TOKEN_COOKIE];
                    var jwtToken = cookieValue.FromJsonString<JwtToken>(true);
                    if (jwtToken is not null)
                    {
                        var ipAddress  = Utils.IpAddress(userSessionService);
                        var newToken = await RefreshToken(jwtToken.AccessToken,
                           jwtToken.RefreshToken,
                           jwtToken.SourceDevice,
                           ipAddress,
                           true);

                        claims = newToken?.Claims;
                    }
                }
                return await RefreshPrincipal(claims, saveInfo);
            }
            catch (Exception exception)
            {
                logService.Error("Error fetching login information", exception);
            }
            return new ClaimsPrincipal();
        }
        public string? LogoutFromBrowser()
        {
            string? userInfo = default;
            if (httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(Constants.APP_TOKEN_COOKIE))
            {
                httpContextAccessor.HttpContext!.Response.Cookies.Delete(Constants.APP_TOKEN_COOKIE);
            }
            if (httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(Constants.APP_USER_COOKIE))
            {
                userInfo = httpContextAccessor.HttpContext!.Request.Cookies[Constants.APP_USER_COOKIE];
                httpContextAccessor.HttpContext!.Response.Cookies.Delete(Constants.APP_USER_COOKIE);
            }
            return userInfo;
        }
        public Task LogoutFromServer(string token)
        {
            var login = token.FromJsonString<LoginResponse>(true);
            if (login?.Token?.RefreshToken is not null)
            {
            }
            return Task.CompletedTask;
        }
        private async Task<ClaimsPrincipal> RefreshPrincipal(IEnumerable<Claim>? claims, bool saveInfo)
        {
            if (claims?.Any() != true)
            {
                var cookieValue = httpContextAccessor.HttpContext!.Request.Cookies[Constants.APP_USER_COOKIE];
                var loginResponse = cookieValue.FromJsonString<LoginResponse>(true);
                if (loginResponse?.Token is not null)
                {
                    var ipAddress = Utils.IpAddress(userSessionService);
                    var newToken = await RefreshToken(loginResponse.Token.AccessToken,
                                    loginResponse.Token.RefreshToken,
                                    loginResponse.Token.SourceDevice,
                                    ipAddress,
                                    false);
                    if (newToken is not null)
                    {
                        if (saveInfo)
                        {
                            loginResponse.Token = newToken;
                            var json = loginResponse.AsJsonString(true);
                            SaveLoginInfo(json);
                        }
                        claims = newToken.Claims;
                    }
                }
            }
            return CreatePricipal(claims);
        }
        private ClaimsPrincipal CreatePricipal(IEnumerable<Claim>? claims = null)
        {
            if (claims?.Any() == true)
            {
                var claimsIdentity = new ClaimsIdentity(claims, Constants.AUTH_TYPE);
                var user = new ClaimsPrincipal(claimsIdentity);
                return user;
            }
            return new ClaimsPrincipal();
        }
        private async Task<JwtToken> CreateToken(TokenData tokenData, string? existingRefreshToken = null)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(jwt.AccessTokenSigningKeyBytes), JWT_ALGORITHM);

            var claims = CreateClaims(tokenData.UserId, tokenData.UserTypeId);
            var accessToken = new JwtSecurityToken(

                issuer: jwt.Issuer,
                audience: jwt.Audience,
            claims: claims,
                expires: Utils.Now.AddMinutes(jwt.AccessTokenExpiryMinutes),
                signingCredentials: credentials
            );

            //Create new Refresh Token
            //In one case where multiple hits for RefreshToken come from same page (same user), first hit creates new token and updates the Database
            //Next calls from same page will fail as the refresh token has changed in database
            //Therefore when refresh token is updated in DB we save current refresh token in Old Refresh Token Column
            //When a call to RefreshToken fails with its own refresh token, we try with Old Refresh Token
            //Following line will have 

            var refreshTokenKey = GenerateRefreshToken(existingRefreshToken);
            credentials = new SigningCredentials(new SymmetricSecurityKey(refreshTokenKey.AsBytes()), JWT_ALGORITHM);
            var expiryDate = Utils.Now.AddHours(jwt.RefreshTokenExpiryHours);
            var refreshToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                expires: expiryDate,
                signingCredentials: credentials);

            var response = new JwtToken
            {
                SourceDevice = tokenData.SourceDevice,
                ExpiryInMinutes = jwt.AccessTokenExpiryMinutes,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken)
            };

            if (existingRefreshToken == null)
            {
                tokenData.IssuedAt = DateTime.UtcNow;
                tokenData.ExpiresAt = expiryDate;
                await userRepository.SaveRefreshToken(refreshTokenKey, tokenData);
            }
            return response;
        }
        private async Task<JwtToken?> RefreshToken(
            string accessJwtToken,
            string refreshJwtToken,
            string sourceDevice,
            string ipAddress,
            bool checkAccessTokenExpiry)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken? jwtSecurityToken = null;
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(jwt.AccessTokenSigningKeyBytes),
                    ClockSkew = TimeSpan.Zero,
                };

                try
                {
                    //Validate Access Token and Ignore Expired Error
                    tokenHandler.ValidateToken(accessJwtToken, tokenValidationParameters, out var securityToken);
                    jwtSecurityToken = securityToken as JwtSecurityToken;
                }
                catch (SecurityTokenExpiredException)
                {
                    if (checkAccessTokenExpiry)
                    {
                        return default;
                    }
                    jwtSecurityToken = tokenHandler.ReadJwtToken(accessJwtToken);
                }

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(JWT_ALGORITHM, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid Access Token");
                }
                var userId = jwtSecurityToken.UserId(new AppException(EnumResponseCode.RefreshTokenError, "Invalid Refresh Token: UserId missing"));
                var userType = jwtSecurityToken.UserType(new AppException(EnumResponseCode.RefreshTokenError, "Invalid Refresh Token: UserType missing"));

                if (await userRepository.IsUserLocked(userId ?? 0))
                {
                    throw new AccessViolationException("User has been locked");
                }

                if (checkAccessTokenExpiry)
                {
                    return new JwtToken
                    {
                        AccessToken = accessJwtToken,
                        RefreshToken = refreshJwtToken,
                        SourceDevice = sourceDevice,
                        Claims = CreateClaims(userId!.Value, userType!.Value)
                    };
                }
                var userRefreshToken = await userRepository.GetRefreshToken(userId!.Value, sourceDevice);
                if (userRefreshToken is null)
                {
                    throw new AppException(EnumResponseCode.RefreshTokenError, "No token record for user");
                }
                tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(userRefreshToken.Token.AsBytes());
                try
                {
                    tokenHandler.ValidateToken(refreshJwtToken, tokenValidationParameters, out var st);
                }
                catch
                {
                    throw;
                }
                var tokenData = new TokenData
                {
                    UserName = string.Empty,
                    UserId = userId!.Value,
                    UserTypeId = userType!.Value,
                    IpAddress = ipAddress,
                    SourceDevice = sourceDevice,
                    Refreshing = REFRESHING_TOKEN,
                };

                var newToken = await CreateToken(tokenData);
                newToken.Claims = CreateClaims(userId!.Value, userType!.Value);
                return newToken;
            }
            catch (Exception exception)
            {
                logService.Error($"Error in refresh token for IP Address: {ipAddress}", exception);
            }
            return default;
        }

        private List<Claim> CreateClaims(int userId, int userTypeId)
        {
            List<Claim> claims =
            [
                new(Constants.CLAIM_USER_TYPE, userTypeId.ToString()),
                new(Constants.CLAIM_USER_ID, userId.ToString()),
            ];
            return claims;
        }
        private string GenerateRefreshToken(string? existingToken)
        {
            if (existingToken is not null)
            {
                return existingToken;
            }
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
