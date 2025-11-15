using EzeePdf.Core.DB;
using EzeePdf.Core.Enums;
using EzeePdf.Core.Model.Users;
using EzeePdf.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace EzeePdf.Core.Repositories
{
    public class UserRepository(EzeepdfContext context) : IUserRepository
    {
        public async Task CreateUser(NewUser request)
        {
            context.Users.Add(new User
            {
                UserTypeId = request.UserTypeId ?? (int)EnumUserType.Public,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                Password = request.Password,
            });

            await context.SaveChangesAsync();
        }
        public async Task<(EnumResponseCode Code, User? User)> Login(LoginRequest request)
        {
            var responseCode = EnumResponseCode.Success;
            var user = await context.Users.FirstOrDefaultAsync(x => x.EmailAddress == request.EmailAddress);
            if (user is null)
            {
                responseCode = EnumResponseCode.InvalidCredentials;
            }
            else if (user.Locked == true)
            {
                responseCode = EnumResponseCode.UserLocked;
            }
            else if (user.Password != request.Password)
            {
                responseCode = EnumResponseCode.InvalidCredentials;
                user.FailedLoginAttemptCount++;
                //if(user.FailedLoginAttemptCount == limit)
                //{
                //    user.Locked = true;
                //    responseCode = EnumResponseCode.InvalidCredentials;
                //}
                context.Update(user);
                await context.SaveChangesAsync();
            }
            else
            {
                user.FailedLoginAttemptCount = 0;
                context.Update(user);
                await context.SaveChangesAsync();
            }
            return (responseCode, user);
        }
        public async Task SaveRefreshToken(string token, TokenData data)
        {
            var existingToken = await context.RefreshTokens
                    .FirstOrDefaultAsync(x => x.UserId == data.UserId && x.SourceDevice == data.SourceDevice);
            if (existingToken is null)
            {
                var refreshToken = new RefreshToken
                {
                    UserId = data.UserId,
                    SourceDevice = data.SourceDevice,
                    Token = token,
                    IssuedAt = data.IssuedAt,
                    ExpiresAt = data.ExpiresAt,
                    IpAddress = data.IpAddress,
                };
                context.RefreshTokens.Add(refreshToken);
            }
            else
            {
                existingToken.Token = token;
                existingToken.IssuedAt = data.IssuedAt;
                existingToken.ExpiresAt = data.ExpiresAt;
                existingToken.IpAddress = data.IpAddress;
                context.RefreshTokens.Update(existingToken);
            }
            await context.SaveChangesAsync();
        }
        public Task<RefreshToken?> GetRefreshToken(int userId, string sourceDevice)
        {
            return context.RefreshTokens
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.SourceDevice == sourceDevice);
        }
        public async Task<bool> IsUserLocked(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            return user is null || user.Locked == true;
        }
    }
}
