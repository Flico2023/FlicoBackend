using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class OrderDto
    {
        public int AirportID { get; set; }
        //public int ClosetID { get; set; }
        public int UserID { get; set; }
       // public int StuffID { get; set; }
        public string OrderStatus { get; set; }
        public float TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        

        public string NameOnCart { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
    }
}
