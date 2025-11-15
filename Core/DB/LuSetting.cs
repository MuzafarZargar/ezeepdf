using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class LuSetting
{
    public int SettingsId { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }
}
