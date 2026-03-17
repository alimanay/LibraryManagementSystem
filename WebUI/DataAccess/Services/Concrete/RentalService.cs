using Entites.Models;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Kütüphane_Yonetim_Sistemi.Services.Concrete
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<RentalService> _logger;

        public RentalService(IRentalRepository rentalRepository, ILogger<RentalService> logger)
        {
            _rentalRepository = rentalRepository;
            _logger = logger;
        }


        public async Task AddAsync(Rental rental)
        {
            _logger.LogInformation(
        "Kiralama oluşturuluyor. UserId: {UserId} - BookId: {BookId} - Teslim: {ReturnDate}",
        rental.UserId, rental.BookId, rental.ReturnDate?.ToString("dd.MM.yyyy"));

            await _rentalRepository.AddAsync(rental);

            _logger.LogInformation(
                "Kiralama oluşturuldu. RentalId: {RentalId} - UserId: {UserId} - BookId: {BookId}",
                rental.Id, rental.UserId, rental.BookId);
        }

        public async Task DeleteAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(id);
            if (rental == null)
            {
                _logger.LogWarning("Silinecek kiralama bulunamadı. Id: {Id}", id);
                return;
            }

            _logger.LogWarning(
                "Kiralama siliniyor. RentalId: {Id} - UserId: {UserId} - BookId: {BookId}",
                rental.Id, rental.UserId, rental.BookId);

            await _rentalRepository.DeleteAsync(rental); 

            _logger.LogWarning("Kiralama silindi. RentalId: {Id}", id);
        }

        public Task<List<Rental>> GetAllRental()
        {
            _logger.LogInformation("Tüm kiralamalar getiriliyor.");
            return _rentalRepository.GetAllRentalAsync();
        }

        public Task<Rental> GetRentalByIdAsync(int id)
        {

            _logger.LogInformation("Kiralama getiriliyor. Id: {Id}", id);
            return _rentalRepository.GetRentalByIdAsync(id);
        }

        public Task<List<Rental>> GetReturnedRentals()
        {
            _logger.LogInformation("Teslim edilen kiralamalar getiriliyor.");
            return _rentalRepository.GetReturnedRentals();
        }

        public async Task UpdateAsync(Rental rental)
        {
            if (rental.IsReturned)
                _logger.LogWarning(
                    "Kitap teslim alındı. RentalId: {Id} - UserId: {UserId} - BookId: {BookId}",
                    rental.Id, rental.UserId, rental.BookId);
            else
                _logger.LogInformation(
                    "Kiralama güncelleniyor. RentalId: {Id}", rental.Id);

            await _rentalRepository.UpdateAsync(rental);

            _logger.LogInformation("Kiralama güncellendi. RentalId: {Id}", rental.Id);
        }
    }
}
