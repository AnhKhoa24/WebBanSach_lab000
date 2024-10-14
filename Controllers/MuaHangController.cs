using HuynhKom_lab00_bansach.Models.Entities;
using HuynhKom_lab00_bansach.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HuynhKom_lab00_bansach.Controllers
{
  
    public class MuaHangController : Controller
    {
        private readonly QuanLySachContext _context;
        private readonly ISachService _service;

        public MuaHangController(QuanLySachContext context, ISachService service)
        {
            _context = context;
            _service = service;
        }
        [Authorize]
        public IActionResult Index()
        {
            return Ok("hello");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult>addToCart(int ma_Sach, int sl)
        {

            var userId = User.FindFirst("maKH")?.Value;
            //var userName = User.FindFirst("username")?.Value;
            int MaKhId;
            if (int.TryParse(userId, out MaKhId))
            {
                return Ok(await _service.addToCart(MaKhId, ma_Sach, sl));
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> getCart()
        {

            var userId = User.FindFirst("maKH")?.Value;
            //var userName = User.FindFirst("username")?.Value;
            int MaKhId;
            if (int.TryParse(userId, out MaKhId))
            {
                return Ok(await _service.getCart(MaKhId));
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> removeCartItem(int idCart)
        {
            var userId = User.FindFirst("maKH")?.Value;
            //var userName = User.FindFirst("username")?.Value;
            int MaKhId;
            if (int.TryParse(userId, out MaKhId))
            {
                var remove = await _service.removeCartItem(idCart, MaKhId);
                if (remove.flag)
                {
                    return Ok(remove.sl);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
            

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Order()
        {
            var userId = User.FindFirst("maKH")?.Value;
            //var userName = User.FindFirst("username")?.Value;
            int MaKhId;
            if (int.TryParse(userId, out MaKhId))
            {
                return Ok(await _service.DatHang(MaKhId));
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> getInfo()
        {
            var userId = User.FindFirst("maKH")?.Value;
            //var userName = User.FindFirst("username")?.Value;
            int MaKhId;
            if (int.TryParse(userId, out MaKhId))
            {
                return Ok(await _service.getInfo(MaKhId));
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> updateInfo(string name, string email, string sdt, string diachi)
        {
            var userId = User.FindFirst("maKH")?.Value;
            int MaKhId;
            if (int.TryParse(userId, out MaKhId))
            {
               var update = await _service.updateInfo(MaKhId, name, email, sdt, diachi);
                if(update.flag)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }  
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }

        }
    }
}
