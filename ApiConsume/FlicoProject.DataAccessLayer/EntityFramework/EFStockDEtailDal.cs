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
    public class EFStockDEtailDal : GenericRepository<StockDetail>, IStockDetailDal
    {
        public EFStockDEtailDal(Context context) : base(context)
        {

        }

        public void DeleteProductStockDetails(int productID)
        {
            var productsToDelete = _context.StockDetails.Where(x => x.ProductID == productID).ToList();
            _context.StockDetails.RemoveRange(productsToDelete);
            _context.SaveChanges();
        }

        public void InsertStockDetails(List<StockDetail> stockDetails)
        {
            _context.StockDetails.AddRange(stockDetails);
            _context.SaveChanges();
        }

        public List<StockDetail> GetStockDetailsByProductId(int productID)
        {
            return _context.StockDetails.Where(x => x.ProductID == productID).ToList();
        }
    }
}
