using EzeePdf.Core.DB;
using EzeePdf.Core.Enums;

namespace EzeePdf.Core.Repositories
{
    public interface ISettingsRepository
    {
        Task<List<LuSetting>> GetAllSettings();
        Task<LuSetting?> GetSetting(EnumSettings setting);
    }
}