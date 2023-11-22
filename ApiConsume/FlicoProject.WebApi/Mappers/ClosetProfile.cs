using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class ClosetProfile:Profile
    {
        public ClosetProfile()
        {
            CreateMap<Closet, ClosetDto>();
            CreateMap<ClosetDto, Closet>();
            CreateMap<Closet, ClosetWithAirportDto>();
            CreateMap<ClosetWithAirportDto, Closet>();
            CreateMap<ClosetDto, ClosetWithAirportDto>();
            CreateMap<ClosetWithAirportDto, ClosetDto>();
        }
    }
}
