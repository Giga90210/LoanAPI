﻿using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class UserLoginValidator : AbstractValidator<User>
    {
        public UserLoginValidator()
        {
            RuleFor(m => m.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid Email");
            RuleFor(m => m.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
