using System;
using System.ComponentModel.DataAnnotations;
using MiniSwitch.Enums;

namespace MiniSwitch.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string Image { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public UserStatusEnum UserStatus { get; set; }
    }
}
