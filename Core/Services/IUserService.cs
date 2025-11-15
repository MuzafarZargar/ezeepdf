using System.Security.Claims;
using EzeePdf.Core.Model.Users;
using EzeePdf.Core.Responses;

namespace EzeePdf.Core.Services
{
    public interface IUserService
    {
        Task<DataResponse> CreateUser(NewUser user);
        Task<DataResponse<string>> Login(LoginRequest request);
        void SaveLoginInfo(string json);
        Task<bool> IsUserLoggedIn();
        Task<ClaimsPrincipal> GetLoginInfo(bool saveInfo = true);
        string? LogoutFromBrowser();
        Task LogoutFromServer(string token);
    }
}
