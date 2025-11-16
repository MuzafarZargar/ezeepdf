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
            var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            userSessionService.IpAddress = ip ?? "127.0.0.1";
            return Task.CompletedTask;
        }
    }
}
