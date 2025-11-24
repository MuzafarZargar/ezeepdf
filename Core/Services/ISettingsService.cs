using EzeePdf.Core.Enums;

namespace EzeePdf.Core.Services
{
    public interface ISettingsService
    {
        Task<string?> Get(EnumSettings setting, string? defaultValue = null);
        Task<int> GetInt(EnumSettings setting, int defaultValue = 0);
        Task<double> GetDouble(EnumSettings setting, double defaultValue = 0);
        Task<int> MaxAllowedPageCount();
        Task<int> PdfDailyLimit();
        Task<int> PdfConsecutiveFeatureUsageWait();
        Task<int> PdfConsecutiveFeedbackWait();
        Task<long> PdfMaxFileSize();
        Task<long> ImageMaxFileSize();
        Task<long> WatermarkImageMaxSize();
        Task<long> PPTMaxFileSize();
    }
}