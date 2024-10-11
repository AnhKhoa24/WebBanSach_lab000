using HuynhKom_lab00_bansach.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using HuynhKom_lab00_bansach.Services;
using HuynhKom_lab00_bansach.Models.Entities;

namespace HuynhKom_lab00_bansach.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISachService _services;
        private readonly QuanLySachContext _context;

        public HomeController(ILogger<HomeController> logger, ISachService services, QuanLySachContext context)
        {
            _logger = logger;
            _services = services;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ChuDes = await _context.Chudes.ToListAsync();
            ViewBag.NhaXuatBans = await _context.Nhaxuatbans.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>GetDSsach(int idcd, int idnxb)
        {
            return Ok(await _services.getDataSach(idcd, idnxb)); 
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
