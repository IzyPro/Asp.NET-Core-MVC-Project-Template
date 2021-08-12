using System;
using AutoMapper;
using MiniSwitch.Models;

namespace MiniSwitch.DTOs
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, LoginResponseDTO>();
            CreateMap<User, UserProfileResponseDTO>();
        }
    }
}
