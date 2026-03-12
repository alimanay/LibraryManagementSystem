using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class RentalEditController : Controller
    {
        private readonly LibraryContext _context;
        public RentalEditController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Rentals
       .Include(x => x.Book)
       .Include(x => x.User)
       .ToList();

            return View("/Views/Rental/RentalEdit.cshtml",result);
        }


    }
}
