using EzeePdf.Core.DB;
using EzeePdf.Core.Model.Users;
using EzeePdf.Core.Responses;

namespace EzeePdf.Core.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(NewUser request);
        Task<(EnumResponseCode Code, User? User)> Login(LoginRequest request);
        Task SaveRefreshToken(string token, TokenData data);
        Task<RefreshToken?> GetRefreshToken(int userId, string sourceDevice);
        Task<bool> IsUserLocked(int userId);
    }
}
