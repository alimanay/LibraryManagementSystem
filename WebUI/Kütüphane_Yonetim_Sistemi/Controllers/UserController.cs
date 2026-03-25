using AutoMapper;
using DataAccess.Services.Abstract;
using Entites.Dtos.BookDtos;
using Entites.Dtos.UserDtos;
using Entites.Models;
using Kütüphane_Yonetim_Sistemi.Context;
using Kütüphane_Yonetim_Sistemi.Helpers;
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
        private readonly IRoleService _roleService;
        public UserController(IMapper mapper, IUserService userService, IRoleService roleService)
        {
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageNo)
        {
            int page = pageNo ?? 1;
            var users = await _userService.GetAllUsers();
            var roles = await _roleService.GetAllRoles();
            var pagedUsers = users.ToPagedList(page, 10);
            ViewBag.Roles = roles;
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
            if(!string.IsNullOrEmpty(createUserDto.TCNumber))
                createUserDto.TCNumber = EncryptionHelper.Encrypt(createUserDto.TCNumber);

            var createUser = _mapper.Map<User>(createUserDto);
            await _userService.Add(createUser);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            if (!string.IsNullOrEmpty(user.TCNumber))
                user.TCNumber = EncryptionHelper.Decrypt(user.TCNumber);
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

            if(!string.IsNullOrEmpty(userDto.TCNumber))
                userDto.TCNumber = EncryptionHelper.Encrypt(userDto.TCNumber);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(int userId, int roleId)
        { 
            await _roleService.UpdateUserRole(userId, roleId);
            TempData["Success"] = "Rol güncellendi.";
            return RedirectToAction(nameof(Index));
        }  
    
    }
}
