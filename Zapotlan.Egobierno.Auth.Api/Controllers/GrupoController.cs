using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Zapotlan.EGobierno.Auth.Api.Mappings;
using Zapotlan.EGobierno.Auth.Api.Responses;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoService _grupoService;
        private readonly IMapper _mapper;
        private readonly GruposMapping _gruposMapping;

        // CONSTRUCTOR

        public GrupoController(IGrupoService grupoService, IMapper mapper)
        {
            _grupoService = grupoService;
            _mapper = mapper;

            _gruposMapping = new GruposMapping(_mapper);
        }

        // ENDPOINTS

        [Produces("application/json")]
        [HttpGet(Name = nameof(GetGrupos))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<GrupoListDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetGrupos([FromQuery] GrupoQueryFilter filters)
        {
            var items = _grupoService.Gets(filters);
            var itemsDto = _gruposMapping.GrupoToListDto(items);
            var metadata = new Metadata
            {
                TotalCount = items.TotalCount,
                PageSize = items.PageSize,
                CurrentPage = items.CurrentPage,
                TotalPages = items.TotalPages,
                HasNextPage = items.HasNextPage,
                HasPreviousPage = items.HasPreviousPage,
            };

            var response = new ApiResponse<IEnumerable<GrupoListDto>>(itemsDto) {
                Meta = metadata
            };

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<GrupoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetGrupo(Guid id)
        {
            var item = await _grupoService.GetAsync(id);

            if (item == null)
            {
                throw new BusinessException($"No se encontró un elemento con el Id: {id}");
            }

            var itemDto = _gruposMapping.GrupoToDetailDto(item);
            var response = new ApiResponse<GrupoDetailsDto>(itemDto);
            
            return Ok(response);
        }

        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<GrupoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostGrupo(GrupoInsertDto itemDto)
        {
            var item = new Grupo
            {
                ID = Guid.NewGuid(),
                Nombre = itemDto.Nombre,
                Descripcion = itemDto.Descripcion,
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacionID = itemDto.UsuarioActualizacionID
            };

            var newItem = await _grupoService.AddAsync(item);
            var itemReturnDto = _mapper.Map<GrupoDto>(newItem);
            var response = new ApiResponse<GrupoDto>(itemReturnDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<GrupoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutUsuario(Guid id, GrupoUpdateDto itemDto)
        {
            if (id != itemDto.ID)
            {
                throw new BusinessException("El id no coincide con el identificador de la ruta");
            }

            var item = _mapper.Map<Grupo>(itemDto);
            item.ID = id;
            item.FechaActualizacion = DateTime.Now;

            var updatedItem = await _grupoService.UpdateAsync(item);
            var itemReturnDto = _mapper.Map<GrupoDto>(updatedItem);
            var response = new ApiResponse<GrupoDto>(itemReturnDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteGrupo(Guid id)
        {
            var result = await _grupoService.DeleteAsync(id);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [HttpPost("{id}/add-derecho")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> PostAddDerecho(Guid id, [FromBody] DerechoIDDto derecho)
        { 
            var itemAdded = await _grupoService.AddDerechoAsync(id, derecho.DerechoID);
            var response = new ApiResponse<bool>(itemAdded);

            return Ok(response);
        }

        [HttpPost("{id}/add-usuario")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> PostAddUsuario(Guid id, [FromBody] UsuarioIDDto usuario)
        {
            var itemAdded = await _grupoService.AddUsuarioAsync(id, usuario.UsuarioID);
            var response = new ApiResponse<bool>(itemAdded);

            return Ok(response);
        }
    }
}
