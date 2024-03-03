using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class PostCartDto
    {
        public int ProductID { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public int UserID { get; set; }

        public string Status { get; set; }

    }
}
