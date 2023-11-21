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

        public ProductController(IProductService productService, IStockDetailService stockDetailservice, IWebHostEnvironment environment)
        {
            _productService = productService;
            _stockDetailservice = stockDetailservice;
            _environment = environment;
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
public async Task<IActionResult> AddProduct(ProductWithDetails productWithDetails)
{
    var product = productWithDetails.Product;
    var stockDetails = productWithDetails.StockDetails;

    if (product.Image != null && product.Image.Length > 0)
    {
        // Güvenli bir dosya adı oluşturun ve dosya yolu oluşturun
        var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(product.Image.FileName);
        var filePath = Path.Combine(_environment.WebRootPath, "product_images", fileName);

        // Klasör yoksa oluştur
        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Dosyayı kaydet
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await product.Image.CopyToAsync(stream);
        }

        // Dosyanın yolu, Product nesnesinde saklanabilir
        product.ImagePath = filePath; // Örneğin, dosya yolunu saklamak için kullanabilirsiniz
    }

    if (_productService.TInsert(product) == 1)
    {
        var list = _productService.TGetList().FindAll(x => x.ProductName == product.ProductName);
        var pList = list.Find(x => x.Color == product.Color);
        foreach (var stockDetail in stockDetails)
        {
            stockDetail.ProductID = pList.ProductID; // Ürün ID'sini StockDetail'e atama
            var ok = _stockDetailservice.TInsert(stockDetail);
              if (ok == 0) {
                return BadRequest(new ResultDTO<ProductWithDetails>("Form values are not valid."));
              }
        }
         var details = _stockDetailservice.TGetList().FindAll(x => x.ProductID == pList.ProductID);
         product.ProductID = pList.ProductID;
         ProductWithDetails a =  new ProductWithDetails();
         a.Product = product;
         a.StockDetails = details;
        return Created("", new ResultDTO<ProductWithDetails>(a));
    }
    else
    {
        return BadRequest(new ResultDTO<ProductWithDetails>("Form values are not valid."));
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
            var variations = _stockDetailservice.TGetList().FindAll(x=>x.ProductID == id);
            var PtoS = new ProductWithDetails
            {
                Product = product,
                StockDetails = variations,
            };
            if (product == null)
            {
                return BadRequest(new ResultDTO<Product>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<ProductWithDetails>(PtoS));
        }

    }
}
