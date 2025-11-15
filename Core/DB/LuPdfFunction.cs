namespace EzeePdf.Core.DB;

public partial class LuPdfFunction
{
    public int PdfFunctionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<UserPdfUsage> UserPdfUsages { get; set; } = new List<UserPdfUsage>();

    public virtual ICollection<PdfUpload> PdfUploads { get; set; } = new List<PdfUpload>();
}
