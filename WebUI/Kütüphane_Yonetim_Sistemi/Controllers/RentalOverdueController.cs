using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class RentalOverdueController : Controller
    {
        private readonly IRentalService _rentalService;
        public RentalOverdueController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageNo)
        {
            int page = pageNo ?? 1;
            var result = await _rentalService.GetAllRental();
            var overdue = result.Where(r => !r.IsReturned && r.ReturnDate.HasValue && r.ReturnDate.Value.Date <= DateTime.Now.Date).ToPagedList(page,10);
            return View("/Views/Rental/RentalOverdue.cshtml", overdue);
        }
    }
}
