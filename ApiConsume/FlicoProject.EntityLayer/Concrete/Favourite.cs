using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class Favourite
    {
        public int FavouriteID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }

    }
}
