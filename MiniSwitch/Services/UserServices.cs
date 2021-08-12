using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MiniSwitch.Data;
using MiniSwitch.Enums;
using MiniSwitch.Models;
using MiniSwitch.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MiniSwitch.Services
{
    public interface IUserServices
    {
        Task<ResponseManager> RegisterUserAsync(RegisterUserViewModel model);
        Task<Tuple<ResponseManager, User, string>> LoginUserAsync(LoginViewModel model);
        Task<ResponseManager> ConfirmEmailAsync(string userID, string token);
        Task<ResponseManager> ForgotPasswordAsync(string email);
        Task<ResponseManager> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<User> GetUserById(string userId);
        Task<User> GetUser();
        //Task<Tuple<ResponseManager, User>> AddBalance(User user, decimal balance);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<ResponseManager> ChangePasswordAsync(ChangePasswordViewModel model);
        Task<ResponseManager> DeactivateAccountAsync();

        Task<Tuple<ResponseManager, User>> UpdateProfileAsync(UserProfileViewModel user);
        Task<Tuple<ResponseManager, User>> GetUserProfileAsync();


	}

    public class UserServices : IUserServices
    {
        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        private IMailServices _mailService;
		private IMailTemplateServices _mailTemplateServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private MiniSwitchContext _context;
		//private ILogger _logger;

        public UserServices(UserManager<User> userManager, IConfiguration configuration, IMailServices mailService, IHttpContextAccessor httpContextAccessor, MiniSwitchContext context, IMailTemplateServices mailTemplateServices)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
			_mailTemplateServices = mailTemplateServices;
			//_logger = logger;
        }

		//CONFIRM EMAIL
		public async Task<ResponseManager> ConfirmEmailAsync(string userID, string token)
		{
			var user = await _userManager.FindByIdAsync(userID);
			if (user == null)
			{
				return new ResponseManager
				{
					Message = "User does not exist",
					isSuccess = false
				};
			}
			var decodedToken = WebEncoders.Base64UrlDecode(token);
			string confirmEmailToken = Encoding.UTF8.GetString(decodedToken);
			var result = await _userManager.ConfirmEmailAsync(user, confirmEmailToken);

			if (result.Succeeded)
			{
				return new ResponseManager
				{
					Message = "Email Confirmed",
					isSuccess = true
				};
			}
			else
			{
				return new ResponseManager
				{
					Message = result.Errors.Select(e => e.Description).ToString(),
					isSuccess = false,
				};
			}
		}



		//FORGOT PASSWORD
		public async Task<ResponseManager> ForgotPasswordAsync(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return new ResponseManager
				{
					Message = "Enter your email address",
					isSuccess = false,
				};
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return new ResponseManager
				{
					Message = "No user with this email exists",
					isSuccess = false,
				};
			}
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var encodedToken = Encoding.UTF8.GetBytes(token);
			var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);

			//string url = $"{_configuration["BaseURL"]}/api/Auth/resetPassword?email={email}&token={validEmailToken}";
			//string emailBody = $"<h4>{validEmailToken}</h4>";

			var emailTemplate = await _mailTemplateServices.GetTemplateAsync(EmailTypesEnum.ResetPassword);
			if (emailTemplate == null)
			{
				return new ResponseManager
				{
					Message = "Unable to fetch email template",
					isSuccess = false,
				};
			}
			string url = $"{_configuration["BaseURL"]}/Auth/ResetPassword?email={user.Email}&token={validEmailToken}";
			emailTemplate.Content.Replace("{URL}", url);
			await _mailService.SendMailAsync(user, user.Email, emailTemplate.Subject, emailTemplate.Content, true);

			return new ResponseManager
			{
				isSuccess = true,
				Message = "A password reset link has been sent to your email"
			};
		}



		//RESET PASSWORD
		public async Task<ResponseManager> ResetPasswordAsync(ResetPasswordViewModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return new ResponseManager
				{
					Message = "No user with this email exists",
					isSuccess = false,
				};
			}
			if (model.NewPassword != model.ConfirmPassword)
			{
				return new ResponseManager
				{
					Message = "Passwords do not match",
					isSuccess = false,
				};
			}
			var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
			string resetPasswordToken = Encoding.UTF8.GetString(decodedToken);
			var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, model.NewPassword);
			if (result.Succeeded)
			{
				user.LastPasswordReset = DateTime.Now;
				return new ResponseManager
				{
					isSuccess = true,
					Message = "Reset password successful"
				};
			}
			return new ResponseManager
			{
				isSuccess = false,
				Message = result.Errors.Select(e => e.Description).ToString(),
			};
		}



		//GENRATE TOKEN
		public async Task<string> GenerateToken(User user)
		{
			var userRole = await _userManager.GetRolesAsync(user);
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, $"{user.Firstname} {" "} {user.Lastname}"),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
			};

			foreach (var role in userRole)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(1),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}



		//LOGIN
		public async Task<Tuple<ResponseManager, User, string>> LoginUserAsync(LoginViewModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			ResponseManager response = null;

			if (user == null)
			{
				response = new ResponseManager
				{
					Message = "There is no account with this email",
					isSuccess = false,
				};
				return new Tuple<ResponseManager, User, string>(response, null, null);
			}
			var result = await _userManager.CheckPasswordAsync(user, model.Password);

			if (!result)
			{
				response = new ResponseManager
				{
					Message = "Incorrect password",
					isSuccess = false,
				};
				return new Tuple<ResponseManager, User, string>(response, null, null);
			}

			if (user.UserStatus == UserStatusEnum.Deactivated)
				user.UserStatus = UserStatusEnum.Active;
			user.LastLoginTime = DateTime.Now;
			await _userManager.UpdateAsync(user);
			var token = await GenerateToken(user);
			response = new ResponseManager
			{
				Message = "Login Successful",
				isSuccess = true,
			};
			return new Tuple<ResponseManager, User, string>(response, user, token);
		}



		//REGISTER
		public async Task<ResponseManager> RegisterUserAsync(RegisterUserViewModel model)
		{
			if (model == null)
				throw new NullReferenceException("Invalid signup model");

			var existinguser = await _userManager.FindByEmailAsync(model.Email);
			if (existinguser != null)
				return new ResponseManager
				{
					Message = "An account with this email already exists",
					isSuccess = false,
				};

			if (model.Password != model.ConfirmPassword)
				return new ResponseManager
				{
					Message = "Passwords do not match",
					isSuccess = false,
				};


			var user = new User
			{
				Email = model.Email,
				UserName = model.Email,
				PhoneNumber = model.PhoneNumber,
				Firstname = model.Firstname,
				Lastname = model.Lastname,
				Image = model.Image,
				Country = model.Country,
				Gender = model.Gender,
				UserStatus = UserStatusEnum.Active,
				DateCreated = DateTime.Now,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, UserRolesEnum.User.ToString());

				var emailTemplate = await _mailTemplateServices.GetTemplateAsync(EmailTypesEnum.NewUserWelcomeMessage);
				if (emailTemplate == null)
				{
					return new ResponseManager
					{
						Message = "Unable to fetch email template",
						isSuccess = false,
					};
				}
				emailTemplate.Subject.Replace("{NAME}", user.Firstname);
				emailTemplate.Content.Replace("{NAME}", user.Firstname);
				await _mailService.SendMailAsync(user, user.Email, emailTemplate.Subject, emailTemplate.Content, true);

				var sendMail = await SendConfirmationEmail(user);
                //if (!sendMail.isSuccess)
                //    _logger.LogError("Failed to send confirmation mail");

                return new ResponseManager
				{
					isSuccess = true,
					Message = "User created successfully",
				};
			}
			else
			{
				return new ResponseManager
				{
					isSuccess = false,
					Message = result.Errors.Select(e => e.Description).ToString(),
				};
			}
		}




		//SEND EMAIL CONFIRMATION
		public async Task<ResponseManager> SendConfirmationEmail(User user)
		{
			if(user == null)
			{
				return new ResponseManager
				{
					Message = "User cannot be null",
					isSuccess = false,
				};
			}
			var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var encodedConfirmEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
			var validConfirmEmailToken = WebEncoders.Base64UrlEncode(encodedConfirmEmailToken);

			var emailTemplate = await _mailTemplateServices.GetTemplateAsync(EmailTypesEnum.ConfirmEmail);
			if (emailTemplate == null)
			{
				return new ResponseManager
				{
					Message = "Unable to fetch email template",
					isSuccess = false,
				};
			}
			string url = $"{_configuration["BaseURL"]}/Auth/ConfirmEmail?userID={user.Id}&token={validConfirmEmailToken}";
			emailTemplate.Content.Replace("{NAME}", user.Firstname);
			emailTemplate.Content.Replace("{LINK}", url);
			await _mailService.SendMailAsync(user, user.Email, emailTemplate.Subject, emailTemplate.Content, true);

			return new ResponseManager
			{
				Message = "Confirmation email sent successfully",
				isSuccess = true,
			};
		}






		//CHANGE PASSWORD
		public async Task<ResponseManager> ChangePasswordAsync(ChangePasswordViewModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
				return new ResponseManager
				{
					Message = "There is no account with this email",
					isSuccess = false,
				};
			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

			if (!isPasswordCorrect)
				return new ResponseManager
				{
					Message = "Incorrect Password",
					isSuccess = false,
				};
			var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

			if (result.Succeeded)
			{
				user.LastPasswordChange = DateTime.Now;
				return new ResponseManager
				{
					Message = "Password changed successfully",
					isSuccess = true,
				};
			}
			else
				return new ResponseManager
				{
					Message = result.Errors.Select(e => e.Description).ToString(),
					isSuccess = false,
				};
		}





		//UPDATE PROFILE
		public async Task<Tuple<ResponseManager, User>> UpdateProfileAsync(UserProfileViewModel model)
		{
			var user = await _userManager.FindByIdAsync(model.Id);
			ResponseManager response = null;
			if (user == null)
			{
				response = new ResponseManager
				{
					Message = "There is no account with this email",
					isSuccess = false,
				};
				return new Tuple<ResponseManager, User>(response, null);
			}
			user.Email = model.Email;
			user.Country = model.Country;
			user.Firstname = model.Firstname;
			user.Gender = model.Gender;
			user.Image = model.Image;
			user.Lastname = model.Lastname;
			user.PhoneNumber = model.PhoneNumber;
			user.Image = model.Image;

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				response = new ResponseManager
				{
					Message = "Profile updated successfully",
					isSuccess = true,
				};
				return new Tuple<ResponseManager, User>(response, user);
			}
			else
			{
				response = new ResponseManager
				{
					Message = result.Errors.Select(e => e.Description).ToString(),
					isSuccess = false,
				};
				return new Tuple<ResponseManager, User>(response, null);
			}
		}




		//GET USER PROFILE
		public async Task<Tuple<ResponseManager, User>> GetUserProfileAsync()
		{
			var result = await GetUser();
			ResponseManager response = null;
			if (result != null)
			{
				response = new ResponseManager
				{
					Message = "Found User",
					isSuccess = true,
				};
				return new Tuple<ResponseManager, User>(response, result);
			}
			else
			{
				response = new ResponseManager
				{
					Message = "No user found",
					isSuccess = false,
				};
				return new Tuple<ResponseManager, User>(response, null);
			}
		}




		//DEACTIVATE ACCOUNT
		public async Task<ResponseManager> DeactivateAccountAsync()
		{
			var user = await GetUser();
			if (user == null)
				return new ResponseManager
				{
					isSuccess = false,
					Message = "Unable to fetch user"
				};
			user.UserStatus = UserStatusEnum.Deactivated;
			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return new ResponseManager
				{
					isSuccess = true,
					Message = $"User with email {user.Email} deactivated successfully"
				};
			}
			else
			{
				return new ResponseManager
				{
					isSuccess = false,
					Message = result.Errors.Select(e => e.Description).ToString(),
				};
			}
		}



		//GET USER
		public async Task<User> GetUser()
		{
			var userID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userID != null)
				return await _userManager.FindByIdAsync(userID);
			return null;
		}



		//GET USER BY ID
		public async Task<User> GetUserById(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}




		//GET ROLES
		public async Task<List<string>> GetUserRolesAsync(User user)
		{
			return new List<string>(await _userManager.GetRolesAsync(user));
		}
	}
}
