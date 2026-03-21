using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class BoMon
{
    public int MaBoMon { get; set; }

    public string TenBoMon { get; set; } = null!;

    public virtual ICollection<CanBo> CanBos { get; set; } = new List<CanBo>();
}
