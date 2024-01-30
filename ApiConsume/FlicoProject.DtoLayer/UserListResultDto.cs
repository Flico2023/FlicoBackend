using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlicoProject.EntityLayer.Concrete;

namespace FlicoProject.DtoLayer
{
    public class UserListResultDto
    {
        public List<User> Users { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
