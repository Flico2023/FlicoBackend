using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer.ProductDTOs
{
    public class ProductListResponse
    {
        public int TotalCount { get; set; }
        
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public bool IsLastPage { get; set; }
        public List<ProductWithDetailsDto> Products { get; set; }
    }
}
