using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartservice;
        private readonly IMapper _mapper;

        public CartController(ICartService cartservice, IMapper mapper)
        {
            _cartservice = cartservice;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult CartList()
        {
            var carts = _cartservice.TGetList();
            return Ok(new ResultDTO<List<Cart>>(carts));
        }
        [HttpPost]
        public IActionResult AddCart(Cartdto cartdto)
        {
            var cart = new Cart();
            cart = _mapper.Map<Cart>(cartdto);
            
            if (_cartservice.TInsert(cart) == 1)
            {

                var a = _cartservice.TGetList().Find(x => x.UserID == cart.UserID && x.StockDetailsID == cart.StockDetailsID);
                cart.CartID = a.CartID;
                return Created("", new ResultDTO<Cart>(cart));
            }
            else
            {
                return BadRequest(new ResultDTO<Cart>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var cartid = _cartservice.TDelete(id);
            if (cartid == 0)
            {
                return BadRequest(new ResultDTO<Cart>("The id to be deleted was not found."));
            }
            else
            {
                var cart = _cartservice.TGetByID(id);
                _cartservice.TDelete(id);

                return Ok(new ResultDTO<Cart>(cart));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCart(int id, Cart cart)
        {
            cart.CartID = id;
            int result = _cartservice.TUpdate(cart);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Cart>("The cart wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Cart>(cart));
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetCart(int id)
        {
            var cart = _cartservice.TGetByID(id);
            if (cart == null)
            {
                return BadRequest(new ResultDTO<Cart>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<Cart>(cart));
        }
    }
}
