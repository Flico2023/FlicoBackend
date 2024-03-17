using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.DtoLayer;
using FlicoProject.DtoLayer.ProductDTOs;
using FlicoProject.EntityLayer.Concrete;
using FluentValidation;
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
        private IStockDetailDal _stockDetailDal;
        private readonly IValidator<ProductRequestDTO> _validator;
        public ProductManager(IProductDal productDal, IValidator<ProductRequestDTO> validator, IStockDetailDal stockDetailDal) {
            _ProductDal = productDal;
            _stockDetailDal = stockDetailDal;
            _validator = validator;
        }

        public List<ProductWithDetailsDto> GetListByFilters(ProductFiltersDto filters) { 
            var products = _ProductDal.GetList(); 
            if (filters.Brands.Count != 0)
            {
                products = products.FindAll(x => filters.Brands.Any(b => b.ToLower() == x.Brand.ToLower()));
            }
            if (filters.Categories.Count != 0)
            {
                products = products.FindAll(x => filters.Categories.Any(c => c.ToLower() == x.Category.ToLower()));
            }
            if (filters.Subcategories.Count != 0)
            {
                products = products.FindAll(x => filters.Subcategories.Any(s => s.ToLower() == x.Subcategory.ToLower()));
            }
            if (filters.Colors.Count != 0)
            {
                products = products.FindAll(x => filters.Colors.Any(c => c.ToLower() == x.Color.ToLower()));
            }

            if(!IsNullOrEmpty(filters.productName))
            {
                products = products.FindAll(x => x.ProductName.ToLower().Contains(filters.productName.ToLower()));
            }

            if (filters.MinPrice != -1)
            {
                products = products.FindAll(x => x.Price >= filters.MinPrice);
            }
            if (filters.MaxPrice != -1)
            {
                products = products.FindAll(x => x.Price <= filters.MaxPrice);
            }
            if (filters.Id != -1)
            {
                products = products.FindAll(x => x.ProductID == filters.Id);
            }

            //Size için büyük küçük kontrolü yanlış olabilir dikkat et
            if (filters.Sizes.Count != 0)
            {
                products = products.FindAll(
                    product =>
                    _stockDetailDal.GetStockDetailsByProductId(product.ProductID)
                    .Select(stockDetail => stockDetail.Size.ToLower())
                    .Any(stockDetailSize => filters.Sizes.Contains(stockDetailSize))
                    );

            }

            var result = products.Select(x => new ProductWithDetailsDto
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                Category = x.Category,
                Subcategory = x.Subcategory,
                Brand = x.Brand,
                Price = x.Price,
                ProductDetail = x.ProductDetail,
                Color = x.Color,
                ImagePath = x.ImagePath,
                StockDetails = _stockDetailDal.GetStockDetailsByProductId(x.ProductID)
            }).ToList();

            return result;
        }

        public ProductFiltersDto FormatProductFilters(string? category, string? subcategory, string? sizes, string? brand, string? color, int? min, int? max, int? id, string? productName)
        {
            var productFilters = new ProductFiltersDto();
            if (category != null)
            {
                productFilters.Categories = category.Split(",").Select(x => x.ToLower()).ToList();
            }
            if (subcategory != null)
            {
                productFilters.Subcategories = subcategory.Split(",").Select(x => x.ToLower()).ToList();
            }
            if (brand != null)
            {
                productFilters.Brands = brand.Split(",").Select(x => x.ToLower()).ToList();
            }
            if (color != null)
            {
                productFilters.Colors = color.Split(",").Select(x => x.ToLower()).ToList();
            }
            if (sizes != null)
            {
                productFilters.Sizes = sizes.Split(",").Select(x=>x.ToLower()).ToList();
            }
            if (min != null)
            {
                productFilters.MinPrice = min;
            }
            if (max != null)
            {
                productFilters.MaxPrice = max;
            }
            if (id != null)
            {
                productFilters.Id = id;
            }
            if (!IsNullOrEmpty(productName))
            {
                productFilters.productName = productName;
            }

            return productFilters;
        }


        public ResultDTO<ProductRequestDTO> ValidateProductRequestDto(ProductRequestDTO dto)
        {
            var result = _validator.Validate(dto);
            if (result.IsValid)
            {
                return new ResultDTO<ProductRequestDTO>(dto);
            }
            else
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
                var error = errors[0] ?? "Something went wrong";
                return new ResultDTO<ProductRequestDTO>(errors[0]);
            }
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
            if (IsNullOrWhiteSpace(t.ProductName) || IsNullOrWhiteSpace(t.Category) || IsNullOrWhiteSpace(t.Subcategory)  || IsNullOrWhiteSpace(t.Brand) || IsNullOrWhiteSpace(t.ProductDetail) || t.Price < 0 ||   count >0)
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
