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
        private readonly IUserDal _userDal;
        // private readonly IValidator<OrderPostWithProductsDto> _validator;

        private readonly IClosetService _closetManager;

        private readonly IAirportService _airportManager;

        private readonly IProductService _productService;
        private readonly  IStockDetailService _stockDetailService;


        public OrderManager(IOrderDal orderDal, IOrderProductDal orderProductDal, IUserDal userDal,
            IStockDetailService stockDetailService,
            //IValidator<OrderPostWithProductsDto> validator, 
            IClosetService closetManager, IAirportService airportManager,
            IProductService productService)
        {
            _orderDal = orderDal;
            _orderProductDal = orderProductDal;
            // _validator = validator;
            _userDal = userDal;
            _closetManager = closetManager;
            _airportManager = airportManager;
            _productService = productService;
            _stockDetailService = stockDetailService;
        }

        public ResultDTO<Product> isAllProductsAvailable(List<OrderProductDto> orderProducts, DateTime startDate, DateTime endDate)
        {

            //fetch all orders in the given date range
            var ordersInDateRange = _orderDal.GetList().Where(x => !(endDate <= x.StartDate || startDate >= x.EndDate)).ToList();

            foreach (var orderProduct in orderProducts)
            {
                var product = _productService.TGetByID(orderProduct.ProductId);
                if (product == null)
                {
                    return new ResultDTO<Product>("Product not found");
                }

                //bir ürünün bir bedenine ait toplam stok miktarı
                var stockDetails = _stockDetailService.GetStockDetailsByProductId(product.ProductID);
                var totalAmountOfProductGivenSize = stockDetails.Where(x => x.Size == orderProduct.Size).Sum(x => x.VariationAmount);


                //bir ürünün bir bedenine ait toplam sipariş miktarı
                var orderedProducts = _orderProductDal.GetList().Where(x => x.ProductId == product.ProductID).ToList();
                var totalAmountOfProductGivenSizeInOrders = orderedProducts.Where(x => x.Size == orderProduct.Size).Sum(x => x.Amount);

                if (totalAmountOfProductGivenSize - totalAmountOfProductGivenSizeInOrders < orderProduct.Amount)
                {   
                    ResultDTO<Product> result = new ResultDTO<Product>("ProductNotAvailable");
                    result.Data = product;
                    return result;
                }

            }

            return new ResultDTO<Product>();


        }

        public List<SingleOrderInfoDto> FilterOrderList(List<Order> orders, string status, string email, string fullname, DateTime endDate, DateTime startDate, string? id, int? UserID)
        {
            if (id != null)
            {
                orders = orders.Where(x => x.OrderID == id).ToList();
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
            if ((!IsNullOrWhiteSpace(endDate.ToString()) && !IsNullOrWhiteSpace(startDate.ToString())) && endDate >= startDate && endDate > DateTime.MinValue)
            {
                orders = orders.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).ToList();
            }

            List<SingleOrderInfoDto> orderList = new List<SingleOrderInfoDto>();
            foreach (var order in orders)
            {
                var closet = _closetManager.TGetList().FirstOrDefault(x => x.ClosetID == order.ClosetID);
                var airport = _airportManager.TGetList().FirstOrDefault(x => x.AirportID == order.AirportID);
                var orderProducts = _orderProductDal.GetList().Where(x => x.OrderId == order.Id).ToList();
                List<OrderProductDtoWithProductEntityDto> orderProductDtos = new List<OrderProductDtoWithProductEntityDto>();
                foreach (var orderProduct in orderProducts)
                {
                    var product = _productService.TGetByID(orderProduct.ProductId);
                    orderProductDtos.Add(new OrderProductDtoWithProductEntityDto
                    {
                        product = product,
                        Size = orderProduct.Size,
                        Amount = orderProduct.Amount
                    });
                }
                orderList.Add(new SingleOrderInfoDto(order, closet, airport, orderProductDtos));
            }


            return orderList;

        }

        public int TDelete(int id)
        {
            var order = _orderDal.GetByID(id);
            if (order == null)
            {
                return 0;
            }
            else
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
            if (a != null)
            {
                return 0;
            }
            else
            {
                _orderDal.Insert(t);
                return 1;
            }
        }
        public int TUpdate(Order t)
        {
            var isvalid = _orderDal.GetList().FirstOrDefault(x => x.Id == t.Id);
            if (isvalid == null)
            {
                return 0;
            }
            else
            {
                _orderDal.Update(t);
                return 1;
            }
        }

        public Closet getFirstClosetAvailable(int AirportID, DateTime StartDate, DateTime EndDate)
        {
            var closets = _closetManager.TGetList().Where(x => x.AirportID == AirportID).ToList();
            var orders = _orderDal.GetList().Where(x => x.AirportID == AirportID).ToList();

            foreach (var closet in closets)
            {
                // if closet is not occupied
                var isOccupied = orders.Any(x => x.ClosetID == closet.ClosetID && !(EndDate <= x.StartDate || StartDate >= x.EndDate));
                if (!isOccupied)
                {
                    return closet;
                }
            }
            return null;
        }



    }
}
