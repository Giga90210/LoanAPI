using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class LoanValidator : AbstractValidator<Loan>
    {
        public LoanValidator()
        {
            RuleFor(m => m.LoanType).IsInEnum().WithMessage("Invalid Loan Type (must be 0, 1 or 2)");
            RuleFor(m => m.LoanAmount).InclusiveBetween(100, 10000).WithMessage("Loan Amount must be between 100-10000");
            RuleFor(m => m.Currency).IsInEnum().WithMessage("Invalid Currency (must be 0, 1 or 2)");
            RuleFor(m => m.LoanPeriod).InclusiveBetween(0, 60).WithMessage("Loan Period can't be longer than 5 years");

        }
    }
}
