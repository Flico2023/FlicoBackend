﻿using AutoMapper;
using FlicoProject.BusinessLayer.Concrete;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User,RegisterUser>();
            CreateMap<RegisterUser, User>();
            CreateMap<RegisterUser,AppUser>();
            CreateMap<AppUser,RegisterUser>();
        }
    }
}
