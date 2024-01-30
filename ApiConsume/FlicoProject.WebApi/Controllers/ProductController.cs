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

        public IActionResult ProductList(int PageIndex, int PageSize, string? category, string? subcategory, string? sizes,string? brand,string? color,string? gender,int? min,int? max)
        {
            var sizes_list = new List<string>();
            var brand_list = new List<string>();
            var color_list = new List<string>();
            var category_list = new List<string>();
            var subcategory_list = new List<string>();
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
            int i4 = 0;
            string totalcategory = "";
            if (category != null)
            {
                while (i4 < category.Count())
                {
                    if (category[i4] == ',')
                    {
                        category_list.Add(totalcategory);
                        totalcategory = "";
                    }
                    else
                    {
                        totalcategory = totalcategory + category[i4];
                    }
                    i4++;
                }
                category_list.Add(totalcategory);
            }
            int i5 = 0;
            string totalsubcategory = "";
            if (subcategory != null)
            {
                while (i5 < subcategory.Count())
                {
                    if (subcategory[i5] == ',')
                    {
                        subcategory_list.Add(totalsubcategory);
                        totalsubcategory = "";
                    }
                    else
                    {
                        totalsubcategory = totalsubcategory + subcategory[i5];
                    }
                    i5++;
                }
                subcategory_list.Add(totalsubcategory);
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
            var products8 = new List<ProductDto3>();
            var products9 = new List<ProductDto3>();
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
            int be4 = 0;
            if (category != null)
            {
                while (be4 < category_list.Count())
                {
                    products8.AddRange(products7.FindAll(x => x.Category == category_list[be4]));

                    be4++;
                }
            }
            else
            {
                products8 = products7;
            }
            int be5 = 0;
            if (subcategory != null)
            {
                while (be5 < subcategory_list.Count())
                {
                    products9.AddRange(products8.FindAll(x => x.Subcategory == subcategory_list[be5]));

                    be5++;
                }
            }
            else
            {
                products9 = products8;
            }

            var total = products9.Count();

            var chunked = products9.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

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

            if(product.Image == null)
            {
                return BadRequest(new ResultDTO<productDTO2>("Image is required."));
            }

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
        var aa = _mapper.Map<LastProductdto>(product);

            if (_productService.TInsert(product) == 1)
            {
                var aaa =_productService.TGetList().Find(x=>x.ProductName == product.ProductName && x.Color == product.Color);
                aa.ProductID = aaa.ProductID;
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

        [HttpPost("load")]
        public IActionResult LoadProducts(int numOfProducts)
        {
            var loaders = new Loaders();
            var random = new Random();
            var products = loaders.GenerateProducts(numOfProducts);

            foreach (var product in products)
            {
                if (_productService.TInsert(product) != 1)
                {
                    return BadRequest(new ResultDTO<Product>("Product Failed"));
                }

                var productID = _productService.TGetList().Find(x => x.ProductName == product.ProductName && x.Color == product.Color).ProductID;
                
                var stockDetails = loaders.GenerateStockDetail(productID);

                foreach (var stockDetail in stockDetails)
                {
                    if(_stockDetailservice.TInsert(stockDetail) != 1)
                    {
                        return BadRequest(new ResultDTO<StockDetail>("StockDetail Failed"));
                    }
                }


            }

            return Ok();
        }



    }

  
    public class Loaders
    {

        public List<Product> GenerateProducts(int numOfProducts)
        {
            var categories = new List<string>() { "man", "woman" };
            List<string> colors = new List<string>() { "Red", "Blue", "Green", "Yellow", "Black" };
            var subcategories = new List<string>() { "shirt", "pants", "shorts", "jogger", "pajamas" };
            List<string> brands = new List<string>() { "nike", "lcw", "koton", "mavi", "colins" };
            var genders = new List<string>() { "man", "woman" };
            List<string> womanImagePaths = new List<string>()
            {
                "https://img-lcwaikiki.mncdn.com/mnresize/1200/1600/mpsellerportal/v0/img_114224834v0_db585d0a-ded1-40e8-8c0d-8657885b854e.jpg",
                "https://img-lcwaikiki.mncdn.com/mnresize/1020/1360/mpsellerportal/v1/img_082207404v1_cca4563c-c289-4f3d-996a-b8f45bb176f4.jpg",
                "https://img-lcwaikiki.mncdn.com/mnresize/1020/1360/pim/productimages/20232/6794573/v2/l_20232-w3g740z8-cvl-83-61-89-179_a.jpg",
                "https://img-lcwaikiki.mncdn.com/mnresize/600/800/mpsellerportal/v1/img_085432115v1_5b65e727-7fa8-4537-9df3-2780aef2b434.jpg",
              };
            List<string> manImagePaths = new List<string>()
            {
                "https://img-lcwaikiki.mncdn.com/mnresize/600/800/pim/productimages/20241/6762420/v1/l_20241-s40408z8-rfd-102-88-96-190_a.jpg",
                "https://img-lcwaikiki.mncdn.com/mnresize/600/800/pim/productimages/20231/6180433/v1/l_20231-s31299z8-rfg-96-81-93-190_a.jpg",
                "https://img-lcwaikiki.mncdn.com/mnresize/600/800/pim/productimages/20232/6444657/v2/l_20232-w30897z8-hcz-100-79-96-188_a.jpg",
                "https://img-lcwaikiki.mncdn.com/mnresize/600/800/mpsellerportal/v1/img_022357893v1_01abbb28-f253-4940-8baf-77cd30fbd31e.jpg"
            };
            Product product = new Product();
            List<Product> products = new List<Product>();

            for (int i = 0; i < numOfProducts; i++)
            {
                Random rnd = new Random();
                product.ProductName = "product " + i;



                int random = rnd.Next(0, categories.Count);
                product.Category = categories[random];

                random = rnd.Next(0, subcategories.Count);
                product.Subcategory = subcategories[random];

                random = rnd.Next(0, colors.Count);
                product.Color = colors[random];

                random = rnd.Next(0, brands.Count);
                product.Brand = brands[random];

                random = rnd.Next(0, genders.Count);
                product.Gender = genders[random];

                if (product.Gender == "man")
                {
                    random = rnd.Next(0, manImagePaths.Count);
                    product.ImagePath = manImagePaths[random];
                }
                else
                {
                    random = rnd.Next(0, womanImagePaths.Count);
                    product.ImagePath = womanImagePaths[random];
                }

                product.Amount = rnd.Next(10, 100);
                product.Price = rnd.Next(10, 100);
                product.CurrentPrice = rnd.Next(10, 100);
                product.ProductDetail = "Detail " + i;
                product.Image = null;

                products.Add(product);
            }


            return products;
        }


        public List<StockDetail> GenerateStockDetail(int id)
        {
            var sizes = new List<string>() { "S", "M", "L", "XL", "XXL" };
            var stockDetails = new List<StockDetail>();
            Random rnd = new Random();
            int numOfStockDetails = rnd.Next(1, 5);

            for (int i = 0; i < numOfStockDetails; i++)
            {

                StockDetail stockDetail = new StockDetail();
                stockDetail.ProductID = id;
                stockDetail.Size = sizes[rnd.Next(0, sizes.Count)];
                stockDetail.VariationAmount = 10;
                stockDetail.VariationActiveAmount = 10;
                stockDetail.WarehouseID = 1;

                stockDetails.Add(stockDetail);
            }

            return stockDetails;
        }
    }

}

