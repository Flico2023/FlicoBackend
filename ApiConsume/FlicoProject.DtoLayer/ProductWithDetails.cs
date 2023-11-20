using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class ProductWithDetails
    {
        public Product Product { get; set; }
        public List<StockDetail> StockDetails { get; set; }
    }

}
