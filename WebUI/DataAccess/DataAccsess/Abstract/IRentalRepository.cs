using Entites.Models;

namespace Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetAllRentalAsync();
        Task<Rental?> GetRentalByIdAsync(int id);
        Task AddAsync(Rental rental);
        Task UpdateAsync(Rental rental);
        Task DeleteAsync(Rental rental);
    }
}
