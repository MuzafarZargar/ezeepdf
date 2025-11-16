using EzeePdf.Core.Services;

namespace EzeePdf.Middleware
{
    public class IpMiddleware
    {
        private readonly RequestDelegate next;

        public IpMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userSessionService = context.RequestServices.GetRequiredService<IUserSessionService>();
            string? ip = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(ip) || ip == "::1" || ip == "127.0.0.1")
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
                {
                    ip = forwarded.ToString().Split(',').First().Trim();
                }
            }

            userSessionService.IpAddress = ip ?? "127.0.0.1";

            await next(context);
        }
    }
}
