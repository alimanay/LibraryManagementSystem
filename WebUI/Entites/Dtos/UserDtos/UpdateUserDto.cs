using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
