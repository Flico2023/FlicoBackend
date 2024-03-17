using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer.ProductDTOs
{
    public class ProductWithDetailsDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }

        public string Brand { get; set; }
        public float Price { get; set; }
        public string ProductDetail { get; set; }
        public string Color { get; set; }
        public string ImagePath { get; set; }

        public List<StockDetail> StockDetails { get; set; }
    }
}
