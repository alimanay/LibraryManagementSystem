using Entites.Models;

namespace Kütüphane_Yonetim_Sistemi.Services.Abstract
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers(); 
       
        Task<User> GetById(int id);
        Task  Add(User user);
        Task Update(User user);
        Task Delete(int id);
       
    }
}
