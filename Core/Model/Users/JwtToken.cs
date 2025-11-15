using System.Security.Claims;
using System.Text.Json.Serialization;

namespace EzeePdf.Core.Model.Users
{
    public class JwtToken
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string SourceDevice { get; set; } = null!;
        public int ExpiryInMinutes { get; set; }
        [JsonIgnore] public List<Claim> Claims { get; set; } = [];
    }
}
