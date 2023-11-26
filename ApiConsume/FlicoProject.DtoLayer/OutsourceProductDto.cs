using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class OutsourceProductDto
    {
        public int OutsourceID { get; set; }
        public int StockDetailID { get; set; }
        public int Amount { get; set; }
        public int AirportID { get; set; }
        public string Status { get; set; }
    }
}
