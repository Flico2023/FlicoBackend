using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.DtoLayer.ProductDTOs;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NanoidDotNet;
using System.Drawing;
using System.Xml.Linq;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderProductService _OrderProductservice;
        private readonly IClosetService _closetService;
        private readonly IStockDetailService _stockDetailService;
        private readonly IAirportService _airportService;
        private readonly IMapper _mapper;


        public OrderController(IOrderService orderService, IOrderProductService OrderProductservice, IClosetService closetService, IStockDetailService stockDetailService, IAirportService airportService, IMapper mapper)
        {
            _orderService = orderService;
            _OrderProductservice = OrderProductservice;
            _mapper = mapper;
            _closetService = closetService;
            _stockDetailService = stockDetailService;
            _airportService = airportService;
        }


        [HttpGet]
        public IActionResult OrderList([FromQuery] int pageSize, int pageIndex, string? email, string? status, int? id, string? fullname, DateTime endDate, DateTime startDate, int? UserID)
        {
            var orders = _orderService.TGetList().ToList();

            List<OrderWithProductsDto> FilteredOrder = _orderService.FilterOrderList(orders, status, email, fullname,endDate, startDate,  id, UserID );


            var totalCount = FilteredOrder.Count;
           // FilteredOrder = FilteredOrder.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var OrderListPageDto = new OrderListPageDto();
            OrderListPageDto.Orders = FilteredOrder;
            OrderListPageDto.TotalCount = totalCount;
            OrderListPageDto.PageIndex = pageIndex;
            OrderListPageDto.PageSize = pageSize;

            return Ok(new ResultDTO<OrderListPageDto>(OrderListPageDto));
        }



        [HttpPost]
        public IActionResult AddOrder(OrderPostWithProductsDto OrderPostWithProductsDto)
        {   
            var result = _orderService.ValidatePostOrderDto(OrderPostWithProductsDto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<OrderDto>(result.Message));
            }

            
            var order = new Order();
            order = _mapper.Map<Order>(OrderPostWithProductsDto.Order);
            order.OrderID = Nanoid.Generate(size:8);
            var orderProducts = OrderPostWithProductsDto.OrderProducts.Select(x => _mapper.Map<OrderProduct>(x)).ToList();

            //to see if there is stock for those products or not
            if (orderProducts.Any())
            {
                //orderProducts.ForEach(x => _stockDetailService.TGetList().FirstOrDefault(y => y.ProductID == x.ProductId && y.VariationActiveAmount == x.Amount ));
                foreach(var product in orderProducts)
                {
                   var stockdetail = _stockDetailService.TGetList().FirstOrDefault(y => y.ProductID == product.ProductId && y.VariationActiveAmount >= product.Amount);
                   if(stockdetail != null)
                    {
                        product.Warehouses = stockdetail.WarehouseID;
                        stockdetail.VariationActiveAmount -= product.Amount;
                        if (_stockDetailService.TUpdate(stockdetail) == 0)
                            throw new("Stock is not updated");

                    }
                    else
                    {
                        throw new Exception("Stock is not insufficent.");
                    }
                }
            }

            //closet
            string status = "Empty";
            var closet = new Closet();
            closet = _closetService.TGetList().FirstOrDefault(x => x.Status == status && x.AirportID == order.AirportID);
            order.ClosetID = closet.ClosetID;
            closet.Status = "Taken";
            closet.OrderID = order.Id;
            _closetService.TUpdate(closet);

            //Insert order and order products
            if (_orderService.TInsert(order) == 0)
            {
                return BadRequest(new ResultDTO<Order>("Form values are not valid."));
            }

            if (orderProducts.Any())
            {
                orderProducts.ForEach(x => x.OrderId = order.Id);
                orderProducts.ForEach(x => _OrderProductservice.TInsert(x));
            }

            

            return Ok(new ResultDTO<Order>(order));

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var orderid = _orderService.TDelete(id);
            if (orderid == 0)
            {
                return BadRequest(new ResultDTO<Order>("The order to be deleted was not found."));
            }
            else
            {
                var order = _orderService.TGetByID(id);
                _orderService.TDelete(id);
                _OrderProductservice.DeleteOrderProducts(id);


                return Ok(new ResultDTO<Order>(order));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, Order order)
        {
            order.Id = id;
            int result = _orderService.TUpdate(order);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Order>("The order wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Order>(order));
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _orderService.TGetByID(id);
            if (order == null)
            {
                return BadRequest(new ResultDTO<Order>("The id to be looking for was not found."));
            }
            var orderProducts = _OrderProductservice.GetOrderProductsByOrderId(id);

            var orderwithProducts = _mapper.Map<OrderWithProductsDto>(order);

            orderwithProducts.AirportName = _airportService.TGetByID(order.AirportID).AirportName;
            orderwithProducts.ClosetNo = _closetService.TGetByID(order.ClosetID).ClosetNo;
            orderwithProducts.ClosetPassword = _closetService.TGetByID(order.ClosetID).Password;
            orderwithProducts.OrderProducts = orderProducts;
            

            return Ok(orderwithProducts);
        }
    }
}

