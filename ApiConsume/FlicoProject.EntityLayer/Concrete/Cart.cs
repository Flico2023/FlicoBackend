using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class Cart
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public int UserID { get; set; }

    }
}
