using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/OrderProduct")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {

        private readonly IOrderProductService _OrderProductservice;
        private readonly IMapper _mapper;

        public OrderProductController(IOrderProductService OrderProductservice, IMapper mapper)
        {
            _OrderProductservice = OrderProductservice;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult OrderProductList()
        {
            var OrderProducts = _OrderProductservice.TGetList();
            return Ok(new ResultDTO<List<OrderProduct>>(OrderProducts));
        }
        /*
        [HttpPost]
        public IActionResult AddOrderProduct()
        {
            return Ok();

        }*/
        [HttpDelete("{id}")]
        public IActionResult DeleteOrderProduct(int id)
        {
            var OrderProductid = _OrderProductservice.TDelete(id);
            if (OrderProductid == 0)
            {
                return BadRequest(new ResultDTO<OrderProduct>("The id to be deleted was not found."));
            }
            else
            {
                var OrderProduct = _OrderProductservice.TGetByID(id);
                _OrderProductservice.TDelete(id);

                return Ok(new ResultDTO<OrderProduct>(OrderProduct));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateOrderProduct(int id, List<OrderProduct> OrderProduct)
        {


            foreach (var item in OrderProduct)
            {

                if (_OrderProductservice.TUpdate(item) == 0)
                {
                    return BadRequest(new ResultDTO<OrderProduct>("The OrderProduct wanted to update could not be updated."));
                }
            }

            return Ok(new ResultDTO<List<OrderProduct>>(OrderProduct));


        }
        [HttpGet("{Orderid}")]
        public IActionResult GetOrderProduct(int Orderid)
        {
            var OrderProduct = _OrderProductservice.TGetList().FindAll(x => x.OrderId == Orderid);
            if (OrderProduct == null)
            {
                return BadRequest(new ResultDTO<OrderProduct>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<List<OrderProduct>>(OrderProduct));
        }
    }
}
