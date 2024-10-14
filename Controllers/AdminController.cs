using HuynhKom_lab00_bansach.Middleware;
using HuynhKom_lab00_bansach.Models.Entities;
using HuynhKom_lab00_bansach.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HuynhKom_lab00_bansach.Controllers
{
    public class AdminController : Controller
    {
        private readonly QuanLySachContext _context;
        private readonly IQuanLyService _service;


        public AdminController(QuanLySachContext context, IQuanLyService service)
        {
            _context = context;
            _service = service;
        }
        public IActionResult Login()
        {
            return View();
        }

        [QuanLy]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetTacGia(string search)
        {
            var tim = search == null ? "" : search.Trim();
            var tacgiacs = await _context.Tacgia.
                Where(x=>(x.TenTg!.Contains(tim)||tim == ""))
                .ToListAsync();
            return Ok(tacgiacs);
        }
        [HttpPost]
        public async Task<IActionResult> GetNhaXuatBan(string search)
        {
            var tim = search == null ? "" : search.Trim();
            var tacgiacs = await _context.Nhaxuatbans.
                Where(x => (x.TenNxb!.Contains(tim) || tim == ""))
                .ToListAsync();
            return Ok(tacgiacs);
        }

        [HttpPost]
        public async Task<IActionResult> getDonHang(int trang, int pagesize, string search)
        {
            return Ok(await _service.getDonHang(trang, pagesize, search));
        }

        [QuanLy]
        public IActionResult QuanLySach()
        {
            return View();
        }

        [QuanLy]
        public IActionResult DonHang()
        {
            return View();
        }

        [QuanLy]
        [HttpPost]
        public async Task<IActionResult> Sach(int trang, int pagesize, string search)
        {
            return Ok(await _service.SachService(trang, pagesize, search));
        }
    }
}
