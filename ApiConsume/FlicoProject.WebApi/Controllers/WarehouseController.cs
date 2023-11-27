using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.String;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseservice;
        private readonly IStockDetailService _stockDetailservice;
        private readonly IMapper _mapper;

        public WarehouseController(IWarehouseService warehouseservice, IStockDetailService stockDetailservice, IMapper mapper)
        {
            _warehouseservice = warehouseservice;
            _stockDetailservice = stockDetailservice;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult WarehouseList()
        {
            var warehouses = _warehouseservice.TGetList();
            return Ok(new ResultDTO<List<Warehouse>>(warehouses));
        }
        [HttpPost]
        public IActionResult AddWarehouse(WarehouseDto warehousedto)
        {
            var warehouse = new Warehouse();
            warehouse = _mapper.Map<Warehouse>(warehousedto);
            if (_warehouseservice.TInsert(warehouse) == 1)
            {
                return Created("", new ResultDTO<Warehouse>(warehouse));
            }
            else {
                return BadRequest(new ResultDTO<Warehouse>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteWarehouse(int id)
        {
            var warehouseid = _warehouseservice.TDelete(id);
            var details = _stockDetailservice.TGetList().FindAll(x => x.WarehouseID == id);
            if (details.Count > 0)
            {
                return BadRequest(new ResultDTO<Warehouse>("The deletion failed because there are products in the warehouse you want to delete."));
            }
            if (warehouseid == 0)
            {
                return BadRequest(new ResultDTO<Warehouse>("The id to be deleted was not found."));
            }
            else
            {
                var warehouse = _warehouseservice.TGetByID(id);
                _warehouseservice.TDelete(id);
                
                return Ok(new ResultDTO<Warehouse>(warehouse));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateWarehouse(int id,Warehouse warehouse)
        {
            warehouse.WarehouseID = id;
            int result = _warehouseservice.TUpdate(warehouse);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Warehouse>("The warehouse wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Warehouse>(warehouse));
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetWarehouse(int id)
        {
            var warehouse = _warehouseservice.TGetByID(id);
            if (warehouse == null)
            {
                return BadRequest(new ResultDTO<Warehouse>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<Warehouse>(warehouse));
        }
    }
}
