using Data.Interfaces;
using Entites.Models;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IRepository<User> Users { get; }
        IRepository<Rental> Rentals { get; }
        Task SaveChangesAsync();
    }
}
