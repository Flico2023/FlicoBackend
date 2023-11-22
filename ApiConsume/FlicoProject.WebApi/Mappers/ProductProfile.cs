using AutoMapper;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.WebApi.Mappers
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductListDTO>();
            CreateMap<ProductListDTO, Product>();
            CreateMap<Product, productDTO2>();
            CreateMap<productDTO2, Product>();
            CreateMap<productDTO2, ProductListDTO>();
            CreateMap<ProductListDTO, productDTO2>();
            CreateMap<productDTO2, ProductDto3>();
            CreateMap<ProductDto3, productDTO2>();
            CreateMap<Product, ProductDto3>();
            CreateMap<ProductDto3, Product>();

        }
    }
}
