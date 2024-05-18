using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Abstract
{
    public interface IOrderProductService : IGenericService<OrderProduct>
    {
        void DeleteOrderProducts(int orderID);

        void InsertOrderProducts(List<OrderProduct> orderProducts);

        List<OrderProduct> GetOrderProductsByOrderId(int orderID);
    }
}
