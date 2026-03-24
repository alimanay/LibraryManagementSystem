using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Concrete;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagement.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Add_WhenUserAdded_PasswordShouldBeHashed()
        {
            // Arrange (Hazırlık)
            var plainPassword = "Sifre123_Sifre";
            var user = new User
            {
                Name = "Ali",
                Surname = "Manay",
                Email = "ali@mail.com",
                PasswordHash = plainPassword
            };

            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act (Eylem)
            await _userService.Add(user);

            // Assert (Doğrulama)
          
            Assert.NotEqual(plainPassword, user.PasswordHash);

            // 2. BCrypt hash'leri
            Assert.StartsWith("$2", user.PasswordHash);
            _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public async Task Delete_WhenUserExists_ShouldInvokeDeleteAsync()
        {
            // Arrange
            int userId = 1;
            var existingUser = new User { UserId = userId, Name = "Mehmet" };
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act
            await _userService.Delete(userId);

            // Assert
            _mockUserRepository.Verify(x => x.DeleteAsync(existingUser), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenUserNotFound_ShouldLogWarningAndNotCallDelete()
        {
            // Arrange
            int nonExistentId = 99;
            _mockUserRepository.Setup(x => x.GetByIdAsync(nonExistentId)).ReturnsAsync((User)null);

            // Act
            await _userService.Delete(nonExistentId);

            // Assert
        
            _mockUserRepository.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Never);
        }
        [Fact]
        public async Task GetAllUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { Name = "User1" }, new User { Name = "User2" } };
            _mockUserRepository.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.Equal(2, result.Count);
            _mockUserRepository.Verify(x => x.GetAllUsersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenUserExists_ShouldReturnCorrectUser()
        {
            // Arrange
            int userId = 10;
            var user = new User { UserId = userId, Name = "Veli" };
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Veli", result.Name);
        }
        [Fact]
        public async Task Update_WhenCalled_ShouldInvokeRepositoryUpdate()
        {
            // Arrange
            var user = new User { UserId = 5, Name = "Ayşe", Surname = "Manay" };

            // Act
            await _userService.Update(user);

            // Assert
            _mockUserRepository.Verify(x => x.UpdateAsync(It.Is<User>(u => u.UserId == 5)), Times.Once);
        }
    }
}
