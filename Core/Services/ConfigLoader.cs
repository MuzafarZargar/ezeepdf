using System.Text;
using EzeePdf.Core.Model.Config;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Serilog.Events;

namespace EzeePdf.Core.Services
{
    public static class ConfigLoader
    {
        public static void Load(Func<Type, object?> provider)
        {
            Utils.PrintMessage("Loading configuration ...\n");
            AppConfig config = new();

            var host = (IOptions<Server>?)provider(typeof(IOptions<Server>));
            var database = (IOptions<Database>?)provider(typeof(IOptions<Database>));
            var jwt = (IOptions<Jwt>?)provider(typeof(IOptions<Jwt>));
            var logger = (IOptions<LogConfig>?)provider(typeof(IOptions<LogConfig>));

            config.EzeePdfHost = ValidateHost(host);
            config.EzeePdfDatabase = ValidateDatabase(database);
            config.EzeePdfJwt = ValidateJwt(jwt);
            config.EzeePdfLogConfig = ValidateLogger(logger);

            AppConfig.Instance = config;
        }
        private static Server ValidateHost(IOptions<Server>? host)
        {
            Utils.PrintMessage("Validating host settings ...");

            if (host is null || host.Value is null)
            {
                throw new Exception("Host configuration is missing");
            }

            var server = host.Value;

            if (string.IsNullOrWhiteSpace(server.RuntimeKey))
            {
                Throw("Runtime key is missing");
            }
            if (string.IsNullOrWhiteSpace(server.Url))
            {
                Throw("Url is missing");
            }
            if (!Uri.TryCreate(server.Url, UriKind.Absolute, out var _))
            {
                Throw("Invalid Url");
            }
            Utils.PrintOk("Database");
            return server;
        }
        private static Database ValidateDatabase(IOptions<Database>? database)
        {
            Utils.PrintMessage("Validating database settings ...");

            if (database is null || database.Value is null)
            {
                throw new Exception("Database configuration is missing");
            }

            var db = database.Value;

            if (string.IsNullOrWhiteSpace(db.ConnectionString))
            {
                Throw("Database Connection string is missing");
            }

            using (var conn = new SqlConnection(db.ConnectionString))
            {
                conn.Open();
                conn.Close();
            }

            Utils.PrintOk("Database");
            return database.Value;
        }
        private static Jwt ValidateJwt(IOptions<Jwt>? jwtSettings)
        {
            Console.WriteLine("Validating Jwt settings ...");

            if (jwtSettings == null || jwtSettings.Value is null)
            {
                throw new Exception("JwtSettings missing");
            }

            var jwt = jwtSettings.Value;

            GuardNull(jwt.Issuer, "JwtSettings: Issuer missing");
            GuardNull(jwt.Audience, "JwtSettings: Audience missing");

            if (jwt.AccessTokenExpiryMinutes <= 0)
            {
                Throw("JwtSettings: Invalid value for AccessTokenExpiryMinutes, expected a value greater than zero");
            }

            if (jwt.RefreshTokenExpiryHours <= 0)
            {
                Throw("JwtSettings: Invalid value for RefreshTokenExpiryHours, expected a value greater than zero");
            }

            if (jwt.RefreshTokenExpiryHours * 60 <= jwt.AccessTokenExpiryMinutes)
            {
                Throw("JwtSettings: Refresh Token should expire after Access Token");
            }

            GuardNull(jwt.AccessTokenSigningKey, $"JwtSettings: SigningKey is missing from environment variables");

            if (jwt.AccessTokenSigningKey!.Contains(" "))
            {
                Throw("JwtSettings: Signing key should not contain spaces");
            }
            if (jwt.AccessTokenSigningKey!.Length < 20)
            {
                Throw("JwtSettings: Signing key should be at least 20 characters long");
            }

            jwt.AccessTokenSigningKeyBytes = Encoding.UTF8.GetBytes(jwt.AccessTokenSigningKey);

            Utils.PrintOk("Jwt Settings");

            return jwt;
        }
        public static LogConfig ValidateLogger(IOptions<LogConfig>? logSettings)
        {
            Console.WriteLine("Validating Logger settings ...");

            if (logSettings is null || logSettings.Value is null)
            {
                throw new Exception("Logger settings missing");
            }

            var logger = logSettings.Value;

            GuardNull(logger.Level, "Logger: LogLevel is missing");

            if (!Enum.TryParse<LogEventLevel>(logger.Level, out var _))
            {
                Throw($"Logger: Invalid value ({logger.Level}) for LogLevel\n" +
                    "Exptected values are:-\n" +
                    $"1. {LogEventLevel.Debug}\n" +
                    $"2. {LogEventLevel.Information}\n" +
                    $"3. {LogEventLevel.Warning}\n" +
                    $"4. {LogEventLevel.Error}\n" +
                    $"5. {LogEventLevel.Fatal}\n");
            }

            GuardNull(logger.Path, "Logger: Path is missing");

            if (logger.MaxFiles <= 0 || logger.MaxFiles > 10)
            {
                throw new Exception($"Logger: Invalid MaxFiles value {logger.MaxFiles}, specify a value between [1 and 10]");
            }

            if (logger.MaxSize <= 0 || logger.MaxSize > 50)
            {
                throw new Exception($"Logger: Invalid MaxSize value {logger.MaxSize}, specify a value between [1 and 50] MB");
            }

            Utils.PrintOk("Log settings");
            return logger;
        }

        private static void Throw(string message)
        {
            throw new Exception(message);
        }
        public static void GuardNull(string? entry, string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(entry))
            {
                Throw(errorMsg);
            }
        }

    }
}
