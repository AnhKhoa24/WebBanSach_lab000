using System;
using System.Collections.Generic;

namespace HuynhKom_lab00_bansach.Models.Entities;

public partial class Dondathang
{
    public int MaDonHang { get; set; }

    public DateOnly? Ngaydat { get; set; }

    public DateOnly? Ngaygiao { get; set; }

    public string? Tinhtranggiaohang { get; set; }

    public int? MaKh { get; set; }

    public virtual ICollection<Chitietdondathang> Chitietdondathangs { get; set; } = new List<Chitietdondathang>();

    public virtual Khachhang? MaKhNavigation { get; set; }
}
