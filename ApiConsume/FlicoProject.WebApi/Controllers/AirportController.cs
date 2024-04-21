using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/airport")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        
        private readonly IAirportService _airportservice;
        private readonly IMapper _mapper;

        public AirportController(IAirportService airportservice, IMapper mapper)
        {
            _airportservice = airportservice;
            _mapper = mapper;
        }
        [HttpGet,Authorize(Roles ="Admin")]
        public IActionResult AirportList()
        {
            var airports = _airportservice.TGetList();
            return Ok(new ResultDTO<List<Airport>>(airports));
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public IActionResult AddAirport(AirportDto airportdto)
        {
            var airport = new Airport();
            airport = _mapper.Map<Airport>(airportdto);

            if (_airportservice.TInsert(airport) == 1)
            {
                return Created("", new ResultDTO<Airport>(airport));
            }
            else
            {
                return BadRequest(new ResultDTO<Airport>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult DeleteAirport(int id)
        {
            var airportid = _airportservice.TDelete(id);
            if (airportid == 0)
            {
                return BadRequest(new ResultDTO<Airport>("The id to be deleted was not found."));
            }
            else
            {
                var airport = _airportservice.TGetByID(id);
                _airportservice.TDelete(id);

                return Ok(new ResultDTO<Airport>(airport));
            }
        }
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public IActionResult UpdateAirport(int id,Airport airport)
        {
            airport.AirportID = id;
            int result = _airportservice.TUpdate(airport);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Airport>("The airport wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Airport>(airport));
            }

        }
        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public IActionResult GetAirport(int id)
        {
            var airport = _airportservice.TGetByID(id);
            if (airport == null)
            {
                return BadRequest(new ResultDTO<Airport>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<Airport>(airport));
        }
    }
}
