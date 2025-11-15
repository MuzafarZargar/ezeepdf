using System.IdentityModel.Tokens.Jwt;

namespace EzeePdf.Core.Extensions
{
    public static class JwtTokenExtensions
    {
        public static string? Get(this JwtSecurityToken? jwtToken, string type)
        {
            if (jwtToken is null)
            {
                return null;
            }
            var value = jwtToken.Claims.SingleOrDefault(c => c.Type == type);
            return value?.Value;
        }
        public static int? UserType(this JwtSecurityToken? jwtToken, Exception? exception = null)
        {
            var value = jwtToken.Get(Constants.CLAIM_USER_TYPE);
            var userType = Utils.ParseIntNull(value);
            if (userType is null && exception is not null)
            {
                throw exception;
            }
            return userType;
        }
        public static int? UserId(this JwtSecurityToken? jwtToken, Exception? exception = null)
        {
            var value = jwtToken.Get(Constants.CLAIM_USER_ID);
            var userId = Utils.ParseIntNull(value);
            if (userId is null && exception is not null)
            {
                throw exception;
            }
            return userId;
        }
        //public static string? Email(this JwtSecurityToken? jwtToken, Exception? exception = null)
        //{
        //    var value = jwtToken.Get(Constants.CLAIM_EMAIL);
        //    if (string.IsNullOrWhiteSpace(value) && exception is not null)
        //    {
        //        throw exception;
        //    }
        //    return Utils.Decrypt(value, true);
        //}
    }
}
