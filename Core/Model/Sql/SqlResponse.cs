namespace EzeePdf.Core.Model.Sql
{
    public class SqlResponse<T>(T? response) : IDisposable
    {
        public T? Response { get; set; } = response;

        public Dictionary<string, object>? Output { get; set; }
        public string? GetString(string name, string? @default = null)
        {
            if (Output != null && Output.TryGetValue(name, out var value))
            {
                return value?.ToString();
            }

            return @default;
        }
        public DateTime GetDateTIme(string name, DateTime @default)
        {
            DateTime date = @default;
            if (Output != null && Output.TryGetValue(name, out var value))
            {
                if (value is null || !DateTime.TryParse(value.ToString(), out date))
                {
                    date = @default;
                }
            }

            return date;
        }
        public int? GetIntNull(string name, int? @default = null)
        {
            if (Output != null && Output.TryGetValue(name, out var value))
            {
                return Utils.ParseIntNull(value, @default);
            }

            return @default;
        }
        public int GetInt(string name, int @default = 0)
        {
            if (Output != null && Output.TryGetValue(name, out var value))
            {
                return Utils.ParseInt(value, @default);
            }
            return @default;
        }
        public double GetDouble(string name, double @default = 0)
        {
            if (Output != null && Output.TryGetValue(name, out var value))
            {
                return Utils.ParseDouble(value, @default);
            }

            return @default;
        }
        public decimal GetDecimal(string name, decimal @default = 0)
        {
            if (Output != null && Output.TryGetValue(name, out var value))
            {
                return Utils.ParseDecimal(value, @default);
            }

            return @default;
        }

        public bool GetBoolean(string name, bool @default = false)
        {
            if (Output != null && Output.TryGetValue(name, out var value))
            {
                if (value != null)
                {
                    if (bool.TryParse(value.ToString(), out var boolValue))
                    {
                        return boolValue;
                    }
                }
            }

            return @default;
        }
        public void Dispose()
        {
            if (Response != null && Response is IDisposable d)
            {
                d.Dispose();
            }
        }

        public static implicit operator T?(SqlResponse<T> response) => response.Response;
    }
}
