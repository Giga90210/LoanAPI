﻿using Application.Services;
using Application.Validators;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Infrastructure.Helpers;
using Infrastructure.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanAPI.Controllers
{

    [Authorize]
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
            if (User.IsInRole(Role.Accountant))
            {
                // var user = _userService.GetUserById(id);
                return user == null ? NotFound("User not found, please enter valid Id") : Ok(user);
                //if (user == null)
                //{
                //    return NotFound("User not found, please enter valid Id");
                //}
                //return Ok(user);
            }
            else
            {
                //var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                //if(currentUserId != id) 
                //{
                //    return Forbid("You can't access this data");
                //}
                ////var user = _userService.GetUserById(id);
                //return Ok(user);


                int currentUserId;

                if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out currentUserId))
                {
                    //if (currentUserId != id)
                    //{
                    //    return Forbid("You can't access this data");
                    //}
                    //return Ok(user);
                    return currentUserId != id ? Forbid() : Ok(user);
                }
                else
                {
                    return BadRequest("Invalid user ID");
                }
            }


        }

        [Authorize(Roles = Role.Accountant)]
        [HttpGet("all")]
        public IActionResult GetUsers() 
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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


        [Authorize(Roles = Role.User)]
        [HttpGet("loans")]
        public IActionResult GetMyLoans() 
        {
            int userId;
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId)) 
            {
                var loans = _userService.GetMyLoans(userId);
                return loans == null ? NotFound("Invalid user Id or user doesn't have loans") : Ok(loans);
            }
            else
            {
                return Unauthorized("User ID is invalid or missing");
            }
        }

    }
}
