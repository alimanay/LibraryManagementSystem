using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly LibraryContext _libraryContext;

        public UserDashboardController(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDashboard()
        {
            var users = await _libraryContext.Users.ToListAsync();
            return View(users);
        }
    }
}
