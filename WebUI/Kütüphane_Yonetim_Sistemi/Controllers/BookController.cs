using AutoMapper;
using Entites.Dtos.BookDtos;
using Entites.Models;
using Infrastructure.ExternalServices.GoogleBooks;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;

public class BookController : Controller
{
    private readonly IGoogleBooksService _googleBooksService;
    private readonly LibraryContext _context;
    public  IMapper _mapper;

    public BookController( IGoogleBooksService googleBooksService,LibraryContext context,IMapper mapper)
    {
        _googleBooksService = googleBooksService;
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.ToListAsync();
        var value = _mapper.Map<List<Book>>(books);
        return View(value);
    }
    [HttpPost]
    public async Task<IActionResult> CreateBook(string query)
    {  
        var books = await _googleBooksService.SearchBooksAsync(query);

        foreach (var book in books)
        { Book existingBook = null;
            
            if(!string.IsNullOrEmpty(book.Type) && book.Type.ToLower() != "other")
            {
                existingBook = await _context.Books.FirstOrDefaultAsync(x => x.ISBN == book.ISBN);
            }

            if(existingBook == null)
            {
                existingBook = await _context.Books.FirstOrDefaultAsync(x => x.Title.ToLower() == book.Title.ToLower() && x.Author.ToLower() == book.Author.ToLower());
            }
            if (existingBook != null)
            {
                existingBook.Title  = book.Title;   
                existingBook.Author = book.Author;
                existingBook.Description = book.Description;  
                existingBook.Type = book.Type;  
                if (string.IsNullOrEmpty(existingBook.ISBN) )
                    existingBook.ISBN = book.ISBN;
                _context.Books.Update(existingBook);
            }
            else
            {
             
                _context.Books.Add(book);
            }


        }
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }



    [HttpPut]

    public async Task<IActionResult> UpdateBook(Book book)
    {
        return View();
    }
   
}
