using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/outsourceproduct")]
    [ApiController]
    public class OutsourceProductController : ControllerBase
    {

        private readonly IOutsourceProductService _outsourceProductService;
        private readonly IMapper _mapper;
        private readonly IOutsourceService _outsourceService;
        private readonly IAirportService _airportService;

        public OutsourceProductController(IOutsourceProductService outsourceProductService, IMapper mapper, IOutsourceService outsourceService, IAirportService airportService)
        {
            _outsourceProductService = outsourceProductService;
            _mapper = mapper;
            _outsourceService = outsourceService;
            _airportService = airportService;
        }
        [HttpGet]
        public IActionResult OutsourceProductList()
        {
            var outsourceProduct = _outsourceProductService.TGetList();
            return Ok(new ResultDTO<List<OutsourceProduct>>(outsourceProduct));
        }
        [HttpPost]
        public IActionResult AddOutsourceProduct(OutsourceProductDto outsourceProductdto)
        {
            var outsourceProduct = new OutsourceProduct();
            outsourceProduct = _mapper.Map<OutsourceProduct>(outsourceProductdto);
            var outsource = _outsourceService.TGetByID(outsourceProduct.OutsourceID);
            if (outsource == null)
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("Enter a valid OutsourceID."));
            }
            var airport = _airportService.TGetByID(outsourceProduct.AirportID);
            if (airport == null)
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("Enter a valid AirportID."));
            }

            if (_outsourceProductService.TInsert(outsourceProduct) == 1)
            {
                return Created("", new ResultDTO<OutsourceProduct>(outsourceProduct));
            }
            else
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteOutsourceProduct(int id)
        {
            var outsourceProductid = _outsourceProductService.TDelete(id);
            if (outsourceProductid == 0)
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("The id to be deleted was not found."));
            }
            else
            {
                var outsourceProduct = _outsourceProductService.TGetByID(id);
                _outsourceProductService.TDelete(id);

                return Ok(new ResultDTO<OutsourceProduct>(outsourceProduct));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateOutsourceProduct(int id, OutsourceProductDto outsourceProductdto)
        {
            var outsourceProduct = new OutsourceProduct();
            outsourceProduct = _mapper.Map<OutsourceProduct>(outsourceProductdto);
            outsourceProduct.OutsourceProductID = id;
            var outsource = _outsourceService.TGetByID(outsourceProduct.OutsourceID);
            if (outsource == null)
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("Enter a valid OutsourceID."));
            }
            var airport = _airportService.TGetByID(outsourceProduct.AirportID);
            if (airport == null)
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("Enter a valid AirportID."));
            }

            int result = _outsourceProductService.TUpdate(outsourceProduct);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("The outsource product wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<OutsourceProduct>(outsourceProduct));
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetOutsourceProduct(int id)
        {
            var outsourceProduct = _outsourceProductService.TGetByID(id);
            if (outsourceProduct == null)
            {
                return BadRequest(new ResultDTO<OutsourceProduct>("The id to be looking for was not found."));
            }
            var outsourceProductdto = new OutsourceProductDto();
            outsourceProductdto = _mapper.Map<OutsourceProductDto>(outsourceProduct);
            return Ok(new ResultDTO<OutsourceProductDto>(outsourceProductdto));
        }
    }
}
