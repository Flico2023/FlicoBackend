using AutoMapper;
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
        public async Task<IActionResult> AddProduct([FromForm] productDTO2 productWithDetails)
        {
            var product = _mapper.Map<Product>(productWithDetails);

    if (product.Image != null && product.Image.Length > 0)
    {
        var fileExtension = Path.GetExtension(product.Image.FileName).ToLower();
        if (fileExtension != ".png" && fileExtension != ".jpg")
        {
            return BadRequest(new ResultDTO<productDTO2>("Only PNG and JPG files are allowed."));
        }
        var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(product.Image.FileName);
        var filePath = Path.Combine(_environment.WebRootPath, "product_images", fileName);

        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await product.Image.CopyToAsync(stream);
        }

        product.ImagePath = filePath;
    }

            if (_productService.TInsert(product) == 1)
            {
                return Created("", new ResultDTO<productDTO2>(productWithDetails));
            }
            else
            {
                return BadRequest(new ResultDTO<productDTO2>("Form values are not valid."));
            }
        }


        
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var productid = _productService.TDelete(id);
            var variations = _stockDetailservice.TGetList().FindAll(x => x.ProductID == id);
            if (productid == 0)
            {
                return BadRequest(new ResultDTO<Product>("The id to be deleted was not found."));
            }
            else
            {
                var product = _productService.TGetByID(id);
                _productService.TDelete(id);
                foreach (var variation in variations) { 
                _stockDetailservice.TDelete(variation.StockDetailID);
                }
                return Ok(new ResultDTO<Product>(product));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDTO4 product)
        {
            var a = new Product();
            a = _mapper.Map<Product>(product);
            a.ProductID = id;
            int result = _productService.TUpdate(a);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Product>(("The product wanted to update could not be updated.")));
            }
            else
            {
                return Ok(new ResultDTO<Product>(a));

            }
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.TGetByID(id);
            var variations = _stockDetailservice.TGetList().FindAll(x=>x.ProductID == id);
            var PtoS = new ProductDto3();
            PtoS = _mapper.Map<ProductDto3>(product);
            
            PtoS.StockDetail = variations;
            if (product == null)
            {
                return BadRequest(new ResultDTO<ProductDto3>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<ProductDto3>(PtoS));
        }

    }
}
