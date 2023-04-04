using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Zapotlan.EGobierno.Auth.Api.Mappings;
using Zapotlan.EGobierno.Auth.Api.Responses;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;
using Zapotlan.EGobierno.Auth.Infrastructure.Interfaces;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServices;
        private readonly IMapper _mapper;
        private readonly UsuariosMapping _usuariosMapping;
        private readonly IUriService _uriService;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper, IUriService uriService)
        {
            _usuarioServices = usuarioService;
            _mapper = mapper;
            _uriService = uriService;

            _usuariosMapping = new UsuariosMapping(mapper);
        }

        /// <summary>
        /// Devuelve un listado de usuarios de acuerdo a los filtros establecidos
        /// </summary>
        /// <param name="filters">Listado de filtros disponibles</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet(Name = nameof(GetUsuarios))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioListDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetUsuarios([FromQuery]UsuarioQueryFilter filters)
        { 
            var items = _usuarioServices.Gets(filters);
            var itemsDto = _usuariosMapping.UsuarioToListDto(items); // _mapper.Map<IEnumerable<UsuarioDto>>(items);
            var metadata = new Metadata
            {
                TotalCount = items.TotalCount,
                PageSize = items.PageSize,
                CurrentPage = items.CurrentPage,
                TotalPages = items.TotalPages,
                HasNextPage = items.HasNextPage,
                HasPreviousPage = items.HasPreviousPage,
                NextPageUrl = _uriService.GetUsuarioPaginationUri(
                    filters,
                    Url.RouteUrl(nameof(GetUsuarios))
                    //, items.NextPageNumber
                ).ToString(),
                PreviousPageUrl = _uriService.GetUsuarioPaginationUri(
                    filters,
                    Url.RouteUrl(nameof(GetUsuarios))
                ).ToString()
            };

            var response = new ApiResponse<IEnumerable<UsuarioListDto>>(itemsDto) {
                Meta = metadata
            };

            // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        /// <summary>
        /// Obtiene un registro detallado de un Usuario de acuerdo al identificador recibido.
        /// </summary>
        /// <param name="id">Identificadorl de usuario a devolver</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioDetailDto>))]
        public async Task<IActionResult> GetUsuario(Guid id)
        {
            var item = await _usuarioServices.GetAsync(id);

            if (item == null)
            {   
                throw new BusinessException($"No se encontró un elemento con el Id: {id}");
            }
            var itemDto = _usuariosMapping.UsuarioToDetailDto(item);
            var response = new ApiResponse<UsuarioDetailDto>(itemDto);

            return Ok(response);
        }

        /// <summary>
        /// Genera un nuevo registro de un usuario con los campos vacios y el
        /// identificador del usuario
        /// </summary>
        /// <param name="itemDto"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostUsuario(UsuarioInsertDto itemDto)
        {   
            var item = new Usuario
            {
                ID = Guid.NewGuid(),
                Estatus = UsuarioEstatusType.Ninguno,
                FechaAlta = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacionID = itemDto.UsuarioActualizacionID
            };

            var newItem = await _usuarioServices.AddAsync(item);

            var itemReturnDto = _mapper.Map<UsuarioDto>(newItem);
            var response = new ApiResponse<UsuarioDto>(itemReturnDto);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UsuarioDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutUsuario(Guid id, UsuarioUpdateDto itemDto)
        {
            if (id != itemDto.ID)
            {   
                throw new BusinessException("El id no coincide con el identificador de la ruta.");
            }

            var item = _mapper.Map<Usuario>(itemDto);
            item.ID = id;
            item.FechaActualizacion = DateTime.Now;

            var updatedItem = await _usuarioServices.UpdateAsync(item);
            var itemReturnDto = _mapper.Map<UsuarioDto>(updatedItem);
            var response = new ApiResponse<UsuarioDto>(itemReturnDto);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            var result = await _usuarioServices.DeleteAsync(id);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}
