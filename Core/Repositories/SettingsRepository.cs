using EzeePdf.Core.DB;
using EzeePdf.Core.Enums;
using EzeePdf.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EzeePdf.Core.Repositories
{
    public class SettingsRepository(EzeepdfContext context) : ISettingsRepository
    {
        private readonly EzeepdfContext context = context;
        public Task<List<LuSetting>> GetAllSettings()
        {
            return context.LuSettings.ToListAsync();
        }
        public Task<LuSetting?> GetSetting(EnumSettings setting)
        {
            return context.LuSettings.FirstOrDefaultAsync(
                            x => x.SettingsId == setting.Value() ||
                            x.Name == setting.ToString());
        }
    }
}
