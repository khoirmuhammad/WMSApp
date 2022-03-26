using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Validations
{
    public class LocationValidation : AbstractValidator<Location>
    {
        public LocationValidation()
        {
            RuleFor(pc => pc.Floor).NotNull();
            RuleFor(pc => pc.RackAisle).NotEmpty().MaximumLength(2);
            RuleFor(pc => pc.Level).NotNull();
            RuleFor(pc => pc.Pos).NotNull();
            RuleFor(pc => pc.Type).NotEmpty().MaximumLength(15);
            RuleFor(pc => pc.MaximumPallet).NotNull();
        }
    }
}
