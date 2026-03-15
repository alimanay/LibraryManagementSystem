using Entites.Dtos.DashboardDtos;
using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Abstract
{
    public interface IDashboardService
    {
        Task<int> GetToplamKitap();
        Task<int> GetToplamKullanici();
        Task<int> GetToplamKiralama();
        Task<int> GetGecikmisSayi();
        Task<List<Rental>> GetSonKiralamalar();
        Task<List<Rental>> GetGecikmisList();
        Task<List<AylikKiralamaDto>> GetAylikKiralama();
    }
}
