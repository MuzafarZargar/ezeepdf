using System.ComponentModel.DataAnnotations;

namespace EzeePdf.DTO.Users
{
    public class LoginRequestDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is missing")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is missing")]
        public string Password { get; set; } = null!;
        public string SourceDevice { get; set; } = null!;
    }
}
