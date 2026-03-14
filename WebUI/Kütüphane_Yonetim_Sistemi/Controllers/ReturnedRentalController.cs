using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class ReturnedRentalController : Controller
    {
        private readonly IRentalService _rentalService;
        public ReturnedRentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? pageNo)
        {
            int page = pageNo ?? 1;
            var result = await _rentalService.GetReturnedRentals();
            var pagedList = result.ToPagedList(page, 10);
            return View("/Views/Rental/ReturnedRentals.cshtml", pagedList);
        }
    }
}
