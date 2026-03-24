using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Kütüphane_Yonetim_Sistemi.Services.Concrete
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            _logger.LogInformation("Tüm kitaplar getiriliyor.");
            var books =  await _bookRepository.GetAllBooksAsync();
            _logger.LogInformation("Tüm kitaplar getirildi.");
            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            _logger.LogInformation("Kitap getiriliyor. Id: {Id}" + id);
            var book=  await _bookRepository.GetByIdAsync(id);
            if(book == null)
                _logger.LogWarning("Kitap bulunamadı. Id: {Id}", id);
            return book;
        }

        public async Task Add(Book book)
        {
            _logger.LogInformation("Kitap ekleniyor: {Title}", book.Title);
            await _bookRepository.AddAsync(book);
            _logger.LogInformation("Kitap eklendi: {Title}", book.Title);
        }

        public async Task Update(Book book)
        {
            _logger.LogInformation("Kitap güncelleniyor. Id: {Id} - {Title}", book.Id, book.Title);
            await _bookRepository.UpdateAsync(book);
            _logger.LogInformation("Kitap güncellendi. Id: {Id}", book.Id);
        }

        public async Task Delete(int id)
        {
            _logger.LogWarning("Kitap siliniyor. Id: {Id}", id);
            await _bookRepository.DeleteAsync(id);
            _logger.LogWarning("Kitap silindi. Id: {Id}", id);
        }
    }
}
