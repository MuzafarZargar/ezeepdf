namespace EzeePdf.Core.DB;

public partial class PdfUpload
{
    public int PdfUploadId { get; set; }

    public int? UserId { get; set; }

    public int PdfFunctionId { get; set; }

    public DateTime UploadDate { get; set; }

    public string? FileName { get; set; }

    public double PdfSize { get; set; }

    public string IpAddress { get; set; } = null!;

    public string SourceDevice { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual LuPdfFunction PdfFunction { get; set; } = null!;
}
