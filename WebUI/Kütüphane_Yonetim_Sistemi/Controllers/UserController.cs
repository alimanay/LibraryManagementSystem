using AutoMapper;
using Entites.Dtos.BookDtos;
using Entites.Dtos.UserDtos;
using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class UserController : Controller
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        public UserController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userDtoList = _mapper.Map<List<GetListUserDto>>(users);
            return View("Users", userDtoList);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View("CreateUser");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {

            if (!ModelState.IsValid)
            {
                return View(createUserDto);
            }

            var createUser = _mapper.Map<User>(createUserDto);
            await _context.Users.AddAsync(createUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetUsers));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var userDto = _mapper.Map<UpdateUserDto>(user);
            return View("EditUser", userDto); // Views/User/EditUser.cshtml mevcutsa bu çalışır
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                // View adı (Views/User/EditUser.cshtml) zaten konvansiyona uygunsa sadece isim verin
                return View("EditUser", userDto);
            }

            var existingUser = await _context.Users.FindAsync(userDto.UserId);
            if (existingUser == null) return NotFound();

            _mapper.Map(userDto, existingUser);

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            // Action adını kullanarak yönlendiriyoruz
            return RedirectToAction(nameof(GetUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var findUser = await _context.Users.FindAsync(id);
            if (findUser == null) return NotFound();
            _context.Users.Remove(findUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetUsers));
        }
    }
}
