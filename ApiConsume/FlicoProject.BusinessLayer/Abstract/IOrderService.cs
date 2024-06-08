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
        //ResultDTO<OrderPostWithProductsDto> ValidatePostOrderDto(OrderPostWithProductsDto order);
        List<SingleOrderInfoDto> FilterOrderList(List<Order> orders, string status, string email, string fullname, DateTime endDate, DateTime startDate, string? id, int? UserID);
        public Closet getFirstClosetAvailable(int AirportID, DateTime StartDate, DateTime EndDate);
        ResultDTO<Product> isAllProductsAvailable(List<OrderProductDto> orderProducts, DateTime startDate, DateTime endDate);
    }
}
