using FluentValidation;
using Zapotlan.EGobierno.Auth.Core.DTOs;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Validators
{
    public class UsuarioValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioValidator() 
        {
            RuleFor(i => i.Username)
                .MaximumLength(25);

            //RuleFor(i => i.Estatus)
            //    .InclusiveBetween(0, 3);
        }
    }
}
