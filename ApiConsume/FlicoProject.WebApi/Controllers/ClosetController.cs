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

        public ClosetController(IClosetService closetService)
        {
            _closetService = closetService;
        }
        [HttpGet]
        public IActionResult ClosetList() 
        { 
            var closets = _closetService.TGetList();
            return Ok(new ResultDTO<List<Closet>>(closets));
        }
        [HttpPost]
        public IActionResult AddCloset(Closet closet)
        {
            if(_closetService.TInsert(closet) == 1)
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
        public IActionResult UpdateCloset(int id, Closet closet)
        {
            closet.ClosetID = id;
            int result = _closetService.TUpdate(closet);
            if(result == 0)
            {
                return BadRequest(new ResultDTO<Closet>("The closet wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<Closet>(closet));
            }
        }

    }
}
