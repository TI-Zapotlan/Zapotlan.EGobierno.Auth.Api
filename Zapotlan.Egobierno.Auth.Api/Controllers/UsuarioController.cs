using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Infrastructure.Repositories;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        { 
            var items = await _usuarioRepository.Gets();
            //var itemsDto = items.Select(i => new UsuarioDetailDto
            //{ 
            //    ID = i.ID,
            //    PersonaID = i.PersonaID,
            //    AreaID = i.AreaID,
            //    EmpleadoID = i.EmpleadoID,
            //    UsuarioJefeID = i.UsuarioJefeID,
            //    Username = i.Username,
            //    Password = i.Password,
            //    Correo = i.Correo,
            //    Puesto = i.Puesto,
            //    Estatus = i.Estatus,
            //    Rol = i.Rol,
            //    FechaAlta = i.FechaAlta,
            //    FechaVigencia = i.FechaVigencia,
            //    ArchivoCartaResponsabilidad = i.ArchivoCartaResponsabilidad,
            //    NombreUsernameActualizacion = i.UsuarioActualizacion?.Username ?? "",
            //    FechaActualizacion = i.FechaActualizacion,
            //});
            var itemsDto = _mapper.Map<IEnumerable<UsuarioDto>>(items);

            return Ok(itemsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _usuarioRepository.Get(id);
            var itemDto = _mapper.Map<UsuarioDto>(item);

            return Ok(itemDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UsuarioDto itemDto)
        {
            var item = _mapper.Map<Usuario>(itemDto);
            item.ID = Guid.NewGuid();
            item.FechaActualizacion = DateTime.Now;

            await _usuarioRepository.Insert(item);
            return Ok(item);
        }


    }
}
