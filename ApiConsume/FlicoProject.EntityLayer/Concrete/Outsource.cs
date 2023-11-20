using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class Outsource
    {
        public int OutsourceId { get; set; }
        public string  CompanyName { get; set;}
        public string City{ get; set;}
        public string Address { get; set;}
        public string Email { get; set;}
        public string Phone { get; set;}    
        public string ContactPerson { get; set;}
    }
}
