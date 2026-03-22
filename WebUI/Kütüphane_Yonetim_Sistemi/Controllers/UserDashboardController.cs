using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UserDashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRentalService _rentalService;

        public UserDashboardController(IUserService userService, IRentalService rentalService)
        {
            _userService = userService;
            _rentalService = rentalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDashboard()
        {
            // Session'dan giriş yapan kullanıcının Id'sini al
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdStr);

            var kullanici = await _userService.GetById(userId);
            var tumKiralamalar = await _rentalService.GetAllRental();
            var kiralamalar = tumKiralamalar
                .Where(r => r.UserId == userId)
                .ToList();

            ViewBag.Kullanici = kullanici;
            ViewBag.Kiralamalar = kiralamalar;

            return View("GetUserDashboard");
        }
        [HttpGet]
        public async Task<IActionResult> MyRentals()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdStr);
            var tumKiralamalar = await _rentalService.GetAllRental();

            // Sadece aktif (teslim edilmemiş) kiralamalar
            var aktifKiralamalar = tumKiralamalar
                .Where(r => r.UserId == userId && !r.IsReturned)
                .ToList();

            ViewBag.Kiralamalar = aktifKiralamalar;
            return View("~/Views/UserDashboard/UserRental.cshtml");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HttpContext.Session.GetString("User") == null)
                context.Result = RedirectToAction("Login", "Account");
            base.OnActionExecuting(context);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor";
                return View("ChangePassword");
            }

            var userId = int.Parse(HttpContext.Session.GetString("UserId")!);
            var user = await _userService.GetById(userId);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.IsPasswordChanged = true;

            await _userService.Update(user);
            return RedirectToAction("Login", "Account");
        }
    }
}