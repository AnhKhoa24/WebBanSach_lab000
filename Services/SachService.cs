using HuynhKom_lab00_bansach.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HuynhKom_lab00_bansach.Services
{

    public interface ISachService
    {
        Task<dynamic> getDataSach(int idcd, int idnxb);
        Task<dynamic> getSachByID(int maSach);
        Task<dynamic> addToCart(int maKH, int ma_Sach, int sl);
        Task<dynamic> getCart(int maKH);
        Task<dynamic> removeCartItem(int idCart, int maKH);
        Task<dynamic> DatHang(int maKH);
        Task<dynamic> getInfo(int maKH);
        Task<dynamic> updateInfo(int maKh, string name, string email, string sdt, string diachi);
    }

    public class SachService : ISachService
    {
        private readonly QuanLySachContext _context;

        public SachService(QuanLySachContext context)
        {
            _context = context;
        }
        public async Task<dynamic> updateInfo(int maKh, string name, string email, string sdt, string diachi)
        {
            var get = await _context.Khachhangs.FindAsync(maKh);
            if (get == null)
            {
                return new
                {
                    flag = false,
                };
            }
            get.HoTen = name;
            get.Email = email;
            get.DienthoaiKh = sdt;
            get.DiachiKh = diachi;
            await _context.SaveChangesAsync();
            return new
            {
                flag = true,
            };

        }
        public async Task<dynamic> getInfo(int maKH)
        {
             return await _context.Khachhangs.FindAsync(maKH);
        }
        public async Task<dynamic> DatHang(int maKH)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cart = await _context.Carts.Where(x => x.MaKh == maKH).ToListAsync();
                if (!cart.Any())
                {
                    return new { flag = false, message = "Giỏ hàng trống" };
                }

                var dondathang = new Dondathang
                {
                    Ngaydat = DateOnly.FromDateTime(DateTime.Now),
                    Ngaygiao = DateOnly.FromDateTime(DateTime.Now).AddDays(3),
                    Tinhtranggiaohang = "Đã đặt hàng",
                    MaKh = maKH
                };
                await _context.Dondathangs.AddAsync(dondathang);
                await _context.SaveChangesAsync(); // Lưu để có MaDonHang

                var Ds = cart.Select(item => new Chitietdondathang
                {
                    MaDonHang = dondathang.MaDonHang,
                    Masach = item.MaSach,
                    Soluong = item.Sl,
                    Dongia = item.Gia
                }).ToList();

                await _context.Chitietdondathangs.AddRangeAsync(Ds);
                _context.Carts.RemoveRange(cart);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new { flag = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new { flag = false, message = ex.Message };
            }
        }
        public async Task<dynamic> removeCartItem(int idCart, int maKH)
        {
            var check =await _context.Carts.FindAsync(idCart);
            if(check == null)
            {
                return new
                {
                    flag = false,
                };
            }
            else
            {
                _context.Carts.Remove(check);
                await _context.SaveChangesAsync();
                var item = await _context.Carts.Where(x=>x.MaKh == maKH).CountAsync();
                return new
                {
                    flag = true,
                    sl = item
                };
            }  
        }
        public async Task<dynamic> getCart(int maKH)
        {
            var cart = await _context.Carts
                .Where(x => x.MaKh == maKH)
                .Select(t => new
                {
                    t.Id,
                    t.MaSachNavigation.Tensach,
                    t.MaSachNavigation.Anhbia,
                    t.Sl,
                    t.Gia,
                    t.Tongtien
                })
                .ToArrayAsync();
            return cart;
        }
        public async Task<dynamic> addToCart(int maKH, int ma_Sach, int sl)
        {
            var kq = await _context.Saches.FindAsync(ma_Sach);
            var check = await _context.Carts.FirstOrDefaultAsync(x => (x.MaKh == maKH && x.MaSach == ma_Sach));
            if (check != null)
            {
                check.Sl += sl;
                check.Tongtien = check.Sl * kq!.Giaban;
                await _context.SaveChangesAsync();
            }
            else
            {
                Cart newcart = new Cart();
                newcart.MaSach = ma_Sach;
                newcart.Sl = sl;
                newcart.Gia = kq!.Giaban;
                newcart.MaKh = maKH;
                newcart.Tongtien = sl * kq.Giaban;
                await _context.Carts.AddAsync(newcart);
                await _context.SaveChangesAsync();
            }    
 
            return new
            {
                statusCode = 200,
                message ="Thêm thành công"
            };
        }
        public async Task<dynamic> getSachByID(int maSach)
        {
            var kq = await _context.Saches.FindAsync(maSach);
            if (kq == null)
            {
                return new
                {
                    statusCode = 404,
                    detail = "Không có"
                };
            }
            else
            {
                return new
                {
                    statusCode = 200,
                    detail = kq
                }; 
            }

        }
        public async Task<dynamic> getDataSach(int idcd, int idnxb)
        {
            if(idcd == 0 && idnxb == 0)
            {
                return await _context.Saches.ToListAsync();
            }
            else
            {
                return await _context.Saches
                   .Where(x => (x.MaCd == idcd || idcd == 0) && (x.MaNxb == idnxb || idnxb == 0))
                   .ToListAsync();
            }   
            //var data = await _context.Saches
            //        .Include(t => t.MaCdNavigation) 
            //        .Include(t => t.Vietsacs)       
            //        .ThenInclude(vs => vs.MaTgNavigation) 
            //        .Select(t => new
            //        {
            //             TenChuDe = t.MaCdNavigation.TenChuDe,
            //             t.Anhbia,
            //             t.Giaban,
            //             t.MaNxbNavigation.TenNxb,
            //             t.Mota,
            //             t.Soluongton,
            //             Vietsacs = t.Vietsacs.Select(g => new
            //            {
            //                    TenTg = g.MaTgNavigation.TenTg 
            //            }).ToList() 
            //         })
            //        .ToListAsync(); 

        }
    }
}
