using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class OrderProductManager : IOrderProductService
    {
        private readonly IOrderProductDal _OrderProductDal;

        public OrderProductManager(IOrderProductDal OrderProductDal)
        {
            _OrderProductDal = OrderProductDal;
        }

        public void DeleteOrderProducts(int orderID)
        {
            _OrderProductDal.DeleteOrderProducts(orderID);
        }
        public void InsertOrderProducts(List<OrderProduct> OrderProducts)
        {
            _OrderProductDal.InsertOrderProducts(OrderProducts);
        }
        public List<OrderProduct> GetOrderProductsByOrderId(int orderID)
        {
            return _OrderProductDal.GetOrderProductsByOrderId(orderID);
        }
        public int TDelete(int id)
        {
            var OrderProduct = _OrderProductDal.GetByID(id);
            if (OrderProduct == null)
            {
                return 0;
            }
            else
            {
                _OrderProductDal.Delete(OrderProduct);
                return 1;
            }
        }

        public OrderProduct TGetByID(int id)
        {
            return _OrderProductDal.GetByID(id);
        }

        public List<OrderProduct> TGetList()
        {
            return _OrderProductDal.GetList();
        }

        public int TInsert(OrderProduct t)
        {
            var OrderProduct1 = _OrderProductDal.GetList().FindAll(x => x.OrderId == t.OrderId);
            //var OrderProduct = OrderProduct1.Find(x => x.Size == t.Size);
            if (IsNullOrWhiteSpace(t.Size))
            {
                return 0;
            }
            else if (_OrderProductDal.GetList().Count == 0)
            {
                _OrderProductDal.Insert(t);
                return 1;
            }
            else
            {
                _OrderProductDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(OrderProduct t)
        {
            var isused1 = _OrderProductDal.GetList().FindAll(x => x.OrderId == t.OrderId);
            var isused = isused1.Find(x => x.Size == t.Size);
            var isvalid = _OrderProductDal.GetList().FirstOrDefault(x => x.OrderProductId == t.OrderProductId);

            if (IsNullOrWhiteSpace(t.Size) || isvalid == null)
            {
                return 0;
            }
            else
            {
                _OrderProductDal.Update(t);
                return 1;

            }
        }
    }
}

