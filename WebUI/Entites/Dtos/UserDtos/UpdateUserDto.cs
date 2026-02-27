using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon boş bırakılamaz")]
        [MinLength(11, ErrorMessage = "Telefon numarası 11 haneli olmalı"), MaxLength(11)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
