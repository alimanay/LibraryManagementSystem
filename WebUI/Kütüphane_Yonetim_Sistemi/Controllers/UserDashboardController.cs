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
    }
}