using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelWeb.Data;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly TravelDbContext _context;

        public AccountController(TravelDbContext context)
        {
            _context = context;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            var hash = HashPassword(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);

            if (user == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            if (user.Status == "banned")
            {
                ViewBag.Error = "Tài khoản của bạn đã bị khóa!";
                return View();
            }

            var roleName = user.RoleId == 1 ? "Admin" : "Customer";

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, roleName)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            TempData["Success"] = $"Xin chào {user.FullName}!";

            // Admin → vào Admin Dashboard
            if (user.RoleId == 1)
            {
                return RedirectToAction("Index", "Admin");
            }

            // Customer → về trang gốc (nếu có) hoặc trang chủ
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                ViewBag.Error = "Email đã được sử dụng!";
                return View(user);
            }

            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.Now;
            user.RoleId = 2; // Customer

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
