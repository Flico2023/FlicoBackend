using FlicoProject.DtoLayer.ProductDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Concrete.Validators.ProductValidators
{

    public class ProductRequestDTOValidator : AbstractValidator<ProductRequestDTO>
    {
        public ProductRequestDTOValidator()
        {
            RuleFor(x => x.Product).NotNull().WithMessage("Product cannot be null").SetValidator(new ProductDtoValidator());
            RuleFor(x => x.StockDetails).NotNull().NotEmpty().WithMessage("StockDetails cannot be null or empty");
            RuleForEach(x => x.StockDetails).SetValidator(new StockDetailDtoValidator());
        }
    }

    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("ProductName cannot be empty");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category cannot be empty");
            RuleFor(x => x.Subcategory).NotEmpty().WithMessage("Subcategory cannot be empty");
            RuleFor(x => x.Brand).NotEmpty().WithMessage("Brand cannot be empty");
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.ProductDetail).NotEmpty().WithMessage("ProductDetail cannot be empty");
            RuleFor(x => x.Color).NotEmpty().WithMessage("Color cannot be empty");
            RuleFor(x => x.ImagePath).Must(BeAValidUrl).WithMessage("Image path must be a valid URL");
        }

        private bool BeAValidUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }

    public class StockDetailDtoValidator : AbstractValidator<StockDetailDto>
    {
        public StockDetailDtoValidator()
        {

            RuleFor(x => x.WarehouseID).NotEmpty().GreaterThan(0).WithMessage("WarehouseID must be greater than 0");
            RuleFor(x => x.Size).NotEmpty().WithMessage("Size cannot be empty");
            RuleFor(x => x.VariationAmount).NotEmpty().GreaterThan(0).WithMessage("VariationAmount must be greater than 0");
            RuleFor(x => x.VariationActiveAmount).NotEmpty().GreaterThan(0).WithMessage("VariationActiveAmount must be greater than 0").LessThanOrEqualTo(x => x.VariationAmount).WithMessage("Variation active amount must be less than or equal to variation amount");
        }
    }


}
