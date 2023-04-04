using AutoMapper;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Api.Mappings
{
    public class UsuariosMapping
    {
        private readonly IMapper _mapper;

        public UsuariosMapping(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<UsuarioListDto> UsuarioToListDto(IEnumerable<Usuario> items)
        { 
            var itemsDto = new List<UsuarioListDto>();

            foreach(var item in items)
            {
                var itemDto = new UsuarioListDto();
                itemDto = _mapper.Map<UsuarioListDto>(item);

                if (item.Area != null)
                {
                    itemDto.NombreArea = item.Area.Nombre;
                }

                if (item.Empleado != null)
                {
                    itemDto.Codigo = item.Empleado.Codigo;
                }

                if (item.Persona != null)
                {
                    itemDto.NombreCompleto = string.Empty;
                    itemDto.NombreCompleto += !string.IsNullOrEmpty(item.Persona.Prefijo) ? item.Persona.Prefijo : string.Empty;
                    itemDto.NombreCompleto += !string.IsNullOrEmpty(itemDto.NombreCompleto) ? " " + item.Persona.Nombres : item.Persona.Nombres;
                    itemDto.NombreCompleto += !string.IsNullOrEmpty(item.Persona.PrimerApellido) ? " " + item.Persona.PrimerApellido : string.Empty;
                    itemDto.NombreCompleto += !string.IsNullOrEmpty(item.Persona.SegundoApellido) ? " " + item.Persona.SegundoApellido : string.Empty;
                }

                itemsDto.Add(itemDto);
            }

            return itemsDto;
        }

        public UsuarioDetailDto UsuarioToDetailDto(Usuario item)
        {
            var itemDto = _mapper.Map<UsuarioDetailDto>(item);

            if (item.Area != null)
            {
                itemDto.NombreArea = item.Area.Nombre;
            }

            if (item.Persona != null)
            {
                itemDto.Prefijo = item.Persona.Prefijo;
                itemDto.Nombres = item.Persona.Nombres;
                itemDto.PrimerApellido = item.Persona.PrimerApellido;
                itemDto.SegundoApellido = item.Persona.SegundoApellido;
            }

            if (item.Empleado != null)
            {
                itemDto.Codigo = item.Empleado.Codigo;
            }

            if (item.UsuarioActualizacion != null)
            {
                itemDto.NombreUsernameActualizacion = item.UsuarioActualizacion.Username;
            }

            if (item.Derechos != null)
            {
                itemDto.Derechos = new List<DerechoListDto>();

                foreach (var derecho in item.Derechos)
                {
                    itemDto.Derechos.Add(new DerechoListDto
                    {
                        ID = derecho.DerechoID,
                        Nombre = derecho.Nombre,
                        Descripcion = derecho.Descripcion,
                        Acceso = derecho.Acceso
                    });
                }
            }

            if (item.Grupos != null) // Esto pasarlo al GruposMapping cuando exista.
            {
                itemDto.Grupos = new List<GrupoListDto>();

                foreach (var grupo in item.Grupos)
                {
                    itemDto.Grupos.Add(new GrupoListDto
                    {
                        ID = grupo.ID,
                        Nombre = grupo.Nombre,
                        Descripcion = grupo.Descripcion
                    });
                }
            }

            return itemDto;
        }
    }
}
