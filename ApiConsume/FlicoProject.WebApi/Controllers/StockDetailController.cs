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
    [Route("api/stockdetail")]
    [ApiController]
    public class StockDetailController : ControllerBase
    {

        private readonly IStockDetailService _stockDetailservice;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public StockDetailController(IStockDetailService stockDetailservice, IWebHostEnvironment environment, IMapper mapper)
        {
            _stockDetailservice = stockDetailservice;
            _environment = environment;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult StockDetailList()
        {
            var stockDetails = _stockDetailservice.TGetList();
            return Ok(new ResultDTO<List<StockDetail>>(stockDetails));
        }
        [HttpPost]
        public IActionResult AddStockDetail(ListStockDetails stockDetail)
        {
            // Veritabanı işlemi
            var a = new List<StockDetail>();
            foreach(var b in stockDetail.StockDetails)
            {
                a.Add(_mapper.Map<StockDetail>(b));
            }
            foreach (var item in a)
            {
                
                if (_stockDetailservice.TInsert(item) == 0)
                {
                    return BadRequest(new ResultDTO<StockDetail>("Form values are not valid."));
                }
            }
            return Created("", new ResultDTO<List<StockDetail>>(a));

        }
        /*[HttpPost]
        public IActionResult AddStockDetail(StockDetail stockDetail)
        {

            if (_stockDetailservice.TInsert(stockDetail) == 1)
            {
                return Created("", new ResultDTO<StockDetail>(stockDetail));
            }
            else
            {
                return BadRequest(new ResultDTO<StockDetail>("Form values are not valid."));
            }
        }*/
        [HttpDelete("{id}")]
        public IActionResult DeleteStockDetail(int id)
        {
            var stockDetailid = _stockDetailservice.TDelete(id);
            if (stockDetailid == 0)
            {
                return BadRequest(new ResultDTO<StockDetail>("The id to be deleted was not found."));
            }
            else
            {
                var stockDetail = _stockDetailservice.TGetByID(id);
                _stockDetailservice.TDelete(id);

                return Ok(new ResultDTO<StockDetail>(stockDetail));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStockDetail(int id, List<StockDetail> stockDetail)
        {

            
            //stockDetail.StockDetailID = id;
            foreach (var item in stockDetail)
            {

                if (_stockDetailservice.TUpdate(item) == 0)
                {
                    return BadRequest(new ResultDTO<StockDetail>("The stockDetail wanted to update could not be updated."));
                }
            }
            
                return Ok(new ResultDTO<List<StockDetail>>(stockDetail));
            

        }
        [HttpGet("{Productid}")]
        public IActionResult GetStockDetail(int Productid)
        {
            var stockDetail = _stockDetailservice.TGetList().FindAll(x => x.ProductID == Productid);
            if (stockDetail == null)
            {
                return BadRequest(new ResultDTO<StockDetail>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<List<StockDetail>>(stockDetail));
        }
    }
}
