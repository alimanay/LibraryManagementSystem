using Data.Interfaces;
using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.Repositories
{
    public class BookRepository : EfRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryContext context) : base(context)
        {
        }

        public IQueryable<Book> GetActiveBooks()
        {
            return _dbSet.Where(b => b.IsActive).AsQueryable();
        }

        public async Task<Book?> GetByIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn)) return null;
            return await _dbSet.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }
    }
}
