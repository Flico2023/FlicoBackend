using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        //public int[] Warehouses { get; set; } = new int[5];
    }
}
