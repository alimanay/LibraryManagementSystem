using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibraryContext _libraryContext;
        public AccountController(LibraryContext libraryContext)
        {

            _libraryContext = libraryContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _libraryContext.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "E-posta veya şifre hatalı";
                return View();
            }

            bool isAdmin = user.UserRoles.Any(ur => ur.Role.Name == "Admin");

            if (isAdmin)
            {
                HttpContext.Session.SetString("Admin", user.Name);
                return RedirectToAction("Index", "Book");
            }
            else
            {
                HttpContext.Session.SetString("User", user.Name);
                return RedirectToAction("GetUserDashboard", "UserDashboard");
            }
        }

    }
}


