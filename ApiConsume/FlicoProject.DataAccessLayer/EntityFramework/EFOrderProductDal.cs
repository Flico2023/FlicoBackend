using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.Concrete;
using FlicoProject.DataAccessLayer.Repositories;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DataAccessLayer.EntityFramework
{
    public class EFOrderProductDal : GenericRepository<OrderProduct>, IOrderProductDal
    {
        public EFOrderProductDal(Context context) : base(context)
        {

        }

        public void DeleteOrderProducts(int orderID)
        {
            var ordersToDelete = _context.OrderProducts.Where(x => x.OrderId == orderID).ToList();
            _context.OrderProducts.RemoveRange(ordersToDelete);
            _context.SaveChanges();
        }

        public void InsertOrderProducts(List<OrderProduct> OrderProducts)
        {
            _context.OrderProducts.AddRange(OrderProducts);
            _context.SaveChanges();
        }

        public List<OrderProduct> GetOrderProductsByOrderId(int orderID)
        {
            return _context.OrderProducts.Where(x => x.OrderId == orderID).ToList();
        }
    }
}

