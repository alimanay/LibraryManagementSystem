using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccsess.Abstract
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRoles();
        Task UpdatUserRole(int userId, int roleId);
    }
}
