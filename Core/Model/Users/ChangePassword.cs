namespace EzeePdf.Core.Model.Users
{
    public class ChangePassword
    {
        public int UserId { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
