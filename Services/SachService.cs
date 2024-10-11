using HuynhKom_lab00_bansach.Models.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HuynhKom_lab00_bansach.Services
{

    public interface ISachService
    {
        Task<dynamic> getDataSach(int idcd, int idnxb);
    }

    public class SachService : ISachService
    {
        private readonly QuanLySachContext _context;

        public SachService(QuanLySachContext context)
        {
            _context = context;
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
