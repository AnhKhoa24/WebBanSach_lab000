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
                return Unauthorized();
            }

            //// Thay thế bằng cách xác thực thực tế
            //if (model.Email == "test@example.com" && model.Password == "password")
            //{
            //    var token = GenerateJwtToken(model.Email);
            //    return Ok(new { Token = token });
            //}
            var token = GenerateJwtToken(check.Email!, check.HoTen!);
            return Ok(new { Token = token });
        }
        private string GenerateJwtToken(string email, string username)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("username", username) // Thêm claim cho tên tài khoản
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




    }
}
