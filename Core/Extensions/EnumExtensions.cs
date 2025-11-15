using System.ComponentModel;
using System.Reflection;

namespace EzeePdf.Core.Extensions
{
    public static class EnumExtensions
    {
        public static int Value<T>(this T source)
            where T : struct, Enum
        {
            return Convert.ToInt32(source);
        }
        public static string ValueStr<T>(this T source)
            where T : struct, Enum
        {
            return Convert.ToInt32(source).ToString();
        }
        public static T ToEnum<T>(this string source, T defaultValue = default)
            where T : struct, Enum
        {
            if (source is null || !Enum.TryParse<T>(source, out var value))
            {
                value = defaultValue;
            }
            return value;
        }
        public static T? ToEnum<T>(this int? source, T? defaultValue = default)
            where T : struct, Enum
        {
            T? value;
            if (source is null)
            {
                value = defaultValue;
            }

            var values = Enum.GetValues<T>();
            value = values.FirstOrDefault(v => Convert.ToInt32(v) == source!.Value);
            return value;
        }
        public static bool EnumContains<T>(this int source)
            where T : struct, Enum
        {
            var values = Enum.GetValues<T>();
            return values.Any(v => Convert.ToInt32(v) == source);
        }
        public static bool EnumContains<T>(this T source)
            where T : struct, Enum
        {
            var values = Enum.GetValues<T>();
            return values.Any(v => source.Equals(v));
        }
        public static TDesc? EnumDesc<TEnum, TDesc>(this TEnum value)
            where TEnum : struct, Enum
            where TDesc : Attribute
        {
            var members = typeof(TEnum).GetMember(value.ToString());
            if (members?.Any() == true)
            {
                return members[0].GetCustomAttribute<TDesc>() as TDesc;
            }
            return default;
        }
        public static string? EnumDesc<TEnum>(this TEnum value, bool defaultAsName = true)
            where TEnum : struct, Enum
        {
            var members = typeof(TEnum).GetMember(value.ToString());
            if (members?.Any() == true)
            {
                return members[0].GetCustomAttribute<DescriptionAttribute>()?.Description;
            }
            if (defaultAsName)
            {
                return value.ToString();
            }
            return default;
        }
        public static Dictionary<string, TEnum> EnumWithDescription<TEnum>(this TEnum source)
            where TEnum : struct, Enum
        {
            Dictionary<string, TEnum> response = [];
            foreach (var name in Enum.GetNames<TEnum>())
            {
                var value = Enum.Parse<TEnum>(name);
                var key = EnumDesc(value);
                if (key is not null)
                {
                    response.Add(key, value);
                }
            }
            return response;
        }
        public static IEnumerable<int> Values<T>(params T[] ignore)
        where T : struct, Enum
        {
            return Enum.GetValues<T>()
                .Where(x => !ignore.Contains(x))
                .Select(t => Convert.ToInt32(t));
        }
        public static string NamesString<T>(bool includeValue, params T[] ignore)
        where T : struct, Enum
        {
            return Enum.GetValues<T>()
                .Where(x => !ignore.Contains(x))
                .Select(x => x.ToString() + (includeValue ? $"({Convert.ToInt32(x)})" : string.Empty))
                .Aggregate((a, b) => $"{a},{b}");
        }
        public static Dictionary<int, string> NameValues<T>(params T[] ignore)
            where T : struct, Enum
        {
            return Enum.GetValues<T>()
                .Where(x => !ignore.Contains(x))
                .ToDictionary(x => Convert.ToInt32(x), v => v.ToString());
        }
        public static bool In<T>(this T value, params T[] options)
            where T : struct, Enum
        {
            return options.Contains(value);
        }
    }
}
