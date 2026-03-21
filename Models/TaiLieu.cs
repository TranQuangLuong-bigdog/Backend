using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class TaiLieu
{
    public int MaTaiLieu { get; set; }

    public string TenTaiLieu { get; set; } = null!;

    public string DuongDanFile { get; set; } = null!;

    public int? MaNguoiTaiLen { get; set; }

    public DateTime? NgayTaiLen { get; set; }

    public string PhanLoai { get; set; } = null!;

    public virtual CanBo? MaNguoiTaiLenNavigation { get; set; }
}
