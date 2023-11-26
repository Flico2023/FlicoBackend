using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class OutsourceProductProfile : Profile
    {
        public OutsourceProductProfile()
        {
            CreateMap<OutsourceProduct, OutsourceProductDto>();
            CreateMap<OutsourceProductDto, OutsourceProduct>();
        }
    }
}
