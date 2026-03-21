using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class VaiTro
{
    public int MaVaiTro { get; set; }

    public string TenVaiTro { get; set; } = null!;

    public virtual ICollection<CanBo> CanBo { get; set; } = new List<CanBo>();

}
