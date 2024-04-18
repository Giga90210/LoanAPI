using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AccountantValidator : AbstractValidator<Accountant>
    {
        public AccountantValidator()
        {
            var passwordRegex = new Regex(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
                RegexOptions.Compiled);

            RuleFor(m => m.FirstName).NotEmpty().WithMessage("The First Name field should not be empty").Length(0, 50).WithMessage("Please put 0-50 characters").Matches("^[a-zA-Z]+$").WithMessage("First name can only be letters");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("The Last Name field should not be empty").Length(0, 50).WithMessage("Please put 0-50 characters").Matches("^[a-zA-Z]+$").WithMessage("Last name can only be letters");
            RuleFor(m => m.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid Email");
            RuleFor(m => m.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches(passwordRegex).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character");

        }
    }
}
