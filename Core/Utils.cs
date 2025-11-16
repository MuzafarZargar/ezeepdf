using EzeePdf.Core.Enums;
using EzeePdf.Core.Model.Config;
using EzeePdf.Core.Responses;
using EzeePdf.Core.Services;

namespace EzeePdf.Core
{
    public static class Utils
    {
        public static DateTime UtcNow => DateTime.UtcNow;
        public static DateTime UtcDay => DateTime.UtcNow.Date;
        public static DateTime Now => DateTime.Now;
        public static string HostName => Environment.MachineName;
        public static string IpAddress(IUserSessionService userSessionService)
        {
            string? ipAddress = default;
            try
            {
                ipAddress = userSessionService.IpAddress;
            }
            catch { }
            return ipAddress ?? "localhost";
        }
        public async static Task<EnumResponseCode> BlockForTime(
            IUserSessionService userSessionService,
            int consecutiveTime,
            Func<string, Task<DateTime>> prevDate)
        {
            var ipAddress = IpAddress(userSessionService);
            var prevUsageDate = await prevDate(ipAddress);
            return ApplyTimeLimit(prevUsageDate, consecutiveTime);
        }
        public async static Task<EnumResponseCode> BlockForTime(
            IUserSessionService userSessionService,
            EnumPdfFunction usageType,
            int consecutiveTime,
            Func<string, EnumPdfFunction, Task<DateTime>> prevDate)
        {
            var ipAddress = IpAddress(userSessionService);
            var prevUsageDate = await prevDate(ipAddress, usageType);
            return ApplyTimeLimit(prevUsageDate, consecutiveTime);
        }

        public static EnumResponseCode ApplyTimeLimit(DateTime prevUsageDate, int consecutiveTime)
        {
            if ((UtcNow - prevUsageDate).TotalMinutes < consecutiveTime)
            {
                return EnumResponseCode.RateLimitApplied;
            }
            return EnumResponseCode.Success;
        }
        public static int ParseInt(string? value, int defaultValue = 0)
        {
            if (value is not null && int.TryParse(value, out var v))
            {
                return v;
            }
            return defaultValue;
        }
        public static int ParseInt(object? value, int defaultValue = 0)
        {
            return ParseInt(value?.ToString(), defaultValue);
        }
        public static int? ParseIntNull(string? value, int? defaultValue = null)
        {
            if (value is not null && int.TryParse(value, out var v))
            {
                return v;
            }
            return defaultValue;
        }
        public static int? ParseIntNull(object? value, int? defaultValue = null)
        {
            return ParseIntNull(value?.ToString(), defaultValue);
        }
        public static decimal ParseDecimal(object value, decimal defaultValue = 0M)
        {
            if (value != null && decimal.TryParse(value.ToString(), out var v))
            {
                return v;
            }
            return defaultValue;
        }
        public static double ParseDouble(object value, double defaultValue = 0)
        {
            if (value != null && double.TryParse(value.ToString(), out var v))
            {
                return v;
            }
            return defaultValue;
        }
        public static bool ParseBool(object value, bool defaultValue = false)
        {
            if (value != null && bool.TryParse(value.ToString(), out var v))
            {
                return v;
            }
            return defaultValue;
        }

        public static void PrintMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void PrintError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }
        public static void PrintOk(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nOK\t");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{msg}\n\n");
            Console.ResetColor();
        }

    }
}
