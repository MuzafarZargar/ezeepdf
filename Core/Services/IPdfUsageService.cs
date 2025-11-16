using EzeePdf.Core.Enums;
using EzeePdf.Core.Responses;

namespace EzeePdf.Core.Services
{
    public interface IPdfUsageService
    {
        Task<DataResponse> DailyLimitReached();
        Task<DataResponse> AddUpload(int? userId,
            EnumPdfFunction function,
            string? sourceDevice,
            long pdfSize,
            string? fileName,
            string? ipAddress);
        Task<DataResponse> SaveUsage(int? userId,
        EnumPdfFunction function,
        string? sourceDevice,
        int pageCount,
        long pdfSize,
        long pdfChangedSize, 
        string? ipAddress);
    }
}