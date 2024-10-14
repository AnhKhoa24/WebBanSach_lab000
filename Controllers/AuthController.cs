using HuynhKom_lab00_bansach.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HuynhKom_lab00_bansach.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly QuanLySachContext _context;

        public AuthController(IConfiguration configuration, QuanLySachContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(string name, string password)
        {
            var check = await _context.Khachhangs.FirstOrDefaultAsync(x => (x.Taikhoan == name || x.Email == name) && (x.Matkhau == password));
            if (check == null)
            {
                return Unauthorized(new{
                    message ="Đăng nhập thất bại!"
                });
            }
            var token = GenerateJwtToken(check.Email!, check.HoTen!, check.MaKh);

            //Response.Cookies.Append("jwtToken", token, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true, // Chỉ gửi cookie qua HTTPS
            //    SameSite = SameSiteMode.None // Cho phép cookie được gửi từ miền khác
            //});

            return Ok(new { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(string name, string password)
        {
            var check = await _context.TaiKhoanQuanLies.FirstOrDefaultAsync(x => (x.Name == name) && (x.Matkhau == password));
            if (check == null)
            {
                return Unauthorized(new
                {
                    message = "Đăng nhập thất bại!"
                });
            }
            var token = GenerateJwtTokenAdmin(check.Name!, check.Ten!, check.UserId, check.Role!);
            Response.Cookies.Append("jwtTokenAdmin", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Chỉ gửi cookie qua HTTPS
                SameSite = SameSiteMode.None // Cho phép cookie được gửi từ miền khác
            });
            return Ok(new { Token = token });
        }


        private string GenerateJwtToken(string email, string username, int userId)
        {
            var claims = new[]
            {
             new Claim(JwtRegisteredClaimNames.Sub, email),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim("username", username),
             new Claim("maKH", userId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJwtTokenAdmin(string name, string ten, int userId, string role)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("ten", ten),
            new Claim("UserID", userId.ToString()),
            new Claim(ClaimTypes.Role, role) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
