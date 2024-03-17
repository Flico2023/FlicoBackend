using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DataAccessLayer.Abstract
{
    public interface IStockDetailDal : IGenericDal<StockDetail>
    {
        void DeleteProductStockDetails(int productID);

        void InsertStockDetails(List<StockDetail> stockDetails);

        List<StockDetail> GetStockDetailsByProductId(int productID);
    }
}
