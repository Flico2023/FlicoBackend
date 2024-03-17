using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer.ProductDTOs
{
    public class StockDetailDto
    {
        public int ProductID { get; set; }
        public int WarehouseID { get; set; }
        public string Size { get; set; }
        public int VariationAmount { get; set; }
        public int VariationActiveAmount { get; set; }
    }
}
