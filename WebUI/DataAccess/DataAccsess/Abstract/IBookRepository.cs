using Entites.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }
}
