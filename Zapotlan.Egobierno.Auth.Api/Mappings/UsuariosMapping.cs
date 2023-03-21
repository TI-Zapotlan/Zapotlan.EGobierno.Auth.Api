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

                if (item.Persona != null)
                {
                    itemDto.Nombres = item.Persona.Nombres;
                    itemDto.PrimerApellido = item.Persona.PrimerApellido;
                    itemDto.SegundoApellido = item.Persona.SegundoApellido;
                }

                if (item.Area != null)
                {
                    itemDto.NombreArea = item.Area.Nombre;
                }

                itemsDto.Add(itemDto);
            }

            return itemsDto;
        }
    }
}
