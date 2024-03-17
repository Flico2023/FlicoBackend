using FlicoProject.DtoLayer.ProductDTOs;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Abstract
{
    public interface IProductService : IGenericService<Product>
    {
        ResultDTO<ProductRequestDTO> ValidateProductRequestDto(ProductRequestDTO dto);

        ProductFiltersDto FormatProductFilters(string? category, string? subcategory, string? sizes, string? brand,  string? color, int? min, int? max, int? id, string productName);

       List<ProductWithDetailsDto> GetListByFilters(ProductFiltersDto filters);
    }
}
