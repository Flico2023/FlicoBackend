using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer.ProductDTOs
{
    public class ProductRequestDTO
    {
        public ProductDto Product { get; set; }
        public List<StockDetailDto> StockDetails { get; set; }
    }
}
