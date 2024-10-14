using System;
using System.Collections.Generic;

namespace HuynhKom_lab00_bansach.Models.Entities;

public partial class TaiKhoanQuanLy
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Ten { get; set; }

    public string? Matkhau { get; set; }

    public string? Role { get; set; }
}
