using System.ComponentModel.DataAnnotations;
using EzeePdf.Attributes;

namespace EzeePdf.DTO.Users
{
    public class NewUserDto
    {
        public int? UserTypeId { get; set; }

        [Required(ErrorMessage = "First Name is missing")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First name length should be between 3 and 100")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is missing")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last name length should be between 3 and 100")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email Address is missing")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Email length should be between 5 and 255")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Password is missing")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password length should be between 5 and 20")]
        public string Password1 { get; set; } = null!;

        [Required(ErrorMessage = "Password is missing")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password length should be between 5 and 20")]
        [StringSame("Password1", "Passwords do not match")]
        public string Password2 { get; set; } = null!;
        public string Password => Password1;
    }
}
