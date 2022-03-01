using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Validations
{
    public class ProductCategoryValidation : AbstractValidator<ProductCategory>
    {
        public ProductCategoryValidation()
        {
            RuleFor(pc => pc.Code).NotEmpty().MaximumLength(10);
            RuleFor(pc => pc.Name).NotEmpty().MaximumLength(50).MinimumLength(2);
            RuleFor(pc => pc.Description).MaximumLength(100);
        }
    }
}
