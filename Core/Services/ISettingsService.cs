using EzeePdf.Core.Enums;

namespace EzeePdf.Core.Services
{
    public interface ISettingsService
    {
        Task<string?> Get(EnumSettings setting, string? defaultValue = null);
        Task<int> GetInt(EnumSettings setting, int defaultValue = 0);
        Task<int> MaxAllowedPageCount();
        Task<int> PdfDailyLimit();
        Task<int> PdfConsecutiveFeatureUsageWait();
        Task<int> PdfConsecutiveFeedbackWait();
        Task<long> PdfMaxPdfFileSize();
        Task<long> WatermarkImageMaxSize();
    }
}