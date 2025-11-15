using Serilog.Events;

namespace EzeePdf.Core.Model.Logging
{
    public class LogData
    {
        public LogEventLevel Level { get; set; }
        public string? Message { get; set; }
        public object?[]? Parameters { get; set; }
        public Exception? Exception { get; set; }
        public string? Url { get; set; }
        public string? IpAddress { get; set; }
    }

}
