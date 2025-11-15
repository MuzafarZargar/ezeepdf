using System.Security.Claims;
using System.Text.Json.Serialization;

namespace EzeePdf.Core.Model.Users
{
    public class LoginResponse
    {
        public required string FullName { get; set; }
        public required JwtToken Token { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSupport { get; set; }
        public bool IsPublic { get; set; }
        public bool ForcePasswordChange { get; set; }
    }
    public class TokenData
    {
        public required int UserId { get; set; }
        public required string UserName { get; set; }
        public int UserTypeId { get; set; }
        public string SourceDevice { get; set; } = null!;
        public string IpAddress { get; set; } = null!;
        public int Refreshing { get; set; } = 0;
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
