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
        public int ProductID { get; set; }

        public int WarehouseID { get; set; }
        public string Size { get; set; }
        public int VariationAmount { get; set; }
        public int VariationActiveAmount { get; set; }
        
    }
}
