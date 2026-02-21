using AutoMapper;
using Entites.Dtos.BookDtos;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class EditBookController : Controller
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        public EditBookController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var bookDto = _mapper.Map<BookUpdateDto>(book);

            // View dosyanız Views/Book/GetBookById.cshtml ise tam yolu verin
            return View("~/Views/Book/GetBookById.cshtml", bookDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBook(BookUpdateDto bookDto)
        {

            if (!ModelState.IsValid)
            {
                // Model geçersizse aynı view'a dön
                return View("~/Views/Book/GetBookById.cshtml", bookDto);
            }
            var existingBook = await _context.Books.FindAsync(bookDto.Id);

            if (existingBook == null)
            {
                return NotFound();
            }
            
            existingBook.Title =  bookDto.Title;
            existingBook.Description = bookDto.Description;
            existingBook.Author = bookDto.Author;
            existingBook.IsActive = bookDto.IsActive;

            _context.Books.Update(existingBook);
            await _context.SaveChangesAsync();

            // Güncelleme sonrası Book listesinin Index action'ına yönlendir
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book == null) return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();  
            return RedirectToAction("Index", "Book");

        }
        }
}
