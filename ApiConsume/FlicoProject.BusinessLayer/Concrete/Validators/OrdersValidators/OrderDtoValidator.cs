﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlicoProject.DtoLayer;
using FluentValidation;

namespace FlicoProject.BusinessLayer.Concrete.Validators.OrdersValidators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(order  => order.AirportID)
                                .GreaterThanOrEqualTo(0).WithMessage("Airport id must be 0 or positive.");
            RuleFor(order => order.ClosetID)
                                .GreaterThanOrEqualTo(0).WithMessage("Clsoet id must be 0 or positive.");
            RuleFor(order => order.UserID)
                                .GreaterThanOrEqualTo(0).WithMessage("User id must be 0 or positive.");
            RuleFor(order => order.StuffID)
                                .GreaterThanOrEqualTo(0).WithMessage("Stuff id must be 0 or positive.");
            RuleFor(order => order.OrderStatus)
                                .NotEmpty().WithMessage("Status cannot be empty.")
                                .Must(orderstatus => IsOneofOrderStatus(orderstatus)).WithMessage("Order status must be one of; Closet, Progress, Customer");
            RuleFor(order => order.TotalPrice)
                                .GreaterThanOrEqualTo(0).WithMessage("Total price must be 0 or positive");
            RuleFor(order => order.OrderProducts)
                                .GreaterThanOrEqualTo(0).WithMessage("Order Products id must be 0 or positive.");


        }

        private bool IsOneofOrderStatus(string orderStatus)
        {
            string[] validorderstatus = { "Closet", "Progress", "Customer" };
            return validorderstatus.Contains(orderStatus);
        }
    }
}