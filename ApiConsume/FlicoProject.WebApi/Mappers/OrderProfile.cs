using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.DtoLayer.ProductDTOs;
using FlicoProject.EntityLayer.Concrete;
using NanoidDotNet;

namespace FlicoProject.WebApi.Mappers
{
    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>()
                           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Order, OrderWithProductsDto>();
            CreateMap<OrderWithProductsDto, Order>();
                           
            CreateMap<Order, OrderPostWithProductsDto>();
            CreateMap<OrderPostWithProductsDto, Order>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
