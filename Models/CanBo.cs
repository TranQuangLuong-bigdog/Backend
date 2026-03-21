using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class CanBo
{
    public int MaCanBo { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public int? MaBoMon { get; set; }

    public int? MaVaiTro { get; set; }

    public virtual ICollection<LichCongTac> LichCongTacs { get; set; } = new List<LichCongTac>();

    public virtual BoMon? MaBoMonNavigation { get; set; }

    public virtual VaiTro? MaVaiTroNavigation { get; set; }

    public virtual ICollection<TaiLieu> TaiLieus { get; set; } = new List<TaiLieu>();

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
