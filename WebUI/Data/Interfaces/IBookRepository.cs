using Entites.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IQueryable<Book> GetActiveBooks();
        Task<Book?> GetByIsbnAsync(string isbn);
    }
}
