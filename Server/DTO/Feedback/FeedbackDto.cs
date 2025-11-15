using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EzeePdf.Core;

namespace EzeePdf.DTO.Feedback
{
    public class FeedbackDto
    {
        private string? email;

        [Required(ErrorMessage = "Feedback is missing")]
        [StringLength(Constants.FEEDBACK_MAX_LENGTH, MinimumLength = Constants.FEEDBACK_MIN_LENGTH, ErrorMessage = "Feedback should range between 10 and 300 characters")]
        public string FeedbackText { get; set; } = null!;

        [AllowNull]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(255, ErrorMessage = "Email exceeds allowed length (255)")]
        public string? EmailAddress
        {
            get
            {
                return email;
            }
            set
            {
                email = string.IsNullOrWhiteSpace(value) ? null : value;
            }
        }
        public string? SourceDevice { get; set; }
    }
}
