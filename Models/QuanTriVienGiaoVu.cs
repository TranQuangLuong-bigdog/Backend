using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class QuanTriVienGiaoVu
{
    public int MaNhanVien { get; set; }

    public string TenNhanVien { get; set; } = null!;

    public int? Tuoi { get; set; }

    public string? GioiTinh { get; set; }

    public string ChucVu { get; set; } = null!;
}
