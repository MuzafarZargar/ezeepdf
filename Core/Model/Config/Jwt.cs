namespace EzeePdf.Core.Model.Config
{
    public class Jwt
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int AccessTokenExpiryMinutes { get; set; }
        public int RefreshTokenExpiryHours { get; set; }
        public string? AccessTokenSigningKey { get; set; }
        public byte[]? AccessTokenSigningKeyBytes { get; set; }
    }
}
