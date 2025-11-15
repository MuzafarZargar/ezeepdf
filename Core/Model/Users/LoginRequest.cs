using System.ComponentModel.DataAnnotations;

namespace EzeePdf.Core.Model.Users
{
    public class LoginRequest
    {
        public string EmailAddress { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string SourceDevice { get; set; } = null!;
    }
}
