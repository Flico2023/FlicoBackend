using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class StockProfile:Profile
    
    {
        public StockProfile()
        {
            CreateMap<StockDetail, StockDetailDTO>();
            CreateMap<StockDetailDTO, StockDetail>();
        }
    }
}
