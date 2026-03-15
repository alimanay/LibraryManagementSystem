using DataAccess.Services.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public  async Task<IActionResult> Index(int? pageNo)
        {
            int page = pageNo ?? 1;
            ViewBag.ToplamKitap = await _dashboardService.GetToplamKitap();
            ViewBag.ToplamKullanici = await _dashboardService.GetToplamKullanici();
            ViewBag.ToplamKiralama = await _dashboardService.GetToplamKiralama();
            ViewBag.GecikmisSayi = await _dashboardService.GetGecikmisSayi();
            ViewBag.GecikmisList = await _dashboardService.GetGecikmisList();
            var sonKiralamalar = await _dashboardService.GetSonKiralamalar();
            ViewBag.SonKiralamalar = sonKiralamalar.ToPagedList(page, 5);

            var aylikData = await _dashboardService.GetAylikKiralama();
            ViewBag.GrafikEtiketler = aylikData.Select(x => x.Etiket).ToList();
            ViewBag.GrafikVeriler = aylikData.Select(x => x.Sayi).ToList();
            return View("~/Views/Dashboard/Dashboard.cshtml");
        }
    }
}
