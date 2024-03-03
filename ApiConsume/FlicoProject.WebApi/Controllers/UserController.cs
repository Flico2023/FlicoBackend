using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.String;

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
        public IActionResult UserList([FromQuery] int pageSize, int PageIndex, string? email, string? name, string? surname, string? phone)
        {
            var users = _userService.TGetList();
            Console.WriteLine(name);
            if (!IsNullOrEmpty(name))
            {
                users = users.Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            }
            if(!IsNullOrEmpty(surname))
            {
                users = users.Where(x => x.Surname.ToLower() == surname.ToLower()).ToList();
            }
            if(!IsNullOrEmpty(email))
            {
                users = users.Where(x => x.Email.ToLower() == email.ToLower()).ToList();
            }
            if(!IsNullOrEmpty(phone))
            {
                users = users.Where(x => x.Phone.ToLower() == phone.ToLower()).ToList();
            }
            var totalCount = users.Count;
            users = users.Skip(pageSize * (PageIndex - 1)).Take(pageSize).ToList();

            var userListDTO = new UserListResultDto();
            userListDTO.Users = users;
            userListDTO.TotalCount = totalCount;
            userListDTO.PageIndex = PageIndex;
            userListDTO.PageSize = pageSize;

            return Ok(new ResultDTO<UserListResultDto>(userListDTO));


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

        [HttpPost("load")]
        public IActionResult LoadUser()
        {
           var loaders = new UserLoader();
            var users = loaders.Load();
            foreach (var user in users)
            {
                if(_userService.TInsert(user) == 0)
                {
                    return BadRequest(new ResultDTO<User>($"Error in {user.Name}"));
                }
            }
            return Ok(new ResultDTO<List<User>>(users));
        }
    }

    public class UserLoader
    {
        public List<User> Load()
        {
            var users = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                var user = new User();
                user.Name = "Name" + i;
                user.Surname = "Surname" + i;
                user.Email = "Email" + i;
                user.Password = "Password" + i;
                user.Phone = "Phone" + i;
                users.Add(user);
            }
            return users;
        }
    }
}

