using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.BusinessLayer.Concrete.Validators.OrdersValidators;
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
using NanoidDotNet;
using System.Web.Http.Filters;
using FlicoProject.DtoLayer.ProductDTOs;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private IOrderProductDal _orderProductDal;
        private IAirportDal _airportDal;
        private IClosetDal _closetDal;
        private readonly IUserDal _userDal;
        private readonly IValidator<OrderPostWithProductsDto> _validator;

        public OrderManager(IOrderDal orderDal, IOrderProductDal orderProductDal, IAirportDal airportDal, IClosetDal closetDal, IUserDal userDal, IValidator<OrderPostWithProductsDto> validator)
        {
            _orderDal = orderDal;
            _orderProductDal = orderProductDal;
            _airportDal = airportDal;
            _closetDal = closetDal;
            _validator = validator;
            _userDal = userDal;
        }
        public List<OrderWithProductsDto> FilterOrderList(List<Order> orders, string status, string email, string fullname, DateTime endDate, DateTime startDate, int? id, int? UserID)
        {
            if (id != null)
            {
                orders = orders.Where(x => x.Id == id).ToList();
            }
            if (!IsNullOrWhiteSpace(email))
            {
                orders = orders.Where(x => _userDal.GetByID(x.UserID).Email == email).ToList();
            }
            if (!IsNullOrWhiteSpace(fullname))
            {
                orders = orders.Where(x => Concat(_userDal.GetByID(x.UserID).Name, _userDal.GetByID(x.UserID).Surname) == fullname).ToList();
            }
            if (!IsNullOrWhiteSpace(UserID.ToString()))
            {
                orders = orders.Where(x => x.UserID == UserID).ToList();
            }
            if (!IsNullOrWhiteSpace(status) && status != "All")
            {
                orders = orders.Where(x => x.OrderStatus == status).ToList();
            }
            if ((!IsNullOrWhiteSpace(endDate.ToString()) && !IsNullOrWhiteSpace(startDate.ToString())) && endDate >= startDate && endDate>DateTime.MinValue)
            {
                orders = orders.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).ToList();
            }

            var result = orders.Select(x => new OrderWithProductsDto
            {
                Id = x.Id,
                OrderID = x.OrderID,
                AirportID = x.AirportID,
                ClosetID = x.ClosetID,
                UserID = x.UserID,
                StuffID = x.StuffID,
                OrderStatus = x.OrderStatus,
                TotalPrice = x.TotalPrice,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreatedAt = x.CreatedAt,
                AirportName = _airportDal.GetByID(x.AirportID).AirportName,
                ClosetNo = _closetDal.GetByID(x.ClosetID).ClosetNo,
                ClosetPassword = _closetDal.GetByID(x.ClosetID).Password,
                OrderProducts = _orderProductDal.GetOrderProductsByOrderId(x.Id)
            }).ToList();


            return result;
            
        }
        public ResultDTO<OrderPostWithProductsDto> ValidatePostOrderDto(OrderPostWithProductsDto order)
        {
            var result = _validator.Validate(order);
            if(result.IsValid)
            {
                return new ResultDTO<OrderPostWithProductsDto>(order);
            }
            else
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
                var error = errors[0] ?? "Something went wrong";
                return new ResultDTO<OrderPostWithProductsDto>(errors[0]);
            }
        }
        public int TDelete(int id)
        {
            var order = _orderDal.GetByID(id);
            if (order == null)
            {
                return 0;
            } else
            {
                _orderDal.Delete(order);
                return 1;
            }
        }
        public Order TGetByID(int id) 
        {
            return _orderDal.GetByID(id);
        }

        public List<Order> TGetList() 
        {
            return _orderDal.GetList();
        }
        public int TInsert(Order t)
        {
            var a = _orderDal.GetList().Find(x => x.Id == t.Id);
            if(a != null )
            {
                return 0;
            } else
            {
                _orderDal.Insert(t);
                return 1;
            }
        }
        public int TUpdate(Order t)
        {
            var isvalid = _orderDal.GetList().FirstOrDefault(x => x.Id == t.Id);
            if (isvalid == null )
            {
                return 0;
            }
            else
            {
                _orderDal.Update(t);
                return 1;
            }
        }
    }
}
