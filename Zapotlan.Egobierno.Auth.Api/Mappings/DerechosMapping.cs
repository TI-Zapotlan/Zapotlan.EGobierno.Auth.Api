using AutoMapper;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Api.Mappings
{
    public class DerechosMapping
    {
        private readonly IMapper _mapper;

        // CONSTRUCTOR

        public DerechosMapping(IMapper mapper)
        {
            _mapper = mapper;
        }

        // METHODS

        public IEnumerable<DerechoListDto> DerechoToListDto(IEnumerable<Derecho> items)
            {
            var itemsDto = new List<DerechoListDto>();
            
            foreach (var item in items)
            {
                var itemDto = _mapper.Map<DerechoListDto>(item);

                if (item.UsuarioActualizacion != null)
                {
                    itemDto.NombreUsuarioActualizacion = item.UsuarioActualizacion.Username ?? string.Empty;
                }

                if (item.Usuarios != null)
                {
                    itemDto.UsuariosCount = item.Usuarios.Count;
                }

                if (item.Grupos != null)
                {
                    itemDto.GruposCount = item.Grupos.Count;
                }

                itemsDto.Add(itemDto);
            }

            return itemsDto;
        }

        public DerechoDetailsDto DerechoToDetailDto(Derecho item)
        {
            var itemDto = _mapper.Map<DerechoDetailsDto>(item);

            if (item.UsuarioActualizacion != null)
            {
                itemDto.NombreUsuarioActualizacion = item.UsuarioActualizacion.Username ?? String.Empty;
            }

            if (item.Grupos != null)
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

            if (item.Usuarios != null)
            {
                itemDto.Usuarios = new List<UsuarioListDto>();

                foreach (var usuario in item.Usuarios)
                {
                    itemDto.Usuarios.Add(new UsuarioListDto
                    {
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
