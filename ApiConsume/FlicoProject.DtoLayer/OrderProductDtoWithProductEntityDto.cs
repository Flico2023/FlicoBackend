using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.DtoLayer
{
    public class OrderProductDtoWithProductEntityDto
    {
        public Product product { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        //public int[] Warehouses { get; set; } = new int[5];
    }
}
