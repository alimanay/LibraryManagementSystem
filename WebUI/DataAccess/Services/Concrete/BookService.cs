using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kütüphane_Yonetim_Sistemi.Services.Concrete
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task Add(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task Update(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task Delete(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
