//using FlicoProject.BusinessLayer.Concrete;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class CartWithProductDto
    {
        public Product Product { get; set; }

        public int CartID { get; set; }
        public string Status { get; set; }

        public string Size { get; set; }
        public int Amount { get; set; }
        public AppUser User { get; set; }

    }
}
