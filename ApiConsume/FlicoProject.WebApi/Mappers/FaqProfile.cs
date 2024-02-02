using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class FaqProfile : Profile
    {
        public FaqProfile()
        {
            CreateMap<Faq, FaqDto>();
            CreateMap<FaqDto, Faq>();
        }
    }
}
