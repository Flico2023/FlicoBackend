using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/outsource")]
    [ApiController]
    public class OutsourceController : ControllerBase
    {
        private readonly IOutsourceService _outsourceService;
        private readonly IMapper _mapper;
        private readonly IOutsourceProductService _outsourceproductService;
        public OutsourceController(IOutsourceService outsourceService, IMapper mapper, IOutsourceProductService outsourceproductService)
        {
            _outsourceService = outsourceService;
            _mapper = mapper;
            _outsourceproductService = outsourceproductService;
        }
        [HttpGet]
        public IActionResult OutsourceList()
        {
            var outsources = _outsourceService.TGetList();
            return Ok(new ResultDTO<List<Outsource>>(outsources));
        }
        [HttpPost]
        public IActionResult AddOutsource(OutsourceDto outsourcedto)
        {
            var outsource = new Outsource();
            outsource = _mapper.Map<Outsource>(outsourcedto);
            if (_outsourceService.TInsert(outsource) == 1)
            {
                return Created("", new ResultDTO<Outsource>(outsource));
            }
            else
            {
                return BadRequest(new ResultDTO<Outsource>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteOutsource(int id)
        {
            var outsourceid = _outsourceService.TDelete(id);
            var outsourceProducts = _outsourceproductService.TGetList().FindAll(x => x.OutsourceID == id);
            if (outsourceid == 0)
            {
                return BadRequest(new ResultDTO<Outsource>("The id to be deleted was not found."));
            }
            else
            {
                var outsource = _outsourceService.TGetByID(id);
                _outsourceService.TDelete(id);
                foreach (var outsourceProduct in outsourceProducts)
                {
                    _outsourceproductService.TDelete(outsourceProduct.OutsourceProductID);
                }

                return Ok(new ResultDTO<Outsource>(outsource));
            }

        }
        [HttpPut("{id}")]
        public IActionResult UpdateOutsource(int id, OutsourceDto outsourcedto)
        {
            var outsource = new Outsource();
            outsource = _mapper.Map<Outsource>(outsourcedto);
            outsource.OutsourceId = id;
            int result = _outsourceService.TUpdate(outsource);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<Outsource>("The outsource wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Outsource>(outsource));
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetOutsource(int id)
        {
            var outsource = _outsourceService.TGetByID(id);
            if (outsource == null)
            {
                return BadRequest(new ResultDTO<Outsource>("The id to be looking for was not found."));
            }
            var getoutsource = new GetOutsourceDto();
            getoutsource = _mapper.Map<GetOutsourceDto>(outsource);
            var outsourceProducts = _outsourceproductService.TGetList().FindAll(x => x.OutsourceID == id);
            getoutsource.OutsourceProducts = outsourceProducts;
            return Ok(new ResultDTO<GetOutsourceDto>(getoutsource)) ;
        }
    }
}
