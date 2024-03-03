﻿using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, PostCartDto>();
            CreateMap<PostCartDto, Cart>();
        }
    }
}
