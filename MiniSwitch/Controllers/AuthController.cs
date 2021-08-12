using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MiniSwitch.DTOs;
using MiniSwitch.Models;
using MiniSwitch.Services;
using MiniSwitch.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniSwitch.Controllers
{
    public class AuthController : Controller
    {
        private IUserServices _userService;
        private readonly IMapper _mapper;

        public AuthController(IUserServices userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel model)
        {
            var result = await _userService.RegisterUserAsync(model);
            if (result.isSuccess)
            {
                return RedirectToAction("Login", "Auth", result);
            }
            else
            {
                ViewBag.ErrorMsg = result.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            var (result, user, token) = await _userService.LoginUserAsync(model);
            if (result.isSuccess && user != null)
            {
				if (user.UserStatus == Enums.UserStatusEnum.Deactivated)
				{
                    ViewBag.ErrorMsg = "Your account has been deactivated please contact customer support";
                    return View();
                }
				else
				{
                    var loginResponse = _mapper.Map<LoginResponseDTO>(user);
                    loginResponse.UserRoles = await _userService.GetUserRolesAsync(user);
                    loginResponse.token = token;
                    return RedirectToAction("Dashboard", "User", loginResponse);
                }
            }
            else
            {
                ViewBag.ErrorMsg = result.Message;
                return View();
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userID, string token)
        {
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(token))
                return View();
            var result = await _userService.ConfirmEmailAsync(userID, token);
            if (result.isSuccess)
            {
                return View(result);
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userService.ForgotPasswordAsync(email);
            if (result.isSuccess)
            {
                ViewBag.Success = result.Message;
                return View(result);
            }
            else
            {
                ViewBag.ErrorMsg = result.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        {
            var result = await _userService.ResetPasswordAsync(model);
            if (result.isSuccess)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewBag.ErrorMsg = result.Message;
                return View();
            }
        }

        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordViewModel model)
        {
            var result = await _userService.ChangePasswordAsync(model);
            if (result.isSuccess)
            {
                return View(result);
            }
            return View(result);
        }

        public async Task<IActionResult> DeactivateAccount()
        {
            var result = await _userService.DeactivateAccountAsync();
            if (result.isSuccess)
                return View(result);
            return View(result);
        }
    }
}
