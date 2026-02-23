using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class UserController : Controller
    {
        private readonly LibraryContext _context;
        public UserController(LibraryContext context)
        {
            _context = context;
        }

        public async  Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View("Users",users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var findUser = await _context.Users.FindAsync(id);
            if (findUser == null) return NotFound();
            _context.Users.Remove(findUser);
           await _context.SaveChangesAsync();
            return RedirectToAction("GetUsers","User");
        }
    }
}
