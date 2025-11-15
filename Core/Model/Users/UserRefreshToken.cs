namespace EzeePdf.Core.Model.Users
{
    public class UserRefreshToken
    {
        public int UserId { get; set; }
        public required JwtToken Token { get; set; }
    }
}
