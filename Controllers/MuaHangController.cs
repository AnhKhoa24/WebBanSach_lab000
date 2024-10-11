using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuynhKom_lab00_bansach.Controllers
{
  
    public class MuaHangController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return Ok("hello");
        }
    }
}
