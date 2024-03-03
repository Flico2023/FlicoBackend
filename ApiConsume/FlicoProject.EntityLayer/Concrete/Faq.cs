using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class Faq
    {
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public string Category { get; set; }

    }
}
