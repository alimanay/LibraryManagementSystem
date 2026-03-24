using DataAccess.DataAccsess.Abstract;
using DataAccess.Services.Concrete;
using Entites.Dtos.DashboardDtos;
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
    public class DashboardServiceTest
    {
        private readonly Mock<IDashboardRepository> _mockDashboardRepository;

        private readonly DashboardService _dashboardService;

        public DashboardServiceTest()
        {
            _mockDashboardRepository = new Mock<IDashboardRepository>();
            _dashboardService = new DashboardService(_mockDashboardRepository.Object);
        }
        [Fact]
        public async Task GetToplamKitap_ShouldReturnCorrectCount()
        {
            // Arrange
            int expectedCount = 150;
            _mockDashboardRepository.Setup(x => x.GetToplamKitap()).ReturnsAsync(expectedCount);

            // Act
            var result = await _dashboardService.GetToplamKitap();

            // Assert
            Assert.Equal(expectedCount, result);
            _mockDashboardRepository.Verify(x => x.GetToplamKitap(), Times.Once);
        }

        [Fact]
        public async Task GetGecikmisSayi_ShouldReturnCorrectCount()
        {
            // Arrange
            int expectedGecikmis = 5;
            _mockDashboardRepository.Setup(x => x.GetGecikmisSayi()).ReturnsAsync(expectedGecikmis);

            // Act
            var result = await _dashboardService.GetGecikmisSayi();

            // Assert
            Assert.Equal(expectedGecikmis, result);
        }
       

        [Fact]
        public async Task GetSonKiralamalar_ShouldReturnLatestRentals()
        {
            // Arrange
            var fakeRentals = new List<Rental>
    {
        new Rental { Id = 1, BookId = 101 },
        new Rental { Id = 2, BookId = 102 }
    };
            _mockDashboardRepository.Setup(x => x.GetSonKiralamalar()).ReturnsAsync(fakeRentals);

            // Act
            var result = await _dashboardService.GetSonKiralamalar();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
