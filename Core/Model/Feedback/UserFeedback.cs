namespace EzeePdf.Core.Model.Feedback
{
    public class UserFeedback
    {
        public string FeedbackText { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? SourceDevice { get; set; }
        public string? IpAddress { get; set; }
        public DateTime FeedbackDate { get; set; }
    }
}
