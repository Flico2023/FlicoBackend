using FlicoProject.DtoLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Concrete.Validators.PostCartDtoValidtors
{
    public class PostCartDtoFluentValidator : AbstractValidator<PostCartDto>
    {
        public PostCartDtoFluentValidator()
        {
            RuleFor(cart => cart.ProductID)
                .GreaterThanOrEqualTo(0).WithMessage("Product id must be 0 or positive.");

            RuleFor(cart => cart.Size)
            .NotEmpty().WithMessage("Size cannot be empty.")
            .Must(size => IsOneOfValidSizes(size)).WithMessage("Size must be one of: XS, S, M, L, XL, XXL");

            RuleFor(cart => cart.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be 0 or positive.");

            RuleFor(cart => cart.UserID)
                .GreaterThanOrEqualTo(0).WithMessage("UserID must be 0 or positive.");
        }

        private bool IsOneOfValidSizes(string size)
        {
            string[] validSizes = { "XS", "S", "M", "L", "XL", "XXL" };
            return validSizes.Contains(size);
        }
    }
}
