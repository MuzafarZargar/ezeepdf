using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class LuUserType
{
    public int UserTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
