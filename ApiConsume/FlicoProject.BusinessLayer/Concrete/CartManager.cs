using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class CartManager : ICartService
    {
        private readonly ICartDal _CartDal;
        private readonly IValidator<PostCartDto> _postValidator;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public CartManager(ICartDal cartDal, IValidator<PostCartDto> postValidator, IUserService userService,IProductService productService )
        {
            _CartDal = cartDal;
            _postValidator = postValidator;
            _userService = userService;
            _productService = productService;
        }

        public ResultDTO<PostCartDto> FluentValidatePostCartDto(PostCartDto dto)
        {
            var result = _postValidator.Validate(dto);
            if (result.IsValid)
            {
                return new ResultDTO<PostCartDto>(dto);
            }
            else
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
                var error = errors[0] ?? "Something went wrong";
                return new ResultDTO<PostCartDto>(errors[0]);
            }
        }

        public ResultDTO<PostCartDto> ValidatePostCartDto(PostCartDto dto)
        {
            var user = _userService.TGetByID(dto.UserID);
            if (user == null)
            {
                return new ResultDTO<PostCartDto>("User not found");
            }
            var product = _productService.TGetByID(dto.ProductID);
            if (product == null)
            {
                return new ResultDTO<PostCartDto>("Product not found");
            }

            return new ResultDTO<PostCartDto>(dto);

        }

        public int TDelete(int id)
        {
            var cart = _CartDal.GetByID(id);
            if (cart == null)
            {
                return 0;
            }
            else
            {
                _CartDal.Delete(cart);
                return 1;
            }
        }

        public Cart TGetByID(int id)
        {
            return _CartDal.GetByID(id);
        }

        public List<Cart> TGetList()
        {
            return _CartDal.GetList();
        }

        public int TInsert(Cart t)
        {
            var existingCart = _CartDal.GetList().Find(x=>x.UserID == t.UserID && x.ProductID == t.ProductID && x.Size == t.Size);

            if (existingCart != null) {
                existingCart.Amount += t.Amount;
                _CartDal.Update(existingCart);
                
            }
            else
            {
                _CartDal.Insert(t);
            }

            return 1;
        }

        public int TUpdate(Cart t)
        {
            _CartDal.Update(t);
            return 1;
            
        }
    
    
    }
}
