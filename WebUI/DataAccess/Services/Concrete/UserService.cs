using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Kütüphane_Yonetim_Sistemi.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Add(User user)
        {
            _logger.LogInformation(
                "Kullanıcı ekleniyor. Ad: {Name} {Surname} - Email: {Email}",
                user.Name, user.Surname, user.Email);

            await _userRepository.AddAsync(user);

            _logger.LogInformation(
                "Kullanıcı eklendi. UserId: {UserId} - Ad: {Name} {Surname}",
                user.UserId, user.Name, user.Surname);
        }

        public async Task Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Silinecek kullanıcı bulunamadı. UserId: {Id}", id);
                return;
            }

            _logger.LogWarning(
                "Kullanıcı siliniyor. UserId: {Id} - Ad: {Name} {Surname}",
                user.UserId, user.Name, user.Surname);

            await _userRepository.DeleteAsync(user);

            _logger.LogWarning("Kullanıcı silindi. UserId: {Id}", id);
        }

        public Task<List<User>> GetAllUsers()
        {
            _logger.LogInformation("Tüm kullanıcılar getiriliyor.");
            return _userRepository.GetAllUsersAsync();
        }

        public Task<User> GetById(int id)
        {
            _logger.LogInformation("Kullanıcı getiriliyor. UserId: {Id}", id);
            return _userRepository.GetByIdAsync(id);
        }

        public async Task Update(User user)
        {
            _logger.LogInformation(
                "Kullanıcı güncelleniyor. UserId: {Id} - Ad: {Name} {Surname}",
                user.UserId, user.Name, user.Surname);

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Kullanıcı güncellendi. UserId: {Id}", user.UserId);
        }
    }
}