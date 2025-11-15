using System.Net;
using EzeePdf.Core.Model.Users;
using EzeePdf.Core.Services.DI;
using Microsoft.AspNetCore.Http;

namespace EzeePdf.Core
{
    public class ServiceLocator
    {
        private static IServiceResolver? provider;
        public static void SetProvider(IServiceResolver? provider)
        {
            ServiceLocator.provider = provider;
        }
        public static T Get<T>() where T : class
        {
            return provider!.Get<T>();
        }
        public static WebUser User
        {
            get
            {
                if (HttpContext is not null && HttpContext.Items.TryGetValue(Constants.CURRENT_HTTP_USER, out var user))
                {
                    return (user as WebUser) ?? WebUser.Default;
                }
                return WebUser.Default;
            }
            set
            {
                if (HttpContext is not null)
                {
                    HttpContext.Items[Constants.CURRENT_HTTP_USER] = value;
                }
            }
        }
        public static bool Admin => User.UserType == Enums.EnumUserType.Admin;
        public static bool Support => User.UserType == Enums.EnumUserType.Support;
        public static bool Public => User.UserType == Enums.EnumUserType.Public;
        public static bool Anonymous => User.UserType == Enums.EnumUserType.Anonymous;
        public static bool Staff => User.UserType == Enums.EnumUserType.Admin || User.UserType == Enums.EnumUserType.Support || User.UserType == Enums.EnumUserType.System;
        public static int? UserId => User?.UserId;
        public static HttpContext? HttpContext => provider!.HttpContext;
        public static string IpAddress => HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        public static string? HttpReferrer => HttpContext?.Request.Path;
        public static string ClientHost
        {
            get
            {
                string host;
                try
                {
                    IPHostEntry ipHostEntry = Dns.GetHostEntry(IpAddress);
                    host = ipHostEntry.HostName;
                }
                catch
                {
                    // give up and just return the IP
                    return WebUtility.HtmlEncode(IpAddress);
                }

                return WebUtility.HtmlEncode(host + " (" + IpAddress + ")");
            }
        }
    }
}
