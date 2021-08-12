using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MiniSwitch.Enums;

namespace MiniSwitch.DTOs
{
    public class UserProfileResponseDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string Image { get; set; }

        [Required]
        public string Country { get; set; }

        public decimal? Balance { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public UserStatusEnum UserStatus { get; set; }
    }
}
