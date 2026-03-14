using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;

namespace Kütüphane_Yonetim_Sistemi.Services.Concrete
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

       
        public async Task AddAsync(Rental rental)
        {
          await _rentalRepository.AddAsync(rental);
        }

        public async Task DeleteAsync(int id)
        {
           var rental =  await _rentalRepository.GetRentalByIdAsync(id);
            if (rental != null) {
             await  _rentalRepository.DeleteAsync(rental);
            }
        }

        public Task<List<Rental>> GetAllRental()
        {
           return   _rentalRepository.GetAllRentalAsync();
        }

        public Task<Rental> GetRentalByIdAsync(int id)
        {

            return _rentalRepository.GetRentalByIdAsync(id);
        }

        public Task<List<Rental>> GetReturnedRentals()
        {
           return _rentalRepository.GetReturnedRentals();
        }

        public async Task UpdateAsync(Rental rental)
        {
           await _rentalRepository.UpdateAsync(rental);
        }
    }
}
