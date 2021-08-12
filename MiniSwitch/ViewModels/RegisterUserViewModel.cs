using System;
using System.ComponentModel.DataAnnotations;
using MiniSwitch.Enums;

namespace MiniSwitch.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^0[0-9]*$", ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Image { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }
    }
}
