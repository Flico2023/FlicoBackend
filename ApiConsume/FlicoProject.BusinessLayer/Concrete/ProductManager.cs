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
    public class ProductManager : IProductService 
    {
        private readonly IProductDal _ProductDal;

        public ProductManager(IProductDal productDal) {
            _ProductDal = productDal;
        }
        public int TDelete(int id)
        {
            var product = _ProductDal.GetByID(id);
            if (product == null)
            {
                return 0;
            }
            else
            {
                _ProductDal.Delete(product);
                return 1;
            }
        }

        public Product TGetByID(int id)
        {
            return _ProductDal.GetByID(id);
        }

        public List<Product> TGetList()
        {
            return _ProductDal.GetList();
        }

        public int TInsert(Product t)
        {
            var product = _ProductDal.GetList().Find(x => x.ProductName == t.ProductName);
            if (IsNullOrWhiteSpace(t.ProductName) || IsNullOrWhiteSpace(t.Category) || IsNullOrWhiteSpace(t.Subcategory) || IsNullOrWhiteSpace(t.Gender) || IsNullOrWhiteSpace(t.Brand) || IsNullOrWhiteSpace(t.ProductDetail) || t.Price < 0 || t.CurrentPrice < 0 || t.Amount < 0 ||   product != null)
            {
                return 0;
            }
            else
            {
                _ProductDal.Insert(t);
                return 1;
            }
        }

        public int TUpdate(Product t)
        {
            var isused = _ProductDal.GetList().FirstOrDefault(x => x.ProductName == t.ProductName);
            var isvalid = _ProductDal.GetList().FirstOrDefault(x => x.ProductID == t.ProductID);

            if (IsNullOrWhiteSpace(t.ProductName) || IsNullOrWhiteSpace(t.Category) || IsNullOrWhiteSpace(t.Subcategory) || IsNullOrWhiteSpace(t.Gender) || IsNullOrWhiteSpace(t.Brand) || IsNullOrWhiteSpace(t.ProductDetail) || t.Price < 0 || t.CurrentPrice < 0 || t.Amount < 0 || isused != null || isvalid == null)
            {
                return 0;
            }
            else
            {
                _ProductDal.Update(t);
                return 1;

            }
        }
    }
}
