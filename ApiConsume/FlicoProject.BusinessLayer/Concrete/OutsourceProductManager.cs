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
    public class OutsourceProductManager : IOutsourceProductService
    {
        private readonly IOutsourceProductDal _OutsourceProductDal;

        public OutsourceProductManager(IOutsourceProductDal OutsourceProductDal)
        {
            _OutsourceProductDal = OutsourceProductDal;
        }

        public int TDelete(int id)
        {
            var outsourceProduct = _OutsourceProductDal.GetByID(id);
            if (outsourceProduct == null)
            {
                return 0;
            }
            else
            {
                _OutsourceProductDal.Delete(outsourceProduct);
                return 1;
            }
        }

        public OutsourceProduct TGetByID(int id)
        {
            return _OutsourceProductDal.GetByID(id);
        }

        public List<OutsourceProduct> TGetList()
        {
            return _OutsourceProductDal.GetList();
        }
        public int TInsert(OutsourceProduct t)
        {
            var outsourceProduct = _OutsourceProductDal.GetList().Find(x => x.OutsourceID == t.OutsourceID && x.StockDetailID == t.StockDetailID);
            if (t.OutsourceID < 0 || t.StockDetailID < 0 || t.Amount < 0 || t.AirportID < 0 || IsNullOrWhiteSpace(t.Status) || outsourceProduct != null)
            {
                return 0;
            }
            else
            {
                _OutsourceProductDal.Insert(t);
                return 1;
            }

        }
        public int TUpdate(OutsourceProduct t)
        {
            //var isused = _OutsourceProductDal.GetList().Find(x => x.OutsourceID == t.OutsourceID && x.StockDetailID == t.StockDetailID);
            var isvalid = _OutsourceProductDal.GetList().FirstOrDefault(x => x.OutsourceProductID == t.OutsourceProductID);
            if(t.OutsourceID < 0 || t.StockDetailID < 0 || t.Amount < 0 || t.AirportID < 0 || IsNullOrWhiteSpace(t.Status) ||  isvalid == null )
            {
                return 0;
            }
            else
            {
                _OutsourceProductDal.Update(t);
                return 1;

            }
        }
    }
}
