namespace EzeePdf.Core.Model.Config
{
    public class AppConfig
    {
        public Server EzeePdfHost { get; set; } = null!;
        public Jwt EzeePdfJwt { get; set; } = null!;
        public Database EzeePdfDatabase { get; set; } = null!;
        public LogConfig EzeePdfLogConfig { get; set; } = null!;
        public static AppConfig Instance { get; set; } = null!;
    }
}
