using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FlicoProject.BusinessLayer.Concrete;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.DtoLayer
{
    public class UserListResultDto
    {
        public List<AppUser> Users { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
