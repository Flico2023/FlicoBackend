using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
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
            var product1 = _ProductDal.GetList().FindAll(x => x.ProductName == t.ProductName);
            var product = product1.FindAll(x => x.Color == t.Color);
            var count = product.Count();
            if (IsNullOrWhiteSpace(t.ProductName) || IsNullOrWhiteSpace(t.Category) || IsNullOrWhiteSpace(t.Subcategory) || IsNullOrWhiteSpace(t.Gender) || IsNullOrWhiteSpace(t.Brand) || IsNullOrWhiteSpace(t.ProductDetail) || t.Price < 0 || t.CurrentPrice < 0 || t.Amount < 0 ||   count >0)
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
            var isused1 = _ProductDal.GetList().FindAll(x => x.ProductName == t.ProductName);
            var isused = isused1.FindAll(x => x.Color == t.Color);
            var isvalid = _ProductDal.GetList().FirstOrDefault(x => x.ProductID == t.ProductID);
            t.ImagePath = isvalid.ImagePath;

            if (IsNullOrWhiteSpace(t.ProductName))
            {
                return 0;
            }
            else if (IsNullOrWhiteSpace(t.Category))
            {
                return 0;
            }
            else if (IsNullOrWhiteSpace(t.Subcategory))
            {
                return 0;
            }
            else if (IsNullOrWhiteSpace(t.Gender))
            {
                return 0;
            }
            else if (IsNullOrWhiteSpace(t.Brand))
            {
                return 0;
            }
            else if (IsNullOrWhiteSpace(t.ProductDetail))
            {
                return 0;
            }
            else if (t.Price < 0)
            {
                return 0;
            }
            else if (t.CurrentPrice < 0) 
            {
                return 0;
            }
            else if ( t.Amount < 0)
            {
                return 0;
            }
            else if(isused.Count() > 0)
            {
                return 0;
            }
            else if(isvalid == null)
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
