using EzeePdf.Core.Model.Config;
using Serilog;
using Serilog.Events;

namespace EzeePdf.Core.Services.Logging
{
    public class SerilogConfigurator
    {
        internal static LogConfig? config;
        private const string OUTPUT_TEMPLATE = "[{Timestamp:dd-MM-yy HH:mm:ss} {Level:u3}] {Message}{NewLine}{Exception}";
        public static ILogger Configure(LogConfig config)
        {
            SerilogConfigurator.config = config;
            config.MaxSize = config.MaxSize * 1024 * 1024;

            var serLogger = new LoggerConfiguration()
                .Enrich.FromLogContext();

            var debugFile = Path.Combine(config.Path!, "debug.log");
            var infoFile = Path.Combine(config.Path!, "info.log");
            var warnFile = Path.Combine(config.Path!, "warn.log");
            var errorFile = Path.Combine(config.Path!, "error.log");

            serLogger.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
            .WriteTo.File(
                debugFile,
                outputTemplate: OUTPUT_TEMPLATE,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: config.MaxFiles,
                fileSizeLimitBytes: config.MaxSize));

            serLogger.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
            .WriteTo.File(
                infoFile,
                outputTemplate: OUTPUT_TEMPLATE,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: config.MaxFiles,
                fileSizeLimitBytes: config.MaxSize));

            serLogger.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
            .WriteTo.File(
                warnFile,
                outputTemplate: OUTPUT_TEMPLATE,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: config.MaxFiles,
                fileSizeLimitBytes: config.MaxSize));

            //serLogger.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
            //.WriteTo.MSSqlServer(
            //    config.SqlConnectionString,
            //    new MSSqlServerSinkOptions
            //    {
            //        AutoCreateSqlTable = false,
            //        TableName = "gblErrorLog",
            //    },
            //    columnOptions: new ColumnOptions
            //    {
            //        AdditionalColumns = new List<SqlColumn>
            //        {
            //            new SqlColumn("DateCreated", System.Data.SqlDbType.DateTime)
            //                {PropertyName = "DateCreated"},
            //            new SqlColumn("ErrorMessage", System.Data.SqlDbType.VarChar)
            //                {PropertyName = "ErrorMessage"},
            //            new SqlColumn("FullMessage", System.Data.SqlDbType.VarChar)
            //                {PropertyName = "FullMessage"},
            //            new SqlColumn("URL", System.Data.SqlDbType.VarChar)
            //                {PropertyName = "URL"},
            //            new SqlColumn("RemoteAddress", System.Data.SqlDbType.VarChar)
            //                { PropertyName = "RemoteAddress" },
            //            new SqlColumn("VersionNumber", System.Data.SqlDbType.VarChar)
            //                { PropertyName = "VersionNumber" },
            //            new SqlColumn("ServerName", System.Data.SqlDbType.VarChar)
            //                { PropertyName = "ServerName" },
            //            new SqlColumn("ServerUpTimeHr", System.Data.SqlDbType.Int)
            //                { PropertyName = "ServerUpTimeHr" }
            //        },
            //        PrimaryKey = new SqlColumn("ErrorLogId", System.Data.SqlDbType.Int),
            //    },
            //    restrictedToMinimumLevel: LogEventLevel.Error));
            switch (GetLogLevel(config.Level))
            {
                case LogEventLevel.Verbose:
                    serLogger.MinimumLevel.Verbose();
                    break;
                case LogEventLevel.Debug:
                    serLogger.MinimumLevel.Debug();
                    break;
                case LogEventLevel.Information:
                    serLogger.MinimumLevel.Information();
                    break;
                case LogEventLevel.Warning:
                    serLogger.MinimumLevel.Warning();
                    break;
                case LogEventLevel.Error:
                    serLogger.MinimumLevel.Error();
                    break;
                case LogEventLevel.Fatal:
                    serLogger.MinimumLevel.Fatal();
                    break;
            }

            return serLogger.CreateLogger();
        }
        private static LogEventLevel GetLogLevel(string? level)
        {
            if (level is not null && Enum.TryParse<LogEventLevel>(level, out var logLevel))
            {
                return logLevel;
            }

            return LogEventLevel.Error;
        }
    }
}
