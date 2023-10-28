using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }        
        public int Amount { get; set; }
        public string Brand { get; set; }
        public float Price { get; set; }
        public string ProductDetail { get; set; }
        public float CurrentPrice { get; set; }
        public string Gender { get; set; }           
    }
}
