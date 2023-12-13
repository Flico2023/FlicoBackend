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
    public class CartManager : ICartService
    {
        private readonly ICartDal _CartDal;

        public CartManager(ICartDal cartDal)
        {
            _CartDal = cartDal;
        }

        public int TDelete(int id)
        {
            var cart = _CartDal.GetByID(id);
            if (cart == null)
            {
                return 0;
            }
            else
            {
                _CartDal.Delete(cart);
                return 1;
            }
        }

        public Cart TGetByID(int id)
        {
            return _CartDal.GetByID(id);
        }

        public List<Cart> TGetList()
        {
            return _CartDal.GetList();
        }

        public int TInsert(Cart t)
        {
            var a = _CartDal.GetList().Find(x=>x.UserID == t.UserID && x.StockDetailsID == t.StockDetailsID);
            if (0>t.UserID || 0>t.StockDetailsID||a !=null)
            {
                return 0;
            }
            /*else if (_CartDal.GetList().Count == 0) {
                _CartDal.Insert(t);
                return 1;
            }*/
            else
            {
                _CartDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(Cart t)
        {
            var isvalid = _CartDal.GetList().FirstOrDefault(x => x.CartID == t.CartID);

            if (0 > t.UserID || 0 > t.StockDetailsID)
            {
                return 0;
            }
            else
            {
                _CartDal.Update(t);
                return 1;

            }
        }
    }
}
