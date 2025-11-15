namespace EzeePdf.Core.Model.Users
{
    public class UpdateUser
    {
        public required int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
