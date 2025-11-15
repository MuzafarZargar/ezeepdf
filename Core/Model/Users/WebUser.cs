using EzeePdf.Core.Enums;

namespace EzeePdf.Core.Model.Users
{
    public class WebUser
    {
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public EnumUserType UserType { get; set; }
        public bool Empty { get; set; }
        public static WebUser Default => new()
        {
            Empty = true,
            Email = string.Empty,
            UserType = EnumUserType.Anonymous,
        };
    }
}
