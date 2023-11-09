using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class StockDetail
    {
        public int StockDetailID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseID { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public string Size { get; set; }
        public int VariationAmount { get; set; }
        public int VariationActiveAmount { get; set; }
        public string Color { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
