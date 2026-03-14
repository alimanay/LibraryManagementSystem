using Entites.Models;

namespace Kütüphane_Yonetim_Sistemi.Services.Abstract
{
    public interface IRentalService
    {
        Task<List<Rental>> GetAllRental();
        Task<Rental> GetRentalByIdAsync(int id);
        Task AddAsync(Rental rental);
        Task UpdateAsync(Rental rental);
        Task DeleteAsync(int id);
    }
}
