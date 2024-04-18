using Application.Services;
using Application.Validators;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Helpers;
using Infrastructure.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace LoanAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private UserLoginValidator _loginValidator;
        private UserValidator _validator;
        private readonly ITokenGenerator<User> _tokenGenerator;

        public UserController(IUserService userService, UserValidator validator, UserLoginValidator loginValidator, ITokenGenerator<User> tokenGenerator)
        {
            _userService = userService;
            _validator = validator;
            _loginValidator = loginValidator;
            _tokenGenerator = tokenGenerator;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id) 
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("all")]
        public IActionResult GetUsers() 
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost("login")]
        public IActionResult login([FromBody]User loginModel)
        {
            var result = _loginValidator.Validate(loginModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }
            var user = _userService.Login(loginModel);
            if (user == null)
            {
                return NotFound("User not found, Please try again");
            }
            var tokenString = _tokenGenerator.GenerateToken(user);
            return Ok(
                new
                {
                    user.FirstName,
                    user.LastName,
                    user.UserName,
                    Token = tokenString
                });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User registerModel)
        {
            var result = _validator.Validate(registerModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }
            var user = _userService.Register(registerModel);
            var tokenString = _tokenGenerator.GenerateToken(user);
            var locationURI = Url.Action("GetUserById", new { id = user.Id });
            return Created(
                locationURI,
                new
                {
                    user.FirstName,
                    user.LastName,
                    user.UserName,
                    Token = tokenString
                });
        }


    }
}
