using EzeePdf.Core.Services;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace EzeePdf.Services
{
    public class AppCircuitHandler : CircuitHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserSessionService userSessionService;

        public AppCircuitHandler(IHttpContextAccessor httpContextAccessor, IUserSessionService userSessionService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.userSessionService = userSessionService;
        }
        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            string? ip = null;
            if (_httpContextAccessor.HttpContext is not null)
            {
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrWhiteSpace(ip) || ip == "::1")
                {
                    if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
                    {
                        ip = forwarded.ToString().Split(',').First().Trim();
                    }
                }
            }
            userSessionService.IpAddress = ip ?? "127.0.0.1";
            return Task.CompletedTask;
        }
    }
}
