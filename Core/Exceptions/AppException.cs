using System;
using System.Text;
using EzeePdf.Core.Extensions;
using EzeePdf.Core.Responses;

namespace EzeePdf.Core.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message, Exception exception, EnumResponseCode code) : base(message, exception)
        {
            Code = code;
        }
        public AppException(EnumResponseCode code, Exception exception) : base(code.ToString(), exception)
        {
            Code = code;
        }
        public AppException(EnumResponseCode code, Exception exception, object info)
            : base(code.EnumDesc(true), exception)
        {
            Code = code;
            Info = info;
        }
        public AppException(string message, EnumResponseCode code) : base($"{code.EnumDesc(true)}-{message}")
        {
            Code = code;
        }
        public AppException(EnumResponseCode code) : base(code.EnumDesc(true))
        {
            Code = code;
        }
        public AppException(EnumResponseCode code, object? info) : base(code.EnumDesc(true))
        {
            Code = code;
            Info = info;
        }
        public EnumResponseCode Code { get; set; }
        public object? Info { get; set; }
        public Dictionary<string, string?>? AdditionalInfo { get; set; }
        public bool LogException { get; set; }

        public static AppException From(string message, Exception exception, EnumResponseCode code)
        {
            return new AppException(message, exception, code);
        }
        public static AppException From(EnumResponseCode code, Exception exception)
        {
            return new AppException(code, exception);
        }
        public static AppException From(EnumResponseCode code, Exception exception, object info)
        {
            return new AppException(code, exception, info);
        }
        public static AppException From(string message, EnumResponseCode code)
        {
            return new AppException(message, code);
        }
        public static AppException From(EnumResponseCode code)
        {
            return new AppException(code);
        }
        public static AppException From(EnumResponseCode code, object? info)
        {
            return new AppException(code, info);
        }
        public static AppException Log(string message, Exception exception, EnumResponseCode code)
        {
            return new AppException(message, exception, code) { LogException = true };
        }
        public static AppException Log(EnumResponseCode code, Exception exception)
        {
            return new AppException(code, exception) { LogException = true };
        }
        public static AppException Log(EnumResponseCode code, Exception exception, object info)
        {
            return new AppException(code, exception, info) { LogException = true };
        }
        public static AppException Log(string message, EnumResponseCode code)
        {
            return new AppException(message, code) { LogException = true };
        }
        public static AppException Log(EnumResponseCode code)
        {
            return new AppException(code) { LogException = true };
        }
        public static AppException Log(EnumResponseCode code, object? info)
        {
            return new AppException(code, info) { LogException = true };
        }
    }

    public static class ExceptionExtensions
    {
        public static string GetExceptionDetails(this Exception? exception)
        {
            if (exception is null)
            {
                return string.Empty;
            }
            StringBuilder buffer = new();
            if (exception is AggregateException aggregateException)
            {
                foreach (var ex in aggregateException.InnerExceptions)
                {
                    var agg = GetExceptionDetails(ex);
                    buffer.AppendLine($"Exception: {agg}");
                }
            }
            else
            {
                buffer.AppendLine(exception.Message);
                int index = 1;
                while (exception.InnerException is not null)
                {
                    buffer.AppendLine($"Inner exception {index++}: {exception.InnerException}");
                    exception = exception.InnerException;
                }

                buffer.AppendLine(exception.StackTrace);
            }
            return buffer.ToString();
        }
        public static bool DuplicateKey(this Exception exception, Func<Exception, bool>? condition = null)
        {
            return Contains(exception, "Violation of UNIQUE KEY") &&
                (condition is null || condition.Invoke(exception));
        }
        public static bool Contains(this Exception exception, string value)
        {
            if (exception.Message.Contains("Violation of UNIQUE KEY", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (exception.InnerException is not null)
            {
                return Contains(exception.InnerException, value);
            }
            return false;
        }

    }
}
