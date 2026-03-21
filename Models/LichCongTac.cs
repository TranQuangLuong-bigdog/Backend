using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class LichCongTac
{
    public int MaLich { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? MoTa { get; set; }

    public DateTime ThoiGianBatDau { get; set; }

    public DateTime ThoiGianKetThuc { get; set; }

    public string DiaDiem { get; set; } = null!;

    public int? MaNguoiToChuc { get; set; }

    public virtual CanBo? MaNguoiToChucNavigation { get; set; }
}
