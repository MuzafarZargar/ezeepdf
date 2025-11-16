using EzeePdf.Core.Model.Feedback;
using EzeePdf.Core.Repositories;
using EzeePdf.Core.Responses;
using EzeePdf.Core.Services.Logging;

namespace EzeePdf.Core.Services
{
    public class FeedbackService(
            IFeedbackRepository feedbackRepository,
            ISettingsService settingsService,
            HttpClient httpClient,
            ILogService logService) : IFeedbackService
    {
        private readonly IFeedbackRepository feedbackRepository = feedbackRepository;
        private readonly ISettingsService settingsService = settingsService;
        private readonly HttpClient httpClient = httpClient;
        private readonly ILogService logService = logService;

        public async Task<DataResponse> SaveFeedback(UserFeedback request)
        {
            string? errorMessage = default;
            var code = EnumResponseCode.Success;
            try
            {
                var consecutiveTime = await settingsService.PdfConsecutiveDownloadWait();
                code = await Utils.BlockForTime(httpClient, consecutiveTime, feedbackRepository.GetThisIpAddressLastFeedbackTime);
                if (code == EnumResponseCode.Success)
                {
                    request.IpAddress = await Utils.IpAddress(httpClient);
                    request.FeedbackDate = Utils.UtcNow;
                    request.SourceDevice ??= Constants.DEFAULT_DEVICE_NAME;
                    await feedbackRepository.SaveFeedback(request);
                }
                else
                {
                    errorMessage = $"Each feature can be used only once in {consecutiveTime} minutes";
                }
            }
            catch (Exception exception)
            {
                code = EnumResponseCode.FeedbackSaveError;
                logService.Error("Error saving feedback", exception);
            }

            return DataResponse.FromError(code, errorMessage);
        }
    }
}
