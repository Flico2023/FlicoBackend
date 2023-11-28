using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlicoProject.WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userservice, IMapper mapper)
        {
            _userService = userservice;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult UserList()
        {
            var users = _userService.TGetList();
            return Ok(new ResultDTO<List<User>>(users));
        }
        [HttpPost]
        public IActionResult AddUser(UserDto userdto)
        {
            var user = new User();
            user = _mapper.Map<User>(userdto);
            if (_userService.TInsert(user) == 1)
            {
                return Created("", new ResultDTO<User>(user));
            }
            else
            {
                return BadRequest(new ResultDTO<User>("Form values are not valid."));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userid = _userService.TDelete(id);
            if (userid == 0)
            {
                return BadRequest(new ResultDTO<User>("The id to be deleted was not found."));
            }
            else
            {
                var user = _userService.TGetByID(id);
                _userService.TDelete(id);

                return Ok(new ResultDTO<User>(user));
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            user.UserID = id;
            int result = _userService.TUpdate(user);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<User>("The user wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<User>(user));
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.TGetByID(id);
            if (user == null)
            {
                return BadRequest(new ResultDTO<User>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<User>(user));
        }
    }
}

