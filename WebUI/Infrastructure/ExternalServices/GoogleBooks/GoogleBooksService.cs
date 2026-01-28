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
        public GoogleBooksService(HttpClient httpClient, IConfiguration configuration   )
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<Book>> SearchBooksAsync(string query)
        {
            var apiKey=   _configuration["GoogleBooks:ApiKey"];
           
            var response = await _httpClient.GetFromJsonAsync<GoogleBooksResponseDto>($"https://www.googleapis.com/books/v1/volumes?q={query}&key={apiKey}");
           
             if(response?.Items == null)
            {
                return new List<Book>();
            }
            return response.Items.Select(GoogleBooksMapper.ToBookEntity).ToList(); 
        }
    }
}
