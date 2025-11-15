using EzeePdf.Core.Enums;
using EzeePdf.Core.Extensions;
using EzeePdf.Core.Model.Pdf;
using EzeePdf.Core.Repositories;
using EzeePdf.Core.Responses;
using EzeePdf.Core.Services.Logging;
using Microsoft.AspNetCore.Http;

namespace EzeePdf.Core.Services
{
    public class PdfUsageService(
        IPdfUsageRepository pdfUsageRepository,
        ISettingsService settingsService,
        ILogService logService,
        IHttpContextAccessor httpContextAccessor) : IPdfUsageService
    {
        private readonly IPdfUsageRepository pdfUsageRepository = pdfUsageRepository;
        private readonly ISettingsService settingsService = settingsService;
        private readonly ILogService logService = logService;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        public async Task<DataResponse> DailyLimitReached()
        {
            string? errorMessage = default;
            var code = EnumResponseCode.Success;
            try
            {
                string? denyReason = null;
                var maxDailyLimit = await settingsService.PdfDailyLimit();
                var consecutiveTime = await settingsService.PdfConsecutiveDownloadWait();

                if (await pdfUsageRepository.GetDayUsage(Utils.UtcDay) >= maxDailyLimit)
                {
                    denyReason = $"Daily download limit ({maxDailyLimit} MB) reached ";
                    code = EnumResponseCode.DailyUploadLimitReached;
                }
            }
            catch (Exception exception)
            {
                code = EnumResponseCode.DailyLimitFetchError;
                logService.Error($"Error while fetching daily limit", exception);
            }

            return DataResponse.FromError(code, errorMessage);
        }
        public async Task<DataResponse> AddUpload(int? userId,
            EnumPdfFunction function,
            string? sourceDevice,
            long pdfSize,
            string? fileName)
        {
            try
            {
                var usage = new PdfUsage
                {
                    SourceDevice = sourceDevice ?? Constants.DEFAULT_DEVICE_NAME,
                    IpAddress = Utils.IpAddress(httpContextAccessor?.HttpContext),
                    Function = function.Value(),
                    UsageDate = Utils.UtcNow,
                    FileName = fileName,
                    PdfSize = pdfSize.BytesToMbWithDecimal(),
                };

                await pdfUsageRepository.AddUpload(usage);
            }
            catch (Exception exception)
            {
                logService.Error($"Error while saving pdf usage ({function})", exception);
            }

            return await DailyLimitReached();
        }
        public async Task<DataResponse> SaveUsage(
            int? userId,
            EnumPdfFunction function,
            string? sourceDevice,
            int pageCount,
            long pdfSize,
            long pdfChangedSize)
        {
            string? errorMessage = default;
            var code = EnumResponseCode.Success;
            try
            {
                var consecutiveTime = await settingsService.PdfConsecutiveDownloadWait();

                code = await Utils.BlockForTime(httpContextAccessor?.HttpContext, function, consecutiveTime,
                                                pdfUsageRepository.GetThisIpAddressLastUsageTime);
                if (code != EnumResponseCode.Success)
                {
                    errorMessage = $"Each feature can be used only one in {consecutiveTime} minutes";
                }
                else
                {
                    var usage = new PdfUsage
                    {
                        SourceDevice = sourceDevice ?? Constants.DEFAULT_DEVICE_NAME,
                        IpAddress = Utils.IpAddress(httpContextAccessor?.HttpContext),
                        Function = function.Value(),
                        UsageDate = Utils.UtcNow,
                        PageCount = pageCount,
                        PdfSize = pdfSize.BytesToMbWithDecimal(),
                        PdfChangedSize = pdfChangedSize.BytesToMbWithDecimal()
                    };

                    await pdfUsageRepository.AddUsage(usage);
                }
            }
            catch (Exception exception)
            {
                code = EnumResponseCode.UsageSaveError;
                logService.Error($"Error while saving pdf usage ({function})", exception);
            }

            return DataResponse.FromError(code, errorMessage);
        }
    }
}
