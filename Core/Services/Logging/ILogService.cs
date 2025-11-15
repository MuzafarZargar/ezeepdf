namespace EzeePdf.Core.Services.Logging
{
    public interface ILogService
    {
        void Debug(string message, params object?[]? args);
        void Info(string message, params object?[]? args);
        void Warn(string message, params object?[]? args);
        void Error(string message, Exception? exception, params object?[]? args);
        public void Error(string message, Exception? exception, string? ipAddress, string? httpUrl);
        void Error(Exception? exception);
    }
}
