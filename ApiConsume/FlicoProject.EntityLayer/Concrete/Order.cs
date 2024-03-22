using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderID { get; set; }
        public int AirportID { get; set; }
        public int ClosetID { get; set; }
        public int UserID { get; set; }
        public int StuffID { get; set; }
        public string OrderStatus { get; set; }
        public float TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderProducts { get; set; }



    }
}
