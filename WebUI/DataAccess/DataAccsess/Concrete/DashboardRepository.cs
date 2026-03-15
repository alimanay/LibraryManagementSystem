using DataAccess.DataAccsess.Abstract;
using Entites.Dtos.DashboardDtos;
using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccsess.Concrete
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly LibraryContext _context;
        public DashboardRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<AylikKiralamaDto>> GetAylikKiralama()
        {
            var altiAyOnce = DateTime.Now.AddMonths(-5);

            var data = await _context.Rentals
                .Where(r => r.RentDate >= altiAyOnce)
                .GroupBy(r => new { r.RentDate.Year, r.RentDate.Month })
                .Select(g => new
                {
                    Yil = g.Key.Year,
                    Ay = g.Key.Month,
                    Sayi = g.Count()
                })
                .OrderBy(x => x.Yil)
                .ThenBy(x => x.Ay)
                .ToListAsync();

            return data.Select(x => new AylikKiralamaDto
            {
                Etiket = x.Ay + "/" + x.Yil,
                Sayi = x.Sayi
            }).ToList();
        }
        public async Task<List<Rental>> GetGecikmisList()
        {
            return await _context.Rentals
      .Include(r => r.User)
      .Include(r => r.Book)
      .Where(r => !r.IsReturned && r.ReturnDate!.Value.Date <= DateTime.Now.Date)
      .OrderBy(r => r.ReturnDate)
      .Take(5)
      .ToListAsync();
        }

        public  async Task<int> GetGecikmisSayi()
        {
            var overdue = await _context.Rentals.Where(x => !x.IsReturned && x.ReturnDate!.Value.Date <= DateTime.Now.Date).CountAsync();
            return overdue;
        }
        public Task<List<Rental>> GetSonKiralamalar()
        {
            return  _context.Rentals.Where(a => a.IsReturned == false).Include(a=>a.Book).Include(a=>a.User).OrderBy(a=>a.RentDate).ToListAsync();
        }

        public async Task<int> GetToplamKiralama()
        {
            return await _context.Rentals.CountAsync();
        }

        public async  Task<int> GetToplamKitap()
        {
            return await _context.Books.CountAsync(b => b.IsActive);   
        }

        public async Task<int> GetToplamKullanici()
        {
            return await _context.Users.CountAsync(b=>b.IsActive);
        }
    }
}
