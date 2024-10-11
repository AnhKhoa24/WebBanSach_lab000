using System;
using System.Collections.Generic;

namespace HuynhKom_lab00_bansach.Models.Entities;

public partial class Chitietdondathang
{
    public int MaDonHang { get; set; }

    public int Masach { get; set; }

    public int? Soluong { get; set; }

    public decimal? Dongia { get; set; }

    public virtual Dondathang MaDonHangNavigation { get; set; } = null!;

    public virtual Sach MasachNavigation { get; set; } = null!;
}
