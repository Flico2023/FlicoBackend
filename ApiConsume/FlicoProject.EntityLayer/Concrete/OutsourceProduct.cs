using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class OutsourceProduct
    {
        public int OutsourceProductID { get; set; }
        public int OutsourceID { get; set; }
        public int StockDetailID { get; set; }
        public int Amount { get; set; }
        public int AirportID { get; set; }
        public string Status { get; set; }

    }
}
