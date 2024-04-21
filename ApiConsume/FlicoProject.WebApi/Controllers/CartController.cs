using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public CartController(ICartService cartservice, IMapper mapper, IProductService productService, IUserService userService)
        {
            _cartservice = cartservice;
            _mapper = mapper;
            _productService = productService;
            _userService = userService;
        }
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult CartList([FromQuery] int pageSize, int pageIndex, int? userId, int? productId, string? status)
        {

            var carts = _cartservice.TGetList();
            if (userId != null)
            {
                carts = carts.FindAll(x => x.UserID == userId);
            }
            if (productId != null)
            {
                carts = carts.FindAll(x => x.ProductID == productId);
            }
            if (!IsNullOrEmpty(status))
            {
                carts = carts.FindAll(x => x.Status == status);
            }

            int totalCount = carts.Count;
            carts = carts.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            var cartsWithProducts = carts.Select(x => new CartWithProductDto()
            {

                CartID = x.CartID,
                Amount = x.Amount,
                Size = x.Size,
                Status = x.Status,
                User = _userService.TGetByID(x.UserID),
                Product = _productService.TGetByID(x.ProductID)

            }).ToList();

            var result = new PaginationResultDto<CartWithProductDto>() {
                Data = cartsWithProducts,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return Ok(new ResultDTO<PaginationResultDto<CartWithProductDto>>(result));
        }
        [HttpPost, Authorize(Roles = "NormalUser,Admin" )]
        public IActionResult AddCart(PostCartDto cartdto)
        {
            //burda da başkasının kartına ekleme yapabilirim gibi bir durum var. Bunu nasıl engelleyebiliriz?
            var result = _cartservice.FluentValidatePostCartDto(cartdto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

            result = _cartservice.ValidatePostCartDto(cartdto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

           
            var cart = new Cart();
            cart = _mapper.Map<Cart>(cartdto);
            
            if (_cartservice.TInsert(cart) == 1)
            {
                return Created("", new ResultDTO<Cart>(cart));
            }
            else
            {
                return BadRequest(new ResultDTO<Cart>("Cart could not be added"));
            }
        }
        [HttpDelete("{id}"), Authorize(Roles = "NormalUser,Admin")]
        public IActionResult DeleteCart(int id)
        {
            /*
             * Bir kişinin başka bir kişinin cartını silmesini önlemek için buraya userId eklemek gerekebilir
             * bunu keremle bi konuşalım. Bakalım admin panel ve stuff işin içine girince bu nasıl hallolucak
             */
            var cart = _cartservice.TGetList().Find(x => x.CartID == id);
            if (cart == null)
            {
                return BadRequest(new ResultDTO<Cart>("Item could not found."));
            }
            int result = _cartservice.TDelete(cart.CartID);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Cart>("The cart could not be deleted."));
            }
            return Ok(new ResultDTO<Cart>(cart));
            
        }
        [HttpPut("{id}"),Authorize(Roles = "NormalUser,Admin")]
        public IActionResult UpdateCart(int id,[FromBody]PostCartDto cartdto)
        {
            //BUARAYA DA BİR KONUŞALIM BAŞKA BİR KİŞİNİN KARTI GÜNCELLENMEMELİ
            //eğer ürün silinirse karttan silecek miyiz? Ne yapmalıyız. Böyle işlemleri nerde halletmeliyiz?
            //Data annotationlar ile on delete aksiyonları belirlenebiliyor mu?
            var result = _cartservice.FluentValidatePostCartDto(cartdto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

            result = _cartservice.ValidatePostCartDto(cartdto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<PostContactMessageDto>(result.Message));
            }

            var existingCart = _cartservice.TGetList().FirstOrDefault(x => x.CartID == id);

            if (existingCart == null)
            {
                return BadRequest(new ResultDTO<Cart>("Product could not be found in cart"));
            }

            if (cartdto.Amount == 0)
            {
                _cartservice.TDelete(id);
                return Ok(new ResultDTO<Cart>(existingCart));
            }

            existingCart.Amount = cartdto.Amount;
            existingCart.Size = cartdto.Size;
            //existingCart.UserID = cartdto.UserID; başkasına kart ekleyemesin diye bunu kaldırdım
            existingCart.Status = cartdto.Status;
            existingCart.ProductID = cartdto.ProductID;
            _cartservice.TUpdate(existingCart);
            return Ok(new ResultDTO<Cart>(existingCart));
        }
        [HttpGet("{id}"), Authorize(Roles = "NormalUser,Admin")]
        public IActionResult GetCart(int id)
        {
            var cart = _cartservice.TGetByID(id);
            if (cart == null)
            {
                return BadRequest(new ResultDTO<Cart>("Cart could not found."));
            }
            return Ok(new ResultDTO<Cart>(cart));
        }

    }
}
