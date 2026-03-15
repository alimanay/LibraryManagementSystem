using DataAccess.DataAccsess.Abstract;
using DataAccess.Services.Abstract;
using Entites.Dtos.DashboardDtos;
using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Concrete
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _service;
        public DashboardService(IDashboardRepository service)
        {
            _service = service;
        }
        public async Task<List<Rental>> GetGecikmisList() =>
            await _service.GetGecikmisList();

        public async Task<List<AylikKiralamaDto>> GetAylikKiralama() =>
            await _service.GetAylikKiralama();

        public async Task<int> GetGecikmisSayi()
        {
         return  await  _service.GetGecikmisSayi();
        }

        public async Task<List<Rental>> GetSonKiralamalar()
        {
         return   await _service.GetSonKiralamalar();
        }

        public  async Task<int> GetToplamKiralama()
        {
            return  await _service.GetToplamKiralama();
        }

        public async Task<int> GetToplamKitap()
        { 
            return await _service.GetToplamKitap();   
        }

        public  async  Task<int> GetToplamKullanici()
        {
            return await _service.GetToplamKullanici();
        }
    }
}
