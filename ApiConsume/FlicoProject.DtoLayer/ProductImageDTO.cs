using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class ProductImageDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        // Diğer ürün alanları...
        public IFormFile ImageFile { get; set; }
    }
}
