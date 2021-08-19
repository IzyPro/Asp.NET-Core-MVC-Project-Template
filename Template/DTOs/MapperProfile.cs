using System;
using AutoMapper;
using Template.Models;

namespace Template.DTOs
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
