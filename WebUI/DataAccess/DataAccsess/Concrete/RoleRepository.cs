using DataAccess.DataAccsess.Abstract;
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
    public class RoleRepository : IRoleRepository
    {
        private readonly LibraryContext _context;
        public RoleRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRoles()
        {
           return await _context.Roles.ToListAsync();
        }

        public async Task UpdatUserRole(int userId, int roleId)
        {
            var existing=  _context.UserRoles.FirstOrDefault(x => x.UserId == userId);
            if(existing != null)
            {
                _context.UserRoles.Remove(existing);
            }
            _context.UserRoles.Add(
                new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });
            await _context.SaveChangesAsync();
        }
    }
}
