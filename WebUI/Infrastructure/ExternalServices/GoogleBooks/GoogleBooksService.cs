using Entites.Models;
using Infrastructure.ExternalServices.DTOs;
using Infrastructure.ExternalServices.Mapping;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;



namespace Infrastructure.ExternalServices.GoogleBooks
{
    public class GoogleBooksService : IGoogleBooksService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        public GoogleBooksService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<Book>> SearchBooksAsync(string query)
        {
            var apiKey = _configuration["GoogleBooks:ApiKey"];
            var cleanQuery = query.Trim();

            // 1. Sorgu Tipini Belirle (ISBN mi yoksa Başlık mı?)
            bool isIsbn = cleanQuery.Replace("-", "").All(char.IsDigit) && (cleanQuery.Length >= 10);
            string searchParam = isIsbn ? $"isbn:{cleanQuery.Replace("-", "")}" : $"intitle:{cleanQuery}";

            // 2. URL Oluştur (API Key eklemeyi unutma!)
            // maxResults'ı 10 yapıp, dil kısıtlamasını ekledik.
            var url = $"https://www.googleapis.com/books/v1/volumes?q={searchParam}&langRestrict=tr&maxResults=10&key={apiKey}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<GoogleBooksResponseDto>(url);

                if (response?.Items == null) return new List<Book>();

                // 3. Dönüşümü Yap ve Filtrele
                return response.Items
                    .Select(GoogleBooksMapper.ToBookEntity)
                    .Where(b => !string.IsNullOrEmpty(b.Title)) // Boş başlıkları ele
                    .Take(isIsbn ? 1 : 5) // ISBN ise 1, değilse 5 tane getir
                    .ToList();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                // 429 hatası durumunda boş liste dön veya logla
                return new List<Book>();
            }
        }
    }
}