using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DataAccessLayer.Abstract
{
    public interface IOrderProductDal : IGenericDal<OrderProduct>
    {
        void DeleteOrderProducts(int orderID);

        void InsertOrderProducts(List<OrderProduct> orderProducts);

        List<OrderProduct> GetOrderProductsByOrderId(int orderID);
    }
}
