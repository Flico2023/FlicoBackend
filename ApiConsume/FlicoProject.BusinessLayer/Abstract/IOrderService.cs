using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Abstract
{
    public interface IOrderService 
    {
        int TInsert(Order t);
        int TDelete(int t);
        int TUpdate(Order t);
        List<Order> TGetList();
        Order TGetByID(int id);
        ResultDTO<OrderDto> ValidatePostOrderDto(OrderDto order);
        List<Order> FilterOrderList(List<Order> orders, string status, string email, string fullname, int? id, int? UserID);


    }
}
