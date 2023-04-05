using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using Zapotlan.EGobierno.Auth.Core.Services;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DerechoController : ControllerBase
    {
        private readonly IDerechoService _derechoService;
        private readonly IMapper _mapper;
        private readonly DerechosMapping _derechosMapping;

        // CONSTRUCTOR

        public DerechoController(IDerechoService derechoService, IMapper mapper)
        {
            _derechoService = derechoService;
            _mapper = mapper;

            _derechosMapping = new DerechosMapping(_mapper);
        }

        // ENDPOINTS

        [Produces("application/json")]
        [HttpGet(Name = nameof(GetDerechos))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<DerechoListDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetDerechos([FromQuery] DerechoQueryFilter filters)
        {
            var items = _derechoService.Gets(filters);
            var itemsDto = _derechosMapping.DerechoToListDto(items);
            var metadata = new Metadata
            {
                TotalCount = items.TotalCount,
                PageSize = items.PageSize,
                CurrentPage = items.CurrentPage,
                TotalPages = items.TotalPages,
                HasNextPage = items.HasNextPage,
                HasPreviousPage = items.HasPreviousPage,
            };

            var response = new ApiResponse<IEnumerable<DerechoListDto>>(itemsDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<DerechoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDerecho(int id)
        {
            var item = await _derechoService.GetAsync(id);

            if (item == null)
            {
                throw new BusinessException($"No se encontró un elemento con el Id: {id}");
            }

            var itemDto = _derechosMapping.DerechoToDetailDto(item);
            var response = new ApiResponse<DerechoDetailsDto>(itemDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<DerechoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostDerecho(DerechoInsertDto itemDto)
        {
            var item = new Derecho
            {
                DerechoID = itemDto.DerechoID,
                Nombre = itemDto.Nombre,
                Descripcion = itemDto.Descripcion,
                Acceso = itemDto.Acceso,
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacionID = itemDto.UsuarioActualizacionID
            };

            var newItem = await _derechoService.AddAsync(item);
            var itemReturnDto = _mapper.Map<DerechoDto>(newItem);
            var response = new ApiResponse<DerechoDto>(itemReturnDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<DerechoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutUsuario(int id, DerechoUpdateDto itemDto)
        {
            if (id != itemDto.DerechoID)
            {
                throw new BusinessException("El id no coincide con el identificador de la ruta");
            }

            var item = _mapper.Map<Derecho>(itemDto);
            item.DerechoID = id;
            item.FechaActualizacion = DateTime.Now;

            var updatedItem = await _derechoService.UpdateAsync(item);
            var itemReturnDto = _mapper.Map<DerechoDto>(updatedItem);
            var response = new ApiResponse<DerechoDto>(itemReturnDto);

            return Ok(response);
        }

        [Produces("application/json")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteDerecho(int id)
        {
            var result = await _derechoService.DeleteAsync(id);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

    }
}
