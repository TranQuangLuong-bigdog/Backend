using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class ThongBao
{
    public int MaThongBao { get; set; }

    public string TieuDe { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public DateTime? NgayDang { get; set; }

    public int? MaNguoiDang { get; set; }

    public virtual CanBo? MaNguoiDangNavigation { get; set; }
}
