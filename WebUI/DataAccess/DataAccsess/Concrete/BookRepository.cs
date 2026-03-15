using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kütüphane_Yonetim_Sistemi.DataAccsess.Concrete
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;
        public BookRepository(LibraryContext context) => _context = context;

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return;
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        
    }
}