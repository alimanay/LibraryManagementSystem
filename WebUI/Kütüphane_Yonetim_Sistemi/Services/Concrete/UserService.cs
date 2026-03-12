using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using System.Threading.Tasks;

namespace Kütüphane_Yonetim_Sistemi.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {       
            _userRepository = userRepository;
        }

        public async Task Add(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task Delete(int id)
        {
           var user = await _userRepository.GetByIdAsync(id);
            if (user != null) { 
         await  _userRepository.DeleteAsync(user);
            }
        }

        public Task<List<User>> GetAllUsers()
        {
           return _userRepository.GetAllUsersAsync();
        }

        public Task<User> GetById(int id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public async Task Update(User user)
        {
            await _userRepository.UpdateAsync(user);
        }
    }
}
