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
    public class StockDetailManager : IStockDetailService
    {
        private readonly IStockDetailDal _StockDetailDal;

        public StockDetailManager(IStockDetailDal stockDetailDal)
        {
            _StockDetailDal = stockDetailDal;
        }

        public int TDelete(int id)
        {
            var stockDetail = _StockDetailDal.GetByID(id);
            if (stockDetail == null)
            {
                return 0;
            }
            else
            {
                _StockDetailDal.Delete(stockDetail);
                return 1;
            }
        }

        public StockDetail TGetByID(int id)
        {
            return _StockDetailDal.GetByID(id);
        }

        public List<StockDetail> TGetList()
        {
            return _StockDetailDal.GetList();
        }

        public int TInsert(StockDetail t)
        {
            var stockDetail1 = _StockDetailDal.GetList().FindAll(x => x.ProductID == t.ProductID);
            var stockDetail2 = stockDetail1.FindAll(x => x.Color == t.Color);
            var stockDetail = stockDetail2.Find(x => x.Size == t.Size);
            if (IsNullOrWhiteSpace(t.Color) || IsNullOrWhiteSpace(t.Size) || stockDetail != null)
            {
                return 0;
            }
            /*else if (_StockDetailDal.GetList().Count == 0) {
                _StockDetailDal.Insert(t);
                return 1;
            }*/
            else
            {
                _StockDetailDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(StockDetail t)
        {
            var isused1 = _StockDetailDal.GetList().FindAll(x => x.ProductID == t.ProductID);
            var isused2 = isused1.FindAll(x => x.Color == t.Color);
            var isused = isused2.Find(x => x.Size == t.Size);
            var isvalid = _StockDetailDal.GetList().FirstOrDefault(x => x.StockDetailID == t.StockDetailID);

            if (IsNullOrWhiteSpace(t.Color) || IsNullOrWhiteSpace(t.Size) || isused != null || isvalid == null)
            {
                return 0;
            }
            else
            {
                _StockDetailDal.Update(t);
                return 1;

            }
        }
    }
}
