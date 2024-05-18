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

        //NEW PART: order id, none id dolayısıyla string oldu
        public string? OrderID { get; set; }
        public int Password { get; set; }
        public string? Status { get; set; }

        public Airport Airport { get; set; }    

    }
}