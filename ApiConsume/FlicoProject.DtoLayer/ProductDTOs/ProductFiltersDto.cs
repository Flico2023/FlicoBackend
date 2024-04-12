using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer.ProductDTOs
{
    public class ProductFiltersDto
    {
        public List<string> Categories { get; set; }
        public List<string> Subcategories { get; set; }
        public List<string> Brands { get; set; }
        public List<string> Colors { get; set; }

        public List<string> Sizes { get; set; }

        public string productName { get; set; }

        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        public int? Id { get; set; }

        public ProductFiltersDto()
        {
            Categories = new List<string>();
            Subcategories = new List<string>();
            Brands = new List<string>();
            Colors = new List<string>();
            Sizes = new List<string>();
            productName = "";
            MinPrice = -1;
            MaxPrice = -1;
            Id = -1;
        }

    }
}
