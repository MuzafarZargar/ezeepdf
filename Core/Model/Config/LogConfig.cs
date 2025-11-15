using System.Text.Json.Serialization;

namespace EzeePdf.Core.Model.Config
{
    public class LogConfig
    {
        public string Path { get; set; } = null!;
        public string Level { get; set; } = null!;
        public long MaxSize { get; set; }
        public int MaxFiles { get; set; }

        [JsonIgnore]
        public string? SqlConnectionString { get; set; }

        [JsonIgnore]
        public string? Version { get; set; }
    }
}
