using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class ProductWithFavId
    {
        public int FavouriteId { get; set; }
        public Product product { get; set; }
    }
}
