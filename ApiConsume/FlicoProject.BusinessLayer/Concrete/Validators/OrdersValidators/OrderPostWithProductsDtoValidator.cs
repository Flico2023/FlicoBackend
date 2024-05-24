using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlicoProject.BusinessLayer.Concrete.Validators.ProductValidators;
using FlicoProject.DtoLayer;
using FlicoProject.DtoLayer.ProductDTOs;
using FluentValidation;

namespace FlicoProject.BusinessLayer.Concrete.Validators.OrdersValidators
{
    public class OrderPostWithProductsDtoValidator : AbstractValidator<OrderPostWithProductsDto>
    {
        public OrderPostWithProductsDtoValidator()
        {
            RuleFor(x => x.Order).NotNull().WithMessage("Order cannot be null").SetValidator(new OrderDtoValidator());
            RuleFor(x => x.OrderProducts).NotNull().NotEmpty().WithMessage("Order Products cannot be null or empty");
            RuleForEach(x => x.OrderProducts).SetValidator(new OrderProductslDtoValidator());
        }
    }
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(order  => order.AirportID)
                                .GreaterThanOrEqualTo(0).WithMessage("Airport id must be 0 or positive.");
            RuleFor(order => order.UserID)
                                .GreaterThanOrEqualTo(0).WithMessage("User id must be 0 or positive.");
            RuleFor(order => order.StuffID)
                                .GreaterThanOrEqualTo(0).WithMessage("Stuff id must be 0 or positive.");
            RuleFor(order => order.OrderStatus)
                                .NotEmpty().WithMessage("Status cannot be empty.")
                                .Must(orderstatus => IsOneofOrderStatus(orderstatus)).WithMessage("Order status must be one of; Closet, Progress, Customer");
            RuleFor(order => order.TotalPrice)
                                .GreaterThanOrEqualTo(0).WithMessage("Total price must be 0 or positive");


        }

        private bool IsOneofOrderStatus(string orderStatus)
        {
            string[] validorderstatus = { "Closet", "Progress", "Customer" };
            return validorderstatus.Contains(orderStatus);
        }
    }

    public class OrderProductslDtoValidator : AbstractValidator<OrderProductDto>
    {
        public OrderProductslDtoValidator()
        {

            RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id must be greater than 0");
            RuleFor(x => x.Size).NotEmpty().WithMessage("Size cannot be empty");
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0).WithMessage("Amount must be greater than 0");
           
        }
    }
}
