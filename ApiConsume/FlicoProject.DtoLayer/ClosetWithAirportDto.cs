using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class ClosetWithAirportDto
    {
        public int ClosetNo { get; set; }
        public int AirportID { get; set; }
        public int OrderID { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string AirportName { get; set; }
    }
}
