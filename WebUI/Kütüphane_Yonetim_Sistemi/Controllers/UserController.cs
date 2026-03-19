using AutoMapper;
using Entites.Dtos.BookDtos;
using Entites.Dtos.UserDtos;
using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace Kütüphane_Yonetim_Sistemi.Controllers
{
    public class UserController : Controller
    {
      
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UserController( IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageNo)
        {
            int page = pageNo ?? 1;

            // Async metodunu await et
            var users = await _userService.GetAllUsers();

            // Şimdi ToPagedList çalışır
            var pagedUsers = users.ToPagedList(page, 5);

            return View("Users", pagedUsers);
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View("CreateUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {

            if (!ModelState.IsValid)
            {
                return View("CreateUser", createUserDto);
            }

            var createUser = _mapper.Map<User>(createUserDto);
            await _userService.Add(createUser);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            var userDto = _mapper.Map<UpdateUserDto>(user);
            return View("EditUser", userDto); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
             
                return View("EditUser", userDto);
            }
            var existingUser =await _userService.GetById(userDto.UserId);
            if (existingUser == null) return NotFound();

              _mapper.Map(userDto, existingUser);

           await _userService.Update(existingUser);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var findUser = await _userService.GetById(id);
            if (findUser == null) return NotFound();
              await _userService.Delete(id);    
            return RedirectToAction(nameof(Index));
        }
    }
}
