using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class FavouriteProfile : Profile
    {
        public FavouriteProfile()
        {
            CreateMap<Favourite, FavouriteDto>();
            CreateMap<FavouriteDto, Favourite>();
        }
    }
}
