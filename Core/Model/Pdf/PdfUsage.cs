using EzeePdf.Core.Enums;

namespace EzeePdf.Core.Model.Pdf
{
    public class PdfUsage
    {
        public int? UserId { get; set; }
        public int Function { get; set; }
        public required string IpAddress { get; set; }
        public int PageCount { get; set; }
        public double PdfSize { get; set; }
        public double PdfChangedSize { get; set; }
        public DateTime UsageDate { get; set; }
        public string? FileName { get; set; }
        public required string SourceDevice { get; set; }
    }
}
