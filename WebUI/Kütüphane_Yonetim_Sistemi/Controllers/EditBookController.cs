using AutoMapper;
using Entites.Dtos.BookDtos;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class EditBookController : Controller
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        public EditBookController(LibraryContext context, IMapper mapper,IBookService bookService)
        {
            _context = context;
            _mapper = mapper;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book =await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            var bookDto = _mapper.Map<BookUpdateDto>(book);
            return View("~/Views/Book/GetBookById.cshtml", bookDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBook(BookUpdateDto bookDto)
        {

            if (!ModelState.IsValid)
            {
                return View("~/Views/Book/GetBookById.cshtml", bookDto);
            }
            var existingBook =  await _bookService.GetBookById(bookDto.Id);

            if (existingBook == null)
            {
                return NotFound();
            }
            
            existingBook.Title = bookDto.Title;
            existingBook.Description = bookDto.Description;
            existingBook.Author = bookDto.Author;
            existingBook.IsActive = bookDto.IsActive;

            await _bookService.Update(existingBook);
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.Delete(id);
            return RedirectToAction("Index", "Book");
        }
        }
}
