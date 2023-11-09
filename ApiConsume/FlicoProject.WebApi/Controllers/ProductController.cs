using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]

        public IActionResult ProductList(int PageIndex, int PageSize)
        {
            var products = _productService.TGetList();
            var total = products.Count;
            var chunked = products.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            var productListDTO = new ProductListDTO
            {
                Products = chunked,
                PageIndex = PageIndex,
                PageSize = PageSize,
                Total = total
            };

            return Ok(new ResultDTO<ProductListDTO>(productListDTO));
        }
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (_productService.TInsert(product) == 1)
            {
                return Created("", new ResultDTO<Product>(product));
            }
            else
            {
                return BadRequest(new ResultDTO<Product>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var productid = _productService.TDelete(id);
            if (productid == 0)
            {
                return BadRequest(new ResultDTO<Product>("The id to be deleted was not found."));
            }
            else
            {
                var product = _productService.TGetByID(id);
                _productService.TDelete(id);
                return Ok(new ResultDTO<Product>(product));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            product.ProductID = id;
            int result = _productService.TUpdate(product);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Product>(("The product wanted to update could not be updated.")));
            }
            else
            {
                return Ok(new ResultDTO<Product>(product));

            }
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.TGetByID(id);
            if (product == null)
            {
                return BadRequest(new ResultDTO<Product>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<Product>(product));
        }

    }
}
