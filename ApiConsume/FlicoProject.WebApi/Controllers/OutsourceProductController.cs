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

        public OutsourceProductController(IOutsourceProductService outsourceProductService)
        {
            _outsourceProductService = outsourceProductService;
        }
        [HttpGet]
        public IActionResult OutsourceProductList()
        {
            var outsourceProduct = _outsourceProductService.TGetList();
            return Ok(new ResultDTO<List<OutsourceProduct>>(outsourceProduct));
        }
        [HttpPost]
        public IActionResult AddOutsourceProduct(OutsourceProduct outsourceProduct)
        {

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
        public IActionResult UpdateOutsourceProduct(int id, OutsourceProduct outsourceProduct)
        {
            outsourceProduct.OutsourceProductID = id;
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
            return Ok(new ResultDTO<OutsourceProduct>(outsourceProduct));
        }
    }
}
