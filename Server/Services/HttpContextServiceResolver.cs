using System.Security.Claims;
using EzeePdf.Core;
using EzeePdf.Core.DB;
using EzeePdf.Core.Services;
using EzeePdf.Core.Services.DI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace EzeePdf.Services
{
    public class HttpContextServiceResolver(InternalResolver resolver) : IServiceResolver
    {
        private readonly IHttpContextAccessor contextAccessor = resolver.Get<IHttpContextAccessor>();
        public HttpContext? HttpContext => contextAccessor.HttpContext;
        public T Get<T>() where T : class
        {
            if (HttpContext is null)
            {
                return resolver.Get<T>();
            }
            return HttpContext?.RequestServices.GetService<T>()!;
        }
    }

    public class EzeePdfAuthenticationStateProvider(IUserService userService) : AuthenticationStateProvider
    {
        private readonly IUserService userService = userService;
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await userService.GetLoginInfo();
            return new AuthenticationState(user);
        }
        public async Task<string?> Signin(string emailAddress, string password)
        {
            var response = await userService.Login(new Core.Model.Users.LoginRequest
            {
                EmailAddress = emailAddress,
                Password = password
            });
            return response.ErrorMessage;            
        }
    }
}
