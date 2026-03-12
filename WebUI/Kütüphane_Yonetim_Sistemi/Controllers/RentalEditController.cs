using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class RentalEditController : Controller
    {
        private readonly LibraryContext _context;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IRentalService _rentalService;

        public RentalEditController(LibraryContext context, IBookService bookService, IUserService userService, IRentalService rentalService)
        {
            _context = context;
            _bookService = bookService;
            _userService = userService;
            _rentalService = rentalService;
        }

        // ── Liste sayfası
        [HttpGet]
        public IActionResult Index(int? pageNo)
        {
            int page = pageNo ?? 1;
            var result = _context.Rentals
                .Include(x => x.Book)
                .Include(x => x.User)
                .ToList();
            var pagedListResult = result.ToPagedList(page, 5);
            return View("/Views/Rental/RentalDetail.cshtml", pagedListResult);
        }

        // ── GET: Düzenleme formunu aç 
        [HttpGet]
        public async Task<IActionResult> EditLoan(int id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null)
            {
                TempData["Error"] = "Kiralama kaydı bulunamadı.";
                return View("/Views/Rental/RentalDetail.cshtml");
            }

            var user = await _userService.GetById(rental.UserId);
            var book = await _bookService.GetBookById(rental.BookId);

            ViewBag.CurrentUserName = user != null ? $"{user.Name} {user.Surname}" : "";
            ViewBag.CurrentUserTC = user?.TCNumber ?? "";
            ViewBag.CurrentBookTitle = book != null ? $"{book.Title} ({book.ISBN})" : "";

            return View("~/Views/Rental/RentalEditLoan.cshtml", rental);
        }

        // ── POST: Güncelle ve RentalDetail'e yönlendir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLoan(int RentalId, int UserId, int BookId, DateTime? ReturnDate)
        {
            var rental = await _rentalService.GetRentalByIdAsync(RentalId);
            if (rental == null)
            {
                TempData["Error"] = "Kiralama kaydı bulunamadı.";
                return RedirectToAction("Index");   
            }

            var user = await _userService.GetById(UserId);
            if (user == null)
            {
                TempData["Error"] = "Seçilen kullanıcı bulunamadı.";
                return RedirectToAction("EditLoan", new { id = RentalId });
            }

            var book = await _bookService.GetBookById(BookId);
            if (book == null || !book.IsActive)
            {
                TempData["Error"] = "Seçilen kitap bulunamadı veya aktif değil.";
                return RedirectToAction("EditLoan", new { id = RentalId });
            }

            var rentals = await _rentalService.GetAllRental();
            bool kitapBaskasinda = rentals.Any(r =>
                r.BookId == BookId &&
                !r.IsReturned &&
                r.Id != RentalId);

            if (kitapBaskasinda)
            {
                TempData["Error"] = "Bu kitap şu anda başka bir kullanıcıya kiralanmış.";
                return RedirectToAction("EditLoan", new { id = RentalId });
            }

            if (ReturnDate == null)
            {
                TempData["Error"] = "Lütfen teslim tarihini giriniz.";
                return RedirectToAction("EditLoan", new { id = RentalId });
            }

            if (ReturnDate < DateTime.Now.Date)
            {
                TempData["Error"] = "Teslim tarihi bugünden önce olamaz.";
                return RedirectToAction("EditLoan", new { id = RentalId });
            }

            rental.UserId = UserId;
            rental.BookId = BookId;
            rental.ReturnDate = ReturnDate;

            await _rentalService.UpdateAsync(rental);

            TempData["Success"] = "Kiralama başarıyla güncellendi.";
            return RedirectToAction("Index");  
        }

        // ── Kitap arama (düzenleme modunda, kendi kaydı hariç) 
        [HttpGet]
        public async Task<IActionResult> SearchBookForEdit(string query, int userId, int rentalId)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            var rentals = await _rentalService.GetAllRental();

            var rentedBookIds = rentals
                .Where(r => !r.IsReturned && r.Id != rentalId)
                .Select(r => r.BookId)
                .ToList();

            var books = await _bookService.GetAllBooksAsync();
            var filteredBooks = books
                .Where(b => b.IsActive &&
                            ((b.Title != null && b.Title.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                             (b.ISBN != null && b.ISBN.Contains(query, StringComparison.OrdinalIgnoreCase))) &&
                            !rentedBookIds.Contains(b.Id))
                .Select(b => new { bookId = b.Id, title = b.Title, isbn = b.ISBN })
                .ToList();

            return Json(filteredBooks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRental(int id)
        {
            var findUser = await _rentalService.GetRentalByIdAsync(id);
            if (findUser == null) return NotFound();
            await _rentalService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
