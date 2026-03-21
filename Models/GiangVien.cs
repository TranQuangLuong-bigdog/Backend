using System;
using System.Collections.Generic;

namespace KhoaNoiVuCNTT.Models;

public partial class GiangVien
{
    public int MaGiangVien { get; set; }

    public string? TenGiangVien { get; set; }

    public int? Tuoi { get; set; }

    public string? GioiTinh { get; set; }

    public string? MonDay { get; set; }
}
