using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Infrastructure.ExternalServices.Mail;
using Entites.Models;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibraryContext _libraryContext;
        private readonly IMailService _mailService;
        public AccountController(LibraryContext libraryContext, IMailService mailService)
        {

            _libraryContext = libraryContext;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _libraryContext.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role).FirstOrDefault(a => a.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "E-posta veya şifre hatalı";
                return View();
            }

            var code = new Random().Next(100000, 999999).ToString();
            user.TwoFactorCode = code;
            user.TwoFactorExpiry = DateTime.Now.AddMinutes(2);
            _libraryContext.SaveChanges();

            await _mailService.SendEmailAsync(user.Email, "Giriş Doğrulama Kodu", $"<h3>Doğrulama kodunuz: <b>{code}</b></h3><p>5 dakika geçerlidir.</p>");
            // user Id lazım
            HttpContext.Session.SetString("PendingUserId", user.UserId.ToString());
            return RedirectToAction("VerifyCode");
        }
        [HttpPost]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var userId = HttpContext.Session.GetString("PendingUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var user = _libraryContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.UserId == int.Parse(userId));

            if (user == null || user.TwoFactorCode != code || user.TwoFactorExpiry < DateTime.Now)
            {
                ViewBag.Error = "Kod hatalı veya süresi dolmuş.";
                return View();
            }

            // Kodu temizle
            user.TwoFactorCode = null;
            user.TwoFactorExpiry = null;
            _libraryContext.SaveChanges();
            HttpContext.Session.Remove("PendingUserId");

            bool isAdmin = user.UserRoles.Any(ur => ur.Role.Name == "Admin");

            if (isAdmin)
            {
                HttpContext.Session.SetString("Admin", user.Name);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                HttpContext.Session.SetString("User", user.Name);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());

                if (!user.IsPasswordChanged)
                    return RedirectToAction("ChangePassword", "UserDashboard");

                return RedirectToAction("GetUserDashboard", "UserDashboard");
            }
        }



        [HttpGet]
        public IActionResult VerifyCode() => View();



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = _libraryContext.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Bu email kayıtlı değildir";
                return View();
            }
            //Token üretme
            var token = Guid.NewGuid().ToString("N");
            user.PasswordResetToken = token;
            user.PasswordResetTime = DateTime.Now.AddHours(1);
            await _libraryContext.SaveChangesAsync();

            //Mail Gönderme 
            var resetLink = Url.Action("ResetPassword", "Account",
                new { token }, Request.Scheme);

            await _mailService.SendEmailAsync(
                email,
                "Şifre Sıfırlama",
                $"<p>Şifrenizi sıfırlamak için <a href='{resetLink}'>tıklayın</a></p> <p>Link 1 saat geçerlidir.</p>"
                );
            ViewBag.Success = "Mail Gönderildi";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            var user = _libraryContext.Users.FirstOrDefault(z => z.PasswordResetToken == token && z.PasswordResetTime > DateTime.Now);
            if (user == null)
            {
                ViewBag.Error = "Link geçersiz veya süresi dolmuş";
                return View("Login");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string newPassword,string confirmPassword)
        {
            
            var user = _libraryContext.Users.FirstOrDefault(a => a.PasswordResetToken == token && a.PasswordResetTime > DateTime.Now);
            if (user == null)
            {
                ViewBag.Error = "Link geçersiz veya süresi dolmuş.";
                return View();
            }
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor";
                ViewBag.Token = token;
                return View();
            }
            else
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.PasswordResetTime = null;
                user.PasswordResetToken = null;
                await _libraryContext.SaveChangesAsync();
                TempData["Success"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Login");
            }
        }
    }
}
