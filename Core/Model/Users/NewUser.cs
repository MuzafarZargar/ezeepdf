namespace EzeePdf.Core.Model.Users
{
    public class NewUser
    {
        public int? UserTypeId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
