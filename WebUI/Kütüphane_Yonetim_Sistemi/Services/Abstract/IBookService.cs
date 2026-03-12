using Entites.Models;

namespace Kütüphane_Yonetim_Sistemi.Services.Abstract
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
       Task<Book?> GetBookById(int id);

        Task Add(Book book);
        Task Update(Book book);
        Task Delete(int id);

    }
}
