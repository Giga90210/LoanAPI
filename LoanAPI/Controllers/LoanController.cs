using Application.Services;
using Application.Validators;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace LoanAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private LoanValidator _loanValidator;
        public LoanController(ILoanService loanService, LoanValidator loanValidator)
        {
            _loanService = loanService;
            _loanValidator = loanValidator;
        }

        [Authorize(Roles = Role.User)]
        [HttpPost]
        public IActionResult AddLoan([FromBody]Loan loan)
        {
            var result = _loanValidator.Validate(loan);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }
            loan.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var addedLoan = _loanService.AddLoan(loan);
            var locationURI = Url.Action("GetLoan", new { id = loan.Id });
            return Created(
                locationURI,
                new
                {
                    LoanType = addedLoan.LoanType.ToString(),
                    addedLoan.LoanAmount,
                    Currency = addedLoan.Currency.ToString(),
                    addedLoan.LoanPeriod,
                    LoanStatus = addedLoan.Status.ToString()
                }
                );
        }

        [HttpGet("{id}")]
        public IActionResult GetLoan(int id)
        {
            var loan = _loanService.GetLoan(id);
            if(loan == null)
            {
                return NotFound("Loan not found");
            }
            if (User.IsInRole(Role.User))
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return userId == loan.UserId ? Ok(loan) : Forbid();
            }
            else
            {
                return Ok(loan);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLoan([FromBody]Loan loan, int id)
        {
            var result = _loanValidator.Validate(loan);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }


            //var updatedLoan = _loanService.UpdateLoan(loan, id);
            //return updatedLoan == null ? NotFound("Loan not found") : Ok(updatedLoan);

            if(User.IsInRole(Role.User))
            {
                if(_loanService.GetLoan(id).UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                {
                    return Forbid();
                }
                loan.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var updatedLoan = _loanService.UpdateLoan(loan, id);
                return updatedLoan == null ? NotFound("Loan not found") : Ok(updatedLoan);
            }
            else
            {
                var updatedLoan = _loanService.UpdateLoan(loan, id);
                return updatedLoan == null ? NotFound("Loan not found") : Ok(updatedLoan);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLoan(int id)
        {
            //var loan = _loanService.DeleteLoan(id);
            //return loan == null ? NotFound("Loan not found") : Ok(loan);


            var loan = _loanService.DeleteLoan(id);
            if (loan == null)
            {
                return NotFound("Loan not found");
            }
            if (User.IsInRole(Role.User))
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return userId == loan.UserId ? Ok(loan) : Forbid();
            }
            else
            {
                return Ok(loan);
            }
        }



        [Authorize(Roles = Role.Accountant)]
        [HttpPatch("{id}/approve")]
        public IActionResult ApproveLoan(int id)
        {
            var loan = _loanService.ApporveLoan(id);
            return loan == null ? NotFound("Loan not found") : Ok(loan);
        }

        [Authorize(Roles = Role.Accountant)]
        [HttpPatch("{id}/reject")]
        public IActionResult RejectLoan(int id)
        {
            var loan = _loanService.RejectLoan(id);
            return loan == null ? NotFound("Loan not found") : Ok(loan);
        }
    }
}
