using Entites.Models;
namespace Infrastructure.ExternalServices.GoogleBooks
{
    public interface IGoogleBooksService
    {
        Task<List<Book>> SearchBooksAsync(string query);
    }
}
