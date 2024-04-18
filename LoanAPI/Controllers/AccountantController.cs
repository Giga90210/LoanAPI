using Application.Services;
using Application.Validators;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanAPI.Controllers
{
    [Authorize(Roles = Role.Accountant)]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountantController : Controller
    {
        private readonly IAccountantService _accountantService;
        private AccountantLoginValidator _loginValidator;
        private AccountantValidator _validator;
        private readonly ITokenGenerator<Accountant> _tokenGenerator;

        public AccountantController(IAccountantService accountantService, AccountantValidator validator, AccountantLoginValidator loginValidator, ITokenGenerator<Accountant> tokenGenerator)
        {
            _accountantService = accountantService;
            _validator = validator;
            _loginValidator = loginValidator;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPatch("{userId}/block")]
        public IActionResult BlockUser(int userId) 
        {
            var user = _accountantService.BlockUser(userId);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpPatch("{userId}/unblock")]
        public IActionResult UnblockUser(int userId)
        {
            var user = _accountantService.UnblockUser(userId);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]Accountant loginModel)
        {
            var result = _loginValidator.Validate(loginModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }
            var accountant = _accountantService.Login(loginModel);
            if (accountant == null)
            {
                return Unauthorized("Invalid Email or Password");
            }
            var tokenString = _tokenGenerator.GenerateToken(accountant);
            return Ok(
                new
                {
                    accountant.FirstName,
                    accountant.LastName,
                    Token = tokenString
                });

        }


        [HttpPost("register")]
        public IActionResult Register([FromBody]Accountant registerModel)
        {
            var result = _validator.Validate(registerModel);
            if(!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }
            var accountant = _accountantService.Register(registerModel);
            var tokenString = _tokenGenerator.GenerateToken(accountant);
            var locationURI = Url.Action("GetAccountantById", new { id = accountant.Id });
            return Created(
                locationURI,
                new
                {
                    accountant.FirstName,
                    accountant.LastName,
                    Token = tokenString
                });
        }
        [HttpGet("{id}")]
        public IActionResult GetAccountantById(int id)
        {
            var accountant = _accountantService.GetAccountantById(id);
            return accountant == null ? NotFound("Accountant not found") : Ok(accountant);
        }
    }
}
