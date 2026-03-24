using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class RentalController : Controller
    {
       
        private readonly IRentalService _rentalService;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        public RentalController(IRentalService rentalService, IUserService userService, IBookService bookService)
        {
            _rentalService = rentalService;
            _userService = userService;
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Rental");
        }
        [HttpGet]
        public async Task<IActionResult> SearchUser(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            var allUsers = await _userService.GetAllUsers();

            // 2️⃣ LINQ işlemi artık List<User> üzerinde çalışır
            var users = allUsers
                .Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            x.Surname.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            (x.TCNumber != null && x.TCNumber.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new
                {
                    userId = x.UserId,
                    fullName = x.Name + " " + x.Surname,
                    tcNumber = Helpers.EncryptionHelper.Decrypt(x.TCNumber)
                })
                .ToList();
            return Json(users);
        }

        // Yeni: kitap arama — seçilmiş kullanıcıya göre 
        [HttpGet]
        public async Task<IActionResult> SearchBook(string query, int? userId)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            var rentals = await _rentalService.GetAllRental();
            var rentedBookIds = rentals.Where(r => !r.IsReturned).Select(r => r.BookId).ToList();

            var books = await _bookService.GetAllBooksAsync();
            var filteredBooks = books
                .Where(b => b.IsActive &&
                           ((b.Title != null && b.Title.Contains(query)) || (b.ISBN != null && b.ISBN.Contains(query))) &&
                           !rentedBookIds.Contains(b.Id))
                .Select(b => new
                {
                    bookId = b.Id,
                    title = b.Title,
                    isbn = b.ISBN
                })
                .ToList();

            return Json(filteredBooks);
        }

        // Yeni: seçili kullanıcı için müsait kitapları döner (query yoksa tüm müsaitler)
        [HttpGet]
        public async Task<IActionResult> AvailableBooksForUser(int userId)
        {
            var allUsers = await _userService.GetAllUsers();
            if (!allUsers.Any(u => u.UserId == userId))
                return Json(new { error = "UserNotFound" });

            var rentals = await _rentalService.GetAllRental();
            var rentedBookIds = rentals.Where(r => !r.IsReturned).Select(r => r.BookId).ToList();

            var books = await _bookService.GetAllBooksAsync();
            var availableBooks = books
                .Where(b => b.IsActive && !rentedBookIds.Contains(b.Id))
                .Select(b => new
                {
                    bookId = b.Id,
                    title = b.Title,
                    isbn = b.ISBN
                })
                .ToList();

            return Json(availableBooks);
        }
        // Yeni: kiralama oluşturma (form submit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoan(int UserId, int BookId, DateTime? ReturnDate)
        {
            var user = await _userService.GetById(UserId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var book = await _bookService.GetBookByIdAsync(BookId);
            if (book == null || !book.IsActive)
            {
                TempData["Error"] = "Kitap bulunamadı veya kullanımda değil.";
                return RedirectToAction(nameof(Index));
            }

            var rentals = await _rentalService.GetAllRental();
            if (rentals.Any(r => r.BookId == BookId && !r.IsReturned))
            {
                TempData["Error"] = "Bu kitap şu anda başkasına kiralanmış.";
                return RedirectToAction(nameof(Index));
            }

            if (ReturnDate == null)
                TempData["Error"] = "Lütfen teslim tarihini giriniz.";
            else if (ReturnDate < DateTime.Now.Date)
                TempData["Error"] = "Teslim tarihi bugünün tarihinden önce olamaz.";
            else
            {

                var rental = new Rental
                {
                    UserId = UserId,
                    BookId = BookId,
                    RentDate = DateTime.Now,
                    ReturnDate = ReturnDate,
                    IsReturned = false
                };
                await _rentalService.AddAsync(rental);
                TempData["Success"] = "Kiralama başarıyla oluşturuldu.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
