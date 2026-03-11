using Data.Interfaces;
using Data.Repositories;
using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _context;
        private BookRepository _bookRepository;
        private EfRepository<User> _userRepository;
        private EfRepository<Rental> _rentalRepository;

        public UnitOfWork(LibraryContext context)
        {
            _context = context;
        }

        public IBookRepository Books => _bookRepository ??= new BookRepository(_context);
        public IRepository<User> Users => _userRepository ??= new EfRepository<User>(_context);
        public IRepository<Rental> Rentals => _rentalRepository ??= new EfRepository<Rental>(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
