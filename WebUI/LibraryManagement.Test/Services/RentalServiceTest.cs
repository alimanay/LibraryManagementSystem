using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Concrete;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Test.Services
{
    public class RentalServiceTest
    {
        private readonly Mock<IRentalRepository> _mockRentalRepository;
        private readonly Mock<ILogger<RentalService>> _mockLogger;
        private readonly RentalService _rentalService;

        public RentalServiceTest()
        {
            _mockRentalRepository = new Mock<IRentalRepository>();
            _mockLogger = new Mock<ILogger<RentalService>>();
            _rentalService = new RentalService(_mockRentalRepository.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task AddAsync_ShouldInvokeRepositoryAdd()
        {
            // Arrange
            var rental = new Rental { UserId = 1, BookId = 10, ReturnDate = DateTime.Now.AddDays(7) };
            _mockRentalRepository.Setup(x => x.AddAsync(rental)).Returns(Task.CompletedTask);

            // Act
            await _rentalService.AddAsync(rental);

            // Assert
            _mockRentalRepository.Verify(x => x.AddAsync(It.Is<Rental>(r => r.UserId == 1 && r.BookId == 10)), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_WhenRentalExists_ShouldCallDelete()
        {
            // Arrange
            int rentalId = 1;
            var rental = new Rental { Id = rentalId, UserId = 1, BookId = 10 };
            _mockRentalRepository.Setup(x => x.GetRentalByIdAsync(rentalId)).ReturnsAsync(rental);

            // Act
            await _rentalService.DeleteAsync(rentalId);

            // Assert
            _mockRentalRepository.Verify(x => x.DeleteAsync(rental), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenRentalNotFound_ShouldNotInvokeDelete()
        {
            // Arrange
            int nonExistentId = 99;
            _mockRentalRepository.Setup(x => x.GetRentalByIdAsync(nonExistentId)).ReturnsAsync((Rental)null);

            // Act
            await _rentalService.DeleteAsync(nonExistentId);

            // Assert
            _mockRentalRepository.Verify(x => x.DeleteAsync(It.IsAny<Rental>()), Times.Never);
        }
        [Fact]
        public async Task UpdateAsync_WhenBookReturned_ShouldInvokeUpdate()
        {
            // Arrange
            var rental = new Rental { Id = 1, IsReturned = true };

            // Act
            await _rentalService.UpdateAsync(rental);

            // Assert
            _mockRentalRepository.Verify(x => x.UpdateAsync(It.Is<Rental>(r => r.IsReturned == true)), Times.Once);
        }
        [Fact]
        public async Task GetReturnedRentals_ShouldReturnOnlyReturnedBooks()
        {
            // Arrange
            var returnedList = new List<Rental> { new Rental { Id = 1, IsReturned = true } };
            _mockRentalRepository.Setup(x => x.GetReturnedRentals()).ReturnsAsync(returnedList);

            // Act
            var result = await _rentalService.GetReturnedRentals();

            // Assert
            Assert.Single(result); // Listede sadece 1 tane olduğunu doğrula
            Assert.True(result[0].IsReturned);
        }
    }
}
