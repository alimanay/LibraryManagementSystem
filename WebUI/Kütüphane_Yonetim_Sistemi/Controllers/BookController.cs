using Infrastructure.ExternalServices.GoogleBooks;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BookController : Controller
{
    private readonly IGoogleBooksService _googleBooksService;
    private readonly LibraryContext _context;

    public BookController(
        IGoogleBooksService googleBooksService,
        LibraryContext context)
    {
        _googleBooksService = googleBooksService;
        _context = context;
    }

    // 🔹 Google'dan çekip DB'ye kaydeder
    [HttpPost]
    public async Task<IActionResult> Import(string query)
    {
        var books = await _googleBooksService.SearchBooksAsync(query);

        foreach (var book in books)
        {
            _context.Books.Add(book);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // 🔹 DB'den okur ve View'e gönderir
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.ToListAsync();
        return View(books);
    }
}
