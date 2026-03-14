using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.DataAccsess.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kütüphane_Yonetim_Sistemi.DataAccsess.Concrete
{
    public class RentalRepository : IRentalRepository
    {
        private readonly LibraryContext _context;
        public RentalRepository(LibraryContext context) => _context = context;

        public async Task<List<Rental>> GetAllRentalAsync()
        {
            return await _context.Rentals.Include(r => r.Book).Include(b => b.User).ToListAsync();
        }

        public async Task<Rental?> GetRentalByIdAsync(int id)
        {
            return await _context.Rentals.FindAsync(id);
        }

        public async Task AddAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rental rental)
        {
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
        }

        public Task<List<Rental>> GetReturnedRentals()
        {
          return   _context.Rentals.Where(x => x.IsReturned == true).Include(r => r.Book).Include(r => r.User).ToListAsync();
        }
    }
}