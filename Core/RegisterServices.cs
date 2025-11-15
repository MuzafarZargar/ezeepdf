using EzeePdf.Core.Model.Config;
using EzeePdf.Core.Repositories;
using EzeePdf.Core.Services;
using EzeePdf.Core.Services.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EzeePdf.Core
{
    public static class RegisterServices
    {
        public static void AddCoreService(this IServiceCollection source)
        {
            source.AddScoped<ILogService, LogService>();
            source.AddScoped<SqlLogger>();
            source.AddSingleton(options =>
            {
                var config = options.GetService<IOptions<LogConfig>>();
                return SerilogConfigurator.Configure(config!.Value);
            });

            source.AddScoped<IUserRepository, UserRepository>();
            source.AddScoped<IUserService, UserService>();

            source.AddScoped<IPdfUsageRepository, PdfUsageRepository>();
            source.AddScoped<IPdfUsageService, PdfUsageService>();

            source.AddScoped<IFeedbackRepository, FeedbackRepository>();
            source.AddScoped<IFeedbackService, FeedbackService>();

            source.AddScoped<ISettingsRepository, SettingsRepository>();
            source.AddScoped<ISettingsService, SettingsService>();

            source.AddScoped<IEzeePdfSqlRepository, EzeePdfSqlRepository>(x => new EzeePdfSqlRepository(AppConfig.Instance.EzeePdfDatabase.ConnectionString));
        }
    }
}