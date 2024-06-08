using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class OrderListPageDto
    {
        public List<SingleOrderInfoDto> Orders { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public OrderListPageDto(List<SingleOrderInfoDto> orders, int totalCount, int pageIndex, int pageSize)
        {
            Orders = orders;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
