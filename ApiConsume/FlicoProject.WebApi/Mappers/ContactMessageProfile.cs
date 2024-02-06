using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class ContactMessageProfile : Profile
    {
        public ContactMessageProfile()
        {
            //src -> destination
            //_mapper.Map<Dest>(Src) şeklinde kullanılır.
            CreateMap<PostContactMessageDto, ContactMessage>()
                           .ForMember(dest => dest.MessageDate, opt => opt.MapFrom(src => DateTime.Now))
                           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
                           .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => ""));

        }
    }
    
}
