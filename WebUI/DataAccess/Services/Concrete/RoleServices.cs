using DataAccess.DataAccsess.Abstract;
using DataAccess.Services.Abstract;
using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Concrete
{

    public class RoleServices : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleServices(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetAllRoles()
        {
         return  await _roleRepository.GetAllRoles();
        }

        public async Task UpdateUserRole(int userId, int roleId)
        {
           await _roleRepository.UpdatUserRole(userId, roleId);
        }
    }
}
