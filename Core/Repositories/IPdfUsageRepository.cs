using EzeePdf.Core.Enums;
using EzeePdf.Core.Model.Pdf;

namespace EzeePdf.Core.Repositories
{
    public interface IPdfUsageRepository
    {
        Task AddUpload(PdfUsage request);
        Task AddUsage(PdfUsage request);
        Task<DateTime> GetThisIpAddressLastUsageTime(string ipAddress, EnumPdfFunction type);
        Task<double> GetDayUsage(DateTime date);
    }
}