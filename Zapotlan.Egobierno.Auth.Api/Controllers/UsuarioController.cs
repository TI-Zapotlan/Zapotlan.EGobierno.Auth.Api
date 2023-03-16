using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Zapotlan.EGobierno.Auth.Api.Responses;
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
        private readonly IUsuarioService _usuarioServices;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioServices = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        { 
            var items = await _usuarioServices.Gets();
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
            var response = new ApiResponse<IEnumerable<UsuarioDto>>(itemsDto);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _usuarioServices.Get(id);
            var itemDto = _mapper.Map<UsuarioDto>(item);
            var response = new ApiResponse<UsuarioDto>(itemDto);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UsuarioDto itemDto)
        {
            var item = _mapper.Map<Usuario>(itemDto);
            item.ID = Guid.NewGuid();
            item.FechaActualizacion = DateTime.Now;

            await _usuarioServices.Insert(item);

            itemDto = _mapper.Map<UsuarioDto>(item);
            var response = new ApiResponse<UsuarioDto>(itemDto);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UsuarioUpdateDto itemDto)
        {
            var item = _mapper.Map<Usuario>(itemDto);
            item.ID = id;
            item.FechaActualizacion = DateTime.Now;

            var result = await _usuarioServices.Update(item);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _usuarioServices.Delete(id);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}
