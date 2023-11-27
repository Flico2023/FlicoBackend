using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/closet")]
    [ApiController]
    public class ClosetController : ControllerBase
    {
        private readonly IClosetService _closetService;
        private readonly IMapper _mapper;
        private readonly IAirportService _airportService;

        public ClosetController(IClosetService closetService, IMapper mapper, IAirportService airportService)
        {
            _closetService = closetService;
            _mapper = mapper;
            _airportService = airportService;
        }
        [HttpGet]
        public IActionResult ClosetList() 
        { 
            var closets = _closetService.TGetList();
            var closetWithAirport = new List<ClosetWithAirportDto>();
            int i = 0;
            foreach (var closet in closets)
            {
                var airport = _airportService.TGetByID(closet.AirportID);
                var addlist = new ClosetWithAirportDto();
                addlist = _mapper.Map<ClosetWithAirportDto>(closet);
                closetWithAirport.Add(addlist);
                closetWithAirport[i].AirportName = airport.AirportName;
                i++;
            };
            return Ok(new ResultDTO<List<ClosetWithAirportDto>>(closetWithAirport));
        }
        [HttpPost]
        public IActionResult AddCloset(ClosetDto closetdto)
        {
            Closet closet = _mapper.Map<Closet>(closetdto);
            Airport airport = _airportService.TGetByID(closet.AirportID);
            if (airport == null)
            {
                return BadRequest(new ResultDTO<Closet>("Enter a valid airport id."));
            }
            closet.Airport = airport;
            if (_closetService.TInsert(closet) == 1)
            {
                return Created("", new ResultDTO<Closet>(closet));
            }
            else
            {
                return BadRequest(new ResultDTO<Closet>("Form values are not valid"));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCloset(int id)
        {
            var closetid = _closetService.TDelete(id);
            if(closetid == 0)
            {
                return BadRequest(new ResultDTO<Closet>("The id to be deleted is not found."));
            }
            else
            {
                var closet = _closetService.TGetByID(id);
                _closetService.TDelete(id);

                return Ok(new ResultDTO<Closet>(closet));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCloset(int id, ClosetDto closetDto)
        {
            var closet = new Closet();
            closet = _mapper.Map<Closet>(closetDto);
            closet.ClosetID = id;
            var airport = _airportService.TGetByID(closet.AirportID);
            if (airport == null)
            {
                return BadRequest(new ResultDTO<Closet>("Enter valid an airport id."));
            }
            var closetWithAirport = new ClosetWithAirportDto();
            closetWithAirport = _mapper.Map<ClosetWithAirportDto>(closet);
            closetWithAirport.AirportName = airport.AirportName;
            closet.Airport = airport;
            int result = _closetService.TUpdate(closet);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Closet>("The closet wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Closet>(closet));
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetCloset(int id)
        {
            var closet = _closetService.TGetByID(id);
            if (closet == null)
            {
                return BadRequest(new ResultDTO<Closet>("The id to be looking for was not found."));
            } else
            {
                var airport = _airportService.TGetByID(closet.AirportID);
                var closetWithAirport = new ClosetWithAirportDto();
                closetWithAirport = _mapper.Map<ClosetWithAirportDto>(closet);
                closetWithAirport.AirportName = airport.AirportName;
                return Ok(new ResultDTO<ClosetWithAirportDto>(closetWithAirport));
            }
          
        }

    }
}
