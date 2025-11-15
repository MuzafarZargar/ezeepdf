using System.Threading.Channels;
using EzeePdf.Core.Model.Logging;

namespace EzeePdf.Core.Services.Logging
{
    public class LogService : ILogService
    {
        private readonly Serilog.ILogger logger;
        private readonly SqlLogger sqlLogger;
        private ChannelWriter<LogData> channelWriter;

        public LogService(Serilog.ILogger logger, SqlLogger sqlLogger)
        {
            this.logger = logger;
            this.sqlLogger = sqlLogger;

            var options = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            };

            var channel = Channel.CreateBounded<LogData>(options);
            var reader = channel.Reader;
            channelWriter = channel.Writer;

            Task.Factory.StartNew(ReadMessages, reader);
            Task.Factory.StartNew(ReadMessages, reader);
            Task.Factory.StartNew(ReadMessages, reader);
        }
        public void Debug(string message, params object?[]? args)
        {
            channelWriter.TryWrite(new LogData
            {
                Level = Serilog.Events.LogEventLevel.Debug,
                Message = message,
                Parameters = args
            });
        }
        public void Info(string message, params object?[]? args)
        {
            channelWriter.TryWrite(new LogData
            {
                Level = Serilog.Events.LogEventLevel.Information,
                Message = message,
                Parameters = args
            });
        }
        public void Warn(string message, params object?[]? args)
        {
            channelWriter.TryWrite(new LogData
            {
                Level = Serilog.Events.LogEventLevel.Warning,
                Message = message,
                Parameters = args
            });
        }
        public void Error(string message, Exception? exception, params object?[]? args)
        {
            channelWriter.TryWrite(new LogData
            {
                Level = Serilog.Events.LogEventLevel.Error,
                Message = message,
                Exception = exception,
                Parameters = args
            });
        }
        public void Error(string message, Exception? exception, string? ipAddress, string? httpUrl)
        {
            channelWriter.TryWrite(new LogData
            {
                Level = Serilog.Events.LogEventLevel.Error,
                Message = message,
                Exception = exception,
                IpAddress = ipAddress,
                Url = httpUrl,
            });
        }
        public void Error(Exception? exception)
        {
            channelWriter.TryWrite(new LogData
            {
                Level = Serilog.Events.LogEventLevel.Error,
                Exception = exception
            });
        }
        private async void ReadMessages(object? msgReader)
        {
            var reader = msgReader as ChannelReader<LogData>;
            if (reader is not null)
            {
                while (await reader.WaitToReadAsync())
                {
                    if (reader.TryRead(out var item))
                    {
                        if (item is not null)
                        {
                            switch (item.Level)
                            {
                                case Serilog.Events.LogEventLevel.Error:
                                case Serilog.Events.LogEventLevel.Fatal:
                                    await sqlLogger.LogException(item.Exception, item.Message, item.IpAddress, item.Url);
                                    break;
                                case Serilog.Events.LogEventLevel.Warning:
                                    logger.Warning(item.Message ?? string.Empty, item.Parameters);
                                    break;
                                case Serilog.Events.LogEventLevel.Information:
                                    logger.Information(item.Message ?? string.Empty, item.Parameters);
                                    break;
                                case Serilog.Events.LogEventLevel.Debug:
                                    logger.Debug(item.Message ?? string.Empty, item.Parameters);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
