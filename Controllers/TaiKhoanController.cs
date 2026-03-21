using KhoaNoiVuCNTT.Data;
using KhoaNoiVuCNTT.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KhoaNoiBo_CNTT.Controllers
{
    [AllowAnonymous]
    public class TaiKhoanController : Controller
    {
        private readonly ThongTinNoiBoContext _context;

        public TaiKhoanController(ThongTinNoiBoContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string matkhau)
        {
            var canBo = _context.CanBo
                .FirstOrDefault(c => c.Email == email && c.MatKhau == matkhau);

            if (canBo != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, canBo.HoTen),
                    new Claim(ClaimTypes.Email, canBo.Email),
                    new Claim("MaCanBo", canBo.MaCanBo.ToString()),
    
                    new Claim(ClaimTypes.Role, canBo.MaVaiTro == 1 ? "Admin" : "User")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                // 🎯 PHÂN LUỒNG
                if (canBo.MaVaiTro == 1)
                {
                    return RedirectToAction("Index", "Trangchu", new { area = "Admin" });
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc Mật khẩu không chính xác!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "TaiKhoan");
        }

        public IActionResult AccessDenied()
        {
            return View(); // nếu đã có view
        }
        public IActionResult Register()
        {
            return View("~/Views/TaiKhoan/Register.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> Register(string hoten, string email, string matkhau)
        {
            // Kiểm tra email đã tồn tại chưa
            var check = _context.CanBo.FirstOrDefault(x => x.Email == email);

            if (check != null)
            {
                ViewBag.Error = "Email đã tồn tại!";
                return View();
            }

            // Tạo cán bộ mới
            var user = new CanBo
            {
                HoTen = hoten,
                Email = email,
                MatKhau = matkhau,
                MaVaiTro = 2 // 👈 mặc định User
            };

            _context.CanBo.Add(user);
            await _context.SaveChangesAsync();

            // 👉 chuyển về login
            return RedirectToAction("Login");
        }
    }
}