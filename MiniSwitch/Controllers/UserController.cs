using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MiniSwitch.DTOs;
using MiniSwitch.Services;
using MiniSwitch.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniSwitch.Controllers
{
    public class UserController : Controller
    {
        private IUserServices _userService;
        private IMapper _mapper;
        public UserController(IUserServices userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }


        [HttpGet]
        public IActionResult MiniSwitchs()
        {
            return View();
        }

        public async Task<IActionResult> GetUserProfile()
        {
            var (result, user) = await _userService.GetUserProfileAsync();
            if (result.isSuccess && user != null)
            {
                var userProfile = _mapper.Map<UserProfileResponseDTO>(user);
                return View(userProfile);
            }
            else
                return View(result);
        }

        public async Task<IActionResult> UpdateProfile([FromForm] UserProfileViewModel model)
        {
            var (result, user) = await _userService.UpdateProfileAsync(model);
            if (result.isSuccess && user != null)
            {
                var userProfile = _mapper.Map<UserProfileResponseDTO>(user);
                return View(userProfile);
            }
            else
                return View(result);
        }
    }
}
