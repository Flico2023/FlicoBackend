using AutoMapper;
using AutoMapper.Internal;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.DtoLayer.ProductDTOs;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStockDetailService _stockDetailservice;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IStockDetailService stockDetailservice, IWebHostEnvironment environment, IMapper mapper)
        {
            _productService = productService;
            _stockDetailservice = stockDetailservice;
            _environment = environment;
            _mapper = mapper;
        }

        [HttpGet()]

        public IActionResult ProductList(int PageIndex, int PageSize, 
            string? category, string? subcategory, 
            string? sizes,string? brand,
            string? color,
            int? min,int? max, int? id, string? productName)
        {
           //converting filters to arrays to use later 
            var productFilters = _productService.FormatProductFilters(category, subcategory, sizes, brand, color, min, max, id, productName);

            var products = _productService.GetListByFilters(productFilters);

            //pagination
            var paginatedProducts = products.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            var response = new ProductListResponse
            {
                Products = paginatedProducts,
                PageIndex = PageIndex,
                PageSize = PageSize,
                TotalCount = products.Count,
                IsLastPage = products.Count <= PageIndex * PageSize
            };


            return Ok(new ResultDTO<ProductListResponse>(response));
        }
        
        
        [HttpPost]
        public IActionResult AddProduct(ProductRequestDTO productRequestModel)
        {
            //validation
            var result = _productService.ValidateProductRequestDto(productRequestModel);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

            //mapping
            var product = _mapper.Map<Product>(productRequestModel.Product);
            var stockDetails = productRequestModel.StockDetails.Select(x => _mapper.Map<StockDetail>(x)).ToList();

            //adding product
            if(_productService.TInsert(product) == 0)
            {
                return BadRequest(new ResultDTO<Product>("The product could not be added."));
            }

            //adding stock details
            if (stockDetails.Any())
            {
                stockDetails.ForEach(x => x.ProductID = product.ProductID);
                stockDetails.ForEach(x => _stockDetailservice.TInsert(x));
            }

            return Ok(new ResultDTO<Product>(product));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _productService.TGetByID(id);
            if (product == null)
            {
                return BadRequest(new ResultDTO<Product>("The id to be deleted was not found."));
            }

            _productService.TDelete(id);
            _stockDetailservice.DeleteProductStockDetails(id);

            return Ok(new ResultDTO<Product>(product));
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductRequestDTO productRequestModel)
        {
            //validation
            var result = _productService.ValidateProductRequestDto(productRequestModel);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

            //mapping
            var product = _mapper.Map<Product>(productRequestModel.Product);
            product.ProductID = id;
            var stockDetails = productRequestModel.StockDetails.Select(x => _mapper.Map<StockDetail>(x)).ToList();

            //updating product
            if (_productService.TUpdate(product) == 0)
            {
                return BadRequest(new ResultDTO<Product>("The product could not be updated."));
            }

            //deleting stock details
            _stockDetailservice.DeleteProductStockDetails(id);

            //adding stock details
            if (stockDetails.Any())
            {
                stockDetails.ForEach(x => x.ProductID = id);
                _stockDetailservice.InsertStockDetails(stockDetails);
            }


            return Ok(product);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.TGetByID(id);
            if (product == null)
            {
                return BadRequest(new ResultDTO<Product>("The product could not be found."));
            }

            var stockDetails = _stockDetailservice.GetStockDetailsByProductId(id);

            var productWithDetails = _mapper.Map<ProductWithDetailsDto>(product);
            productWithDetails.StockDetails = stockDetails;
            
            return Ok(productWithDetails);
        }


    }


}

