using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;

namespace WMSApplication.Models.Validations
{
    public class UnitValidation :AbstractValidator<Unit>
    {
        public UnitValidation()
        {
            RuleFor(u => u.Code).NotEmpty();
            RuleFor(u => u.Name).NotEmpty().MaximumLength(10);
            RuleFor(u => u.Description).NotEmpty().MaximumLength(50);
        }
    }
}
