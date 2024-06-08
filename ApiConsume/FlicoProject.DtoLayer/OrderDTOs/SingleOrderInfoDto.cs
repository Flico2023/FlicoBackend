using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.DtoLayer
{
    public class SingleOrderInfoDto
    {
        public Order Order { get; set; }
        public Closet Closet { get; set; }
        public Airport Airport { get; set; }

        public List<OrderProductDtoWithProductEntityDto> OrderProducts { get; set; }

        public SingleOrderInfoDto(Order order, Closet closet, Airport airport, List<OrderProductDtoWithProductEntityDto> orderProducts)
        {
            Order = order;
            Closet = closet;
            Airport = airport;
            OrderProducts = orderProducts;
        }

    }

}