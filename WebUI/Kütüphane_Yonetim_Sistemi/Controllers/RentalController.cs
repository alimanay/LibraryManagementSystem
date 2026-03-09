using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class RentalController : Controller
    {
        private readonly LibraryContext _context;
        public RentalController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Rental");
        }

        // (sadece SearchUser metodunu değiştiriyoruz)
        [HttpGet]
        public async Task<IActionResult> SearchUser(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            var users = await _context.Users
                .Where(x => x.Name.Contains(query) || (x.TCNumber != null && x.TCNumber.Contains(query)))
                .Select(x => new
                {
                    userId = x.UserId,
                    fullName = x.Name + " " + x.Surname,
                    tcNumber = x.TCNumber
                })
                .ToListAsync();

            return Json(users);
        }

        // Yeni: kitap arama — seçilmiş kullanıcıya göre (userId isteğe bağlı)
        [HttpGet]
        public async Task<IActionResult> SearchBook(string query, int? userId)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            // Önce şu anda iade edilmemiş (mevcut) kiralamalardaki kitap id'lerini al
            var rentedBookIds = await _context.Rentals
                .Where(r => !r.IsReturned)
                .Select(r => r.BookId)
                .ToListAsync();

            // Aktif ve query ile eşleşen kitapları getir; eğer isterseniz userId'ye özel filtre ekleyebilirsiniz
            var books = await _context.Books
                .Where(b => b.IsActive
                            && (b.Title != null && b.Title.Contains(query) || (b.ISBN != null && b.ISBN.Contains(query)))
                            && !rentedBookIds.Contains(b.Id))
                .Select(b => new
                {
                    bookId = b.Id,
                    title = b.Title,
                    isbn = b.ISBN
                })
                .ToListAsync();

            return Json(books);
        }

        // Yeni: seçili kullanıcı için müsait kitapları döner (query yoksa tüm müsaitler)
        [HttpGet]
        public async Task<IActionResult> AvailableBooksForUser(int userId)
        {
            // Basit kontrol: kullanıcı var mı
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists) return Json(new { error = "UserNotFound" });

            var rentedBookIds = await _context.Rentals
                .Where(r => !r.IsReturned)
                .Select(r => r.BookId)
                .ToListAsync();

            var books = await _context.Books
                .Where(b => b.IsActive && !rentedBookIds.Contains(b.Id))
                .Select(b => new
                {
                    bookId = b.Id,
                    title = b.Title,
                    isbn = b.ISBN
                })
                .ToListAsync();

            return Json(books);
        }

        // Yeni: kiralama oluşturma (form submit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoan(int UserId, int BookId, DateTime? ReturnDate)
        {
            // Basit validasyon
            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var book = await _context.Books.FindAsync(BookId);
            if (book == null || !book.IsActive)
            {
                TempData["Error"] = "Kitap bulunamadı veya kullanımda değil.";
                return RedirectToAction(nameof(Index));
            }

            // Kitap zaten iade edilmemiş şekilde kiradaysa engelle
            var isCurrentlyRented = await _context.Rentals.AnyAsync(r => r.BookId == BookId && !r.IsReturned);
            if (isCurrentlyRented)
            {
                TempData["Error"] = "Bu kitap şu anda başkasına kiralanmış.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ReturnDate == null) TempData["Error"] = "Lütfen teslim tarihini giriniz.";
                if (ReturnDate < DateTime.Now.Date) TempData["Error"] = "Teslim tarihi, bugünün tarihinden önce olamaz. Lütfen geçerli bir tarih seçiniz.";
                else
                {
                    // Kiralama kaydı oluştur
                    var rental = new Rental
                    {
                        UserId = UserId,
                        BookId = BookId,
                        RentDate = DateTime.Now,
                        ReturnDate = ReturnDate,
                        IsReturned = false
                    };

                    await _context.Rentals.AddAsync(rental);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Kiralama başarıyla oluşturuldu.";

                }
            }
            catch { 
            
            } return RedirectToAction(nameof(Index));
           
        }
    }
}
