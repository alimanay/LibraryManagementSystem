using Castle.Core.Logging;
using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Concrete;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Concrete;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagement.Test.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<ILogger<BookService>> _mockLogger;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockLogger = new Mock<ILogger<BookService>>();

            _bookService = new BookService(_mockBookRepository.Object, _mockLogger.Object);
        }

        // Id
        [Fact]
        public async Task GetBookById_WhenBookExists_ReturnBook()
        {
            //Arrange => hazırlık
            int bookId = 27;
            var expectedBook = new Book { Id = bookId, Title = "Suç ve Ceza" };

            // Repository kısmı 
            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(expectedBook);

            // Act =>  eylem
            var result = await _bookService.GetBookByIdAsync(bookId);
            // Assert=> doğrulama
            Assert.NotNull(result);
            Assert.Equal(expectedBook.Id, result.Id);
            Assert.Equal(expectedBook.Title, result.Title);
            //Doğrulama yapma ıd var mı ?
            _mockBookRepository.Verify(z => z.GetByIdAsync(bookId), Times.Once);

        }

        // Id Hatalı durum
        [Fact]
        public async Task GetBookById_WhenBookDoesNotExist_ReturnsNull()
        {    // Arrange
            int bookId = 99;
            // Act
            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book)null);
            var result = await _bookService.GetBookByIdAsync(bookId);

            //Assert Doğrulama
            Assert.Null(result);
        }
        //ADD
        [Fact]
        public async Task Add_ValidBook_ShouldCallRepositoryAddAsync()
        {
            var newBook = new Book
            {
                Id = 1,
                Title = "TestBook",
                Author = "TestAuthor"

            };
            _mockBookRepository.Setup(a => a.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
            await _bookService.Add(newBook);
            _mockBookRepository.Verify(x => x.AddAsync(It.IsAny<Book>()), Times.Once);
        }
        // ADD Hatalı durum
        [Fact]
        public async Task Add_WhenRepositoryFails_ShouldThrowException()
        {
            // Arrange
            var book = new Book { Title = "Test" };
            _mockBookRepository
                .Setup(x => x.AddAsync(book))
                .ThrowsAsync(new Exception("Veritabanı bağlantı hatası"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.Add(book));
        }
        //Delete
        [Fact]
        public async Task Delete_WhenCalled_ShouldInvokeRepositoryDeleteAsync()
        {
            int bookId = 2;
            _mockBookRepository.Setup(a => a.DeleteAsync(bookId)).Returns(Task.CompletedTask);
            await _bookService.Delete(bookId);
            _mockBookRepository.Verify(s => s.DeleteAsync(bookId), Times.Once);
        }

        //GetAll
        [Fact]
        public async Task GetAllBooksAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            var fakeBooks = new List<Book>
            {
               new Book {  Id  = 1,
                   Title = "Suç ve Ceza" },
                    new Book {  Id  = 2,
                   Title = "Test2" },
            };

            _mockBookRepository.Setup(a => a.GetAllBooksAsync()).ReturnsAsync(fakeBooks);
            var result = await _bookService.GetAllBooksAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, z => z.Title == "Suç ve Ceza");

            // Log Doğrulama
            _mockLogger.Verify(x => x.Log(
        LogLevel.Information,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("getiriliyor")),
        null,
        It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        //Update
        [Fact]
        public async Task Update_WhenCalled_ShouldInvokeRepositoryUpdateAsync()
        {
            // 1. Arrange
            var bookToUpdate = new Book { Id = 27, Title = "Suç ve Ceza (Yeni Baskı)" };
           
            _mockBookRepository.Setup(x => x.UpdateAsync(bookToUpdate)).Returns(Task.CompletedTask);
            // 2. Act
            await _bookService.Update(bookToUpdate);

            // 3. Assert
            _mockBookRepository.Verify(x => x.UpdateAsync(It.Is<Book>(b => b.Id == 27 && b.Title.Contains("Yeni Baskı"))), Times.Once);
        }
        [Fact]
        public async Task Update_WhenBookIsNull_ShouldThrowException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _bookService.Update(null));
        }

    }
}
