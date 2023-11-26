using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class OutsourceProfile : Profile
    {
        public OutsourceProfile()
        {
            CreateMap<Outsource, OutsourceDto>();
            CreateMap<OutsourceDto, Outsource>();
        }
    }
}
