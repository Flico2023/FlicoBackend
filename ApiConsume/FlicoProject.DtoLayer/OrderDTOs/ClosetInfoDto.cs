using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.DtoLayer
{
    public class ClosetInfoDto
    {
        public Closet closet { get; set; }
        public Airport airport { get; set; }
    }

}