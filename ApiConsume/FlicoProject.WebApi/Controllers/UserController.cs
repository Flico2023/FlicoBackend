using AutoMapper;
using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.BusinessLayer.Concrete;
using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(IUserService userservice, IMapper mapper,SignInManager<AppUser> signInManager,UserManager<AppUser> userManager,RoleManager<AppRole> rolemanager)
        {
            _userService = userservice;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = rolemanager;
        }
        [HttpGet,Authorize(Roles ="Admin")]
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
                users = users.Where(x => x.PhoneNumber.ToLower() == phone.ToLower()).ToList();
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
        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUser userdto)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    UserName = userdto.Email,
                    Name = userdto.Name,
                    Surname =userdto.Surname,
                    Email =userdto.Email,
                    PhoneNumber =userdto.Phone
                };
                var result = await _userManager.CreateAsync(appUser, userdto.Password);
                var role = await _userManager.AddToRoleAsync(appUser, "NormalUser");
                if (result.Succeeded&& role.Succeeded)
                {
                    var user = _userService.TGetList().Find(x => x.Email == userdto.Email);
                    
                    return Created("Register Succesful",JwtManager.GenerateToken(user,"NormalUser"));
                }
                else
                {
                    return BadRequest(new ResultDTO<AppUser>("This Email is already used"));
                }
            }
            else { return BadRequest();
            }
            
            
        }
        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUser userdto)
        {
            var user = new AppUser();
            user = _userService.TGetList().Find(x=>x.Email == userdto.Mail);
            if (user == null)
            {
                return BadRequest(new ResultDTO<LoginUser>("Email is not exist"));
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(user, userdto.Password, false, true);
                var role = await _userManager.GetRolesAsync(user);
                if (result.Succeeded)
                {
                    return Ok(JwtManager.GenerateToken(user, role[0]));
                }
                else
                {
                    return BadRequest(new ResultDTO<LoginUser>("Password is incorrect"));
                }
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
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

                return Ok(new ResultDTO<AppUser>(user));
            }
        }
        [HttpPut("{id}"),Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(int id, AppUser user)
        {
            user.Id = id;
            int result = _userService.TUpdate(user);
            if (result == 0)
            {
                return BadRequest(new ResultDTO<AppUser>("The user wanted to update could not be updated."));
            }
            else
            {
                return Ok(new ResultDTO<AppUser>(user));
            }

        }
        [HttpGet("{id}"),Authorize(Roles = "Admin")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.TGetByID(id);
            if (user == null)
            {
                return BadRequest(new ResultDTO<AppUser>("The id to be looking for was not found."));
            }
            return Ok(new ResultDTO<AppUser>(user));
        }

        [HttpPost("load"), Authorize(Roles = "Admin")]
        public IActionResult LoadUser()
        {
           var loaders = new UserLoader();
            var users = loaders.Load();
            foreach (var user in users)
            {
                if(_userService.TInsert(user) == 0)
                {
                    return BadRequest(new ResultDTO<AppUser>($"Error in {user.Name}"));
                }
            }
            return Ok(new ResultDTO<List<AppUser>>(users));
        }
    }

    public class UserLoader
    {
        public List<AppUser> Load()
        {
            var users = new List<AppUser>();
            for (int i = 0; i < 100; i++)
            {
                var user = new AppUser();
                user.Name = "Name" + i;
                user.Surname = "Surname" + i;
                user.Email = "Email" + i;
                user.PasswordHash = "Password" + i;
                user.PhoneNumber = "Phone" + i;
                users.Add(user);
            }
            return users;
        }
    }
}

