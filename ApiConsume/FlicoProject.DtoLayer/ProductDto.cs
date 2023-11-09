using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class ProductListDTO
    {
        public List<Product> Products { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
