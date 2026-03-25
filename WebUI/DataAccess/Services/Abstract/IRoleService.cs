using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Abstract
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRoles();
        Task UpdateUserRole(int userId, int roleId);
    }
}
