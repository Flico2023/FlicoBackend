using FlicoProject.DtoLayer.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class OrderPostWithProductsDto
    {
        public OrderDto Order { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; }
    }
}
