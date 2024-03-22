using AutoMapper;
using FlicoProject.DtoLayer;
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

        }
    }
}
