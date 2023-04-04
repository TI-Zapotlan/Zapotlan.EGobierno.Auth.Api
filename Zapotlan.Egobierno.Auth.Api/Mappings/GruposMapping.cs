using AutoMapper;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Api.Mappings
{
    public class GruposMapping
    {
        private readonly IMapper _mapper;

        public GruposMapping(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<GrupoListDto> GrupoToListDto(IEnumerable<Grupo> items)
        { 
            var itemsDto = new List<GrupoListDto>();

            foreach (var item in items)
            {
                //var itemDto = new GrupoListDto();
                var itemDto = _mapper.Map<GrupoListDto>(item);

                if (item.UsuarioActualizacion != null)
                {
                    itemDto.NombreUsuarioActualizacion = item.UsuarioActualizacion.Username ?? string.Empty;
                }

                if (item.Usuarios != null)
                {
                    itemDto.UsuariosCount = item.Usuarios.Count;
                }

                if (item.Derechos != null)
                {
                    itemDto.DerechosCount = item.Derechos.Count;
                }

                itemsDto.Add(itemDto);
            }

            return itemsDto;
        }

        public GrupoDetailsDto GrupoToDetailDto(Grupo item)
        { 
            var itemDto = _mapper.Map<GrupoDetailsDto>(item);

            if (item.UsuarioActualizacion != null) {
                itemDto.NombreUsuarioActualizacion = item.UsuarioActualizacion.Username ?? String.Empty;
            }

            if (item.Derechos != null)
            {
                itemDto.Derechos = new List<DerechoListDto>();

                foreach (var derecho in item.Derechos)
                {
                    itemDto.Derechos.Add(new DerechoListDto { 
                        ID = derecho.DerechoID,
                        Nombre = derecho.Nombre,
                        Descripcion = derecho.Descripcion,
                        Acceso = derecho.Acceso
                    });
                }
            }

            if (item.Usuarios != null)
            {
                itemDto.Usuarios = new List<UsuarioListDto>();

                foreach (var usuario in item.Usuarios)
                {
                    itemDto.Usuarios.Add(new UsuarioListDto { 
                        ID = usuario.ID,
                        Username = usuario.Username,
                        Correo = usuario.Correo,
                        Puesto = usuario.Puesto,
                        Estatus = usuario.Estatus,
                        Rol = usuario.Rol
                    });
                }
            }

            return itemDto;
        }
    }
}
