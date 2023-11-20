using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class Closet
    {
        public int ClosetID { get; set; }
        public int ClosetNo { get; set; }
        public int AirportID { get; set; }
        public int OrderID { get; set; }
        public int Password { get; set; }
        public string Status { get; set; }

    }
}