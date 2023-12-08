using AutoMapper;
using AutoMapper.Internal;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
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

        public IActionResult ProductList(int PageIndex, int PageSize,string? sizes,string? brand,string? color,string? gender,int? min,int? max)
        {
            var sizes_list = new List<string>();
            var brand_list = new List<string>();
            var color_list = new List<string>();
            int i = 0;
            string totalsize="";
            if (sizes != null)
            {
                while (i < sizes.Count())
                {
                    if (sizes[i] == ',')
                    {
                        sizes_list.Add(totalsize);
                        totalsize = "";
                    }
                    else
                    {
                        totalsize = totalsize + sizes[i];
                    }
                    i++;
                }
                sizes_list.Add(totalsize);
            }
            int i2 = 0;
            string totalbrand="";
            if (brand != null)
            {
                while (i2 < brand.Count())
                {
                    if (brand[i2] == ',')
                    {
                        brand_list.Add(totalbrand);
                        totalbrand = "";
                    }
                    else
                    {
                        totalbrand = totalbrand + brand[i2];
                    }
                    i2++;
                }
                brand_list.Add(totalbrand);
            }
            int i3 = 0;
            string totalcolor = "";
            if (color != null)
            {
                while (i3 < color.Count())
                {
                    if (color[i3] == ',')
                    {
                        color_list.Add(totalcolor);
                        totalcolor = "";
                    }
                    else
                    {
                        totalcolor = totalcolor + color[i3];
                    }
                    i3++;
                }
                color_list.Add(totalcolor);
            }
            var products = _productService.TGetList();
            var plist = new List<ProductDto3>();
            foreach (Product ab in products) { 
                ProductDto3 Pdto3 = new ProductDto3();
                var variations = new List<StockDetail>();
                Pdto3 = _mapper.Map<ProductDto3>(ab);
                variations = _stockDetailservice.TGetList().FindAll(x => x.ProductID == ab.ProductID);
                Pdto3.StockDetail = variations;
                plist.Add(Pdto3);
            }
            var products2 = new List<ProductDto3>();
            if (gender != null) 
            {
                products2 = plist.FindAll(x => x.Gender == gender);
            }
            else
            {
                products2 = plist;
            }
            
            int be = 0;
            var products3 = new List<ProductDto3>();
            if (brand != null)
            {
                while (be < brand_list.Count())
                {
                    products3.AddRange(products2.FindAll(x => x.Brand == brand_list[be]));

                    be++;
                }
            }
            else 
            {
                products3 = products2;
            }
            int be2 = 0;
            var products4 = new List<ProductDto3>();
            if (color != null)
            {
                while (be2 < color_list.Count())
                {
                    products4.AddRange(products3.FindAll(x => x.Color == color_list[be2]));

                    be2++;
                }
            }
            else 
            {
                products4 = products3;
            }
            int be3 = 0;
            var products5 = new List<ProductDto3>();
            if (sizes != null)
            {
                while (be3 < sizes_list.Count())
                {
                    foreach (StockDetail ac in _stockDetailservice.TGetList())
                    {
                        if (products5.Find(x => x.ProductID == ac.ProductID) == null)
                        {
                            var af = products4.FindAll(b => b.StockDetail.Find(x => x.Size == sizes_list[be3]) != null);
                            foreach (ProductDto3 p in af)
                            {
                                if (!products5.Contains(p))
                                {
                                    products5.Add(p);
                                }
                            }


                        }

                    }
                    //products5.AddRange(products4.FindAll(b => b.StockDetail.Find(x => x.Size == sizes_list[be3]&&x.ProductID == b.ProductID) != null ));
                    be3++;

                }
            }
            else {
                products5 = products4;
            }
            var products6 = new List<ProductDto3>();
            var products7 = new List<ProductDto3>();
            if (max > 0)
            {
                products6 = products5.FindAll(x => x.Price < max);
            }
            else 
            { 
            products6 = products5; 
            }
            if (min > 0)
            {
                products7 = products6.FindAll(x => x.Price > min);
            }
            else 
            { 
                products7= products6;
            }

            var total = products6.Count();

            var chunked = products7.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

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
        var aa = _mapper.Map<LastProductdto>(productWithDetails);

            if (_productService.TInsert(product) == 1)
            {
                return Created("", new ResultDTO<LastProductdto>(aa));
            }
            else
            {
                return BadRequest(new ResultDTO<LastProductdto>("Form values are not valid."));
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
