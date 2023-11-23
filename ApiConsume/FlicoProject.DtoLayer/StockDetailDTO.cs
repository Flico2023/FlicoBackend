using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;


namespace FlicoProject.DtoLayer
{
    public class StockDetailDTO
    {
        public int ProductID { get; set; }
        public int WarehouseID { get; set; }
        public string Size { get; set; }
        public int VariationAmount { get; set; }
        public int VariationActiveAmount { get; set; }
    }
}
