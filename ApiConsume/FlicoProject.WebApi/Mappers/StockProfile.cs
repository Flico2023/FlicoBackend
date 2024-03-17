using AutoMapper;
using FlicoProject.DtoLayer.ProductDTOs;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class StockProfile:Profile
    
    {
        public StockProfile()
        {
            CreateMap<StockDetail, StockDetailDto>();
            CreateMap<StockDetailDto, StockDetail>();

        }
    }
}
