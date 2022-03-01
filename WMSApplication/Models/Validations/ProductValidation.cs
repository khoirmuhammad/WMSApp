using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
            RuleFor(p => p.WholeUnit).NotEmpty().MaximumLength(10);
            RuleFor(p => p.LooseQty).NotNull().GreaterThan(0);
            RuleFor(p => p.WholeQty).NotNull().GreaterThan(0);
            RuleFor(p => p.AllocationType).NotEmpty().MaximumLength(10);
            RuleFor(p => p.LoosePrice).NotEmpty();
            RuleFor(p => p.WholePrice).NotEmpty();
            RuleFor(p => p.UnitCode).NotNull().NotEmpty();
        }
    }
}
