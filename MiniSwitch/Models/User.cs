using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MiniSwitch.Enums;
using Microsoft.AspNetCore.Identity;

namespace MiniSwitch.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        public string Image { get; set; }

		[Required]
        public string Country { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public UserStatusEnum UserStatus { get; set; }

        public bool ForceChangeOfPassword { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public DateTime? LastPasswordReset { get; set; }
    }
}
