using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EzeePdf.Core.Extensions
{
    public static class StringExtensions
    {
        public static byte[] AsBytes(this string? source)
        {
            return source is null ? [] : Encoding.UTF8.GetBytes(source);
        }
        public static string FromBytes(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        public static string Hash256(this string source)
        {
            var sha = SHA256.Create();
            var hash = sha.ComputeHash(source.AsBytes());
            var buffer = new StringBuilder();
            foreach (var b in hash)
            {
                buffer.Append(b.ToString("x"));
            }
            return buffer.ToString();
        }
        public static int Int(this string? value, int defaultValue = 0)
        {
            return Utils.ParseInt(value, defaultValue);
        }
        public static double Double (this string? value, double defaultValue = 0)
        {
            return Utils.ParseDouble(value, defaultValue);
        }
        public static string AsJsonString(this object? value, bool toBase64 = false)
        {
            string json = string.Empty;
            if (value is not null)
            {
                try
                {
                    json = JsonSerializer.Serialize(value);
                    if (toBase64)
                    {
                        json = Convert.ToBase64String(json.AsBytes());
                    }
                    return json;
                }
                catch { }
            }
            return json;
        }
        public static T? FromJsonString<T>(this string? json, bool isBase64 = false)
        {
            T? response = default;
            if (json is not null)
            {
                try
                {
                    if (isBase64)
                    {
                        json = FromBytes(Convert.FromBase64String(json));
                    }
                    response = JsonSerializer.Deserialize<T>(json);
                }
                catch { }
            }
            return response;
        }
    }
}