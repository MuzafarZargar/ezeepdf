using System.Collections.Generic;
using EzeePdf.Core.Enums;
using EzeePdf.Core.Extensions;
using EzeePdf.Core.Model.Settings;
using EzeePdf.Core.Repositories;
using EzeePdf.Core.Services.DI;
using EzeePdf.Core.Services.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace EzeePdf.Core.Services
{
    public class SettingsService(IResolver resolver) : ISettingsService
    {
        private readonly IResolver resolver = resolver;

        public async Task<string?> Get(EnumSettings setting, string? defaultValue = default)
        {
            var settings = await CacheSettings();
            var value = settings.FirstOrDefault(x => x.Id == setting.Value());
            return value?.Value ?? defaultValue;
        }
        public async Task<int> GetInt(EnumSettings setting, int defaultValue = 0)
        {
            var settings = await CacheSettings();
            var value = settings.FirstOrDefault(x => x.Id == setting.Value());
            return value?.Value.Int(defaultValue) ?? defaultValue;
        }
        public async Task<int> MaxAllowedPageCount()
        {
            //if (await resolver.Get<IUserService>().IsUserLoggedIn())
            //{
            //    return await GetInt(EnumSettings.MaxPages, Constants.FREE_VERSION_PAGE_COUNT);
            //}
            return await GetInt(EnumSettings.FreeVersionAllowedPageCount, Constants.FREE_VERSION_PAGE_COUNT);
        }
        public async Task<int> PdfDailyLimit()
        {
            return await GetInt(EnumSettings.MaxUploadPerDay, Constants.PDF_DAILY_UPLOAD_SIZE_MB);
        }
        public async Task<int> PdfConsecutiveFeatureUsageWait()
        {
            return await GetInt(EnumSettings.ConsecutiveUsageWait, Constants.CONSECUTIVE_FEATURE_USAGE_WAIT_MINUTES);
        }
        public async Task<int> PdfConsecutiveFeedbackWait()
        {
            return await GetInt(EnumSettings.ConsecutiveFeedbackWait, Constants.CONSECUTIVE_FEATURE_USAGE_WAIT_MINUTES);
        }
        public async Task<long> PdfMaxPdfFileSize()
        {
            var size = await GetInt(EnumSettings.MaxPdfSizeMB, Constants.PDF_MAX_UPLOAD_SIZE_MB);
            return size.MbToBytes();
        }
        public async Task<long> WatermarkImageMaxSize()
        {
            var size = await GetInt(EnumSettings.WatermarkMaxImageSizeKB, Constants.WATERMARK_MAX_SIZE_KB);
            return size.KbToByte();
        }
        private async Task<List<Setting>> CacheSettings()
        {
            List<Setting>? settings = default;
            try
            {
                var cacheService = resolver.Get<IMemoryCache>();
                settings = cacheService.Get<List<Setting>>(CacheConstants.SETTINGS_CACHE);
                if (settings is null)
                {
                    var allSettings = await resolver.Get<ISettingsRepository>().GetAllSettings();
                    settings = allSettings.Select(x => new Setting
                    {
                        Id = x.SettingsId,
                        Name = x.Name,
                        Value = x.Value
                    }).ToList();
                    cacheService.Set(CacheConstants.SETTINGS_CACHE, settings, TimeSpan.FromMinutes(CacheConstants.SETTINGS_CACHE_EXPIRY_MINUTES));
                }
            }
            catch (Exception exception)
            {
                resolver.Get<ILogService>().Error("Error downloading settings", exception);
            }
            return settings ?? [];
        }
    }
}
