using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;
using FlicoProject.DataAccessLayer.Abstract;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/favourities")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {

        private readonly IFavouriteDal _favouriteDal;
        private readonly IMapper _mapper;

        private readonly IProductService _productService;


        public FavouriteController(IFavouriteDal Favouriteservice, IMapper mapper, IProductService productService)
        {
            _favouriteDal = Favouriteservice;
            _mapper = mapper;
            _productService = productService;
        }
        [HttpGet("{userId}")]
        public IActionResult FavouriteList(int userId)
        {
            var FavouritesData = _favouriteDal.GetList().Where(x => x.UserID == userId);

            //FavouritesData içinde productIdler var, bu idleri kullanarak productları bulan kod
            var Favourites = new List<ProductWithFavId>();
            foreach (var item in FavouritesData)
            {
                var productWithFavId = new ProductWithFavId();
                productWithFavId.FavouriteId = item.FavouriteID;
                productWithFavId.product = _productService.TGetByID(item.ProductID);
                Favourites.Add(productWithFavId);
            }

            return Ok(Favourites);
        }

        [HttpPost]
        public IActionResult AddFavourite(FavouriteDto favouriteDto)
        {
            var Favourite = new Favourite();
            Favourite = _mapper.Map<Favourite>(favouriteDto);

            if (_productService.TGetByID(Favourite.ProductID) == null)
            {
                return BadRequest(new ResultDTO<Favourite>("Product not found."));
            }


            if (_favouriteDal.GetList().Where(x => x.UserID == Favourite.UserID && x.ProductID == Favourite.ProductID).Count() > 0)
            {
                return BadRequest(new ResultDTO<Favourite>("You already has this product."));
            }

            //burda user var mı yok mu kontrolü yapılmıypr haberi olsun


            _favouriteDal.Insert(Favourite);

            return Created("", new ResultDTO<Favourite>(Favourite));


        }
        [HttpDelete("{id}")]
        public IActionResult DeleteFavourite(int id)
        {
            var favourite = _favouriteDal.GetList().Where(x => x.FavouriteID == id).FirstOrDefault();

            if (favourite == null)
            {
                return BadRequest(new ResultDTO<Favourite>("The favourite was not found."));
            }
            else
            {
                _favouriteDal.Delete(favourite);
                return Ok(new ResultDTO<Favourite>(favourite));
            }

        }

    }
}
