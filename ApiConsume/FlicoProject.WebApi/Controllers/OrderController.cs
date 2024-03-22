using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;


        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult OrderList([FromQuery] int pageSize, int pageIndex, string? email, string? status, int? id, string? fullname, int? UserID)
        {
            var orders = _orderService.TGetList().ToList();

            List<Order> FilteredOrder = _orderService.FilterOrderList(orders, status, email, fullname,  id, UserID );


            var totalCount = FilteredOrder.Count;
            FilteredOrder = FilteredOrder.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var OrderListPageDto = new OrderListPageDto();
            OrderListPageDto.order = orders;
            OrderListPageDto.TotalCount = totalCount;
            OrderListPageDto.PageIndex = pageIndex;
            OrderListPageDto.PageSize = pageSize;

            return Ok(new ResultDTO<OrderListPageDto>(OrderListPageDto));
        }
        [HttpPost]
        public IActionResult AddOrder(OrderDto orderdto)
        {   
            var result = _orderService.ValidatePostOrderDto(orderdto);
            if (result.Success != true)
            {
                return BadRequest(new ResultDTO<OrderDto>(result.Message));
            }
            var order = new Order();
            order = _mapper.Map<Order>(orderdto);
            order.OrderID = Nanoid.Generate(size:8);

            if (_orderService.TInsert(order) == 1)
            {
                return Created("", new ResultDTO<Order>(order));
            }
            else
            {
                return BadRequest(new ResultDTO<Order>("Form values are not valid."));
            }
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
            return Ok(new ResultDTO<Order>(order));
        }
    }
}

