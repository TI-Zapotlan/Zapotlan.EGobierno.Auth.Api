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

    public class UsuarioPostValidator : AbstractValidator<UsuarioInsertDto>
    {
        public UsuarioPostValidator()
        {
            RuleFor(i => i.UsuarioActualizacionID)
                .NotEmpty().WithMessage("Faltó especificar el identificador del usuario que ejecuta la aplicación");
        }
    }

    public class UsuarioPutValidator : AbstractValidator<UsuarioUpdateDto>
    {
        public UsuarioPutValidator()
        {
            RuleFor(i => i.UsuarioActualizacionID)
                .NotEmpty().WithMessage("Faltó especificar el identificador del usuario que ejecuta la aplicación");

            RuleFor(i => i.PersonaID)
                .NotEmpty().WithMessage("Faltó especificar a la Persona");

            RuleFor(i => i.Username)
                .NotEmpty().WithMessage("Faltó especificar el nombre usuario")
                .MaximumLength(25).WithMessage("El nombre de usuario no puede ser mayor a {MaxLength} carácteres");

            RuleFor(i => i.Correo)
                .EmailAddress().WithMessage("El correo electrónico tiene un formato no valido")
                .MaximumLength(255).WithMessage("El correo electrónico no puede ser mayor a {MaxLength} carácteres");

            RuleFor(i => i.Puesto)
                .MaximumLength(150).WithMessage("El puesto no puede ser mayor a {MaxLength} carácteres");

            RuleFor(i => i.Estatus)
                .NotEmpty().WithMessage("Faltó especificar el Estatus del usuario")
                .IsInEnum().WithMessage("El valor {PropertyValue} para {PropertyName} no es un valor válido");

            RuleFor(i => i.Rol)
                .NotEmpty().WithMessage("Faltó especificar el Rol del usuario")
                .IsInEnum().WithMessage("El valor {PropertyValue} para {PropertyName} no es un valor válido");
            
            RuleFor(i => i.ArchivoCartaResponsabilidad)
                .MaximumLength(255).WithMessage("El nombre del archivo de la carta de responsabilidad no puede ser mayor a {MaxLength} carácteres");

            RuleFor(i => i.UsuarioActualizacionID)
                .NotEmpty().WithMessage("Faltó especificar el identificador del usuario que ejecuta la aplicación");
        }
    }

    public class UsuarioLoginValidator : AbstractValidator<UsuarioLoginDto>
    {
        public UsuarioLoginValidator()
        {
            RuleFor(i => i.Username)
                .NotEmpty().WithMessage("Faltó especificar el nombre usuario")
                .MaximumLength(25).WithMessage("El nombre de usuario no puede ser mayor a {MaxLength} carácteres");

            RuleFor(i => i.Password)
                .NotEmpty().WithMessage("Faltó especificar la contraseña");

            RuleFor(i => i.DerechosInicio)
                .GreaterThan(0).WithMessage("Los derechos no pueden ser menor a cero o negativo");

            RuleFor(i => i.DerechosTermino)
                .GreaterThanOrEqualTo(i => i.DerechosInicio).WithMessage("El termino de los derechos solicitados no puede ser menor al inicio.");
        }
    }
}
