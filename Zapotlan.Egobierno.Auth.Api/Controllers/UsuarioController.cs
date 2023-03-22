using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Zapotlan.EGobierno.Auth.Api.Mappings;
using Zapotlan.EGobierno.Auth.Api.Responses;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;
using Zapotlan.EGobierno.Auth.Infrastructure.Interfaces;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
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
            _usuariosMapping = new UsuariosMapping(mapper);
            _uriService = uriService;
        }

        /// <summary>
        /// Devuelve un listado de usuarios de acuerdo a los filtros establecidos
        /// </summary>
        /// <param name="filters">Listado de filtros disponibles</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet(Name = nameof(Gets))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UsuarioListDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Gets([FromQuery]UsuarioQueryFilter filters)
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
                    Url.RouteUrl(nameof(Gets))
                    //, items.NextPageNumber
                ).ToString(),
                PreviousPageUrl = _uriService.GetUsuarioPaginationUri(
                    filters,
                    Url.RouteUrl(nameof(Gets))
                ).ToString()
            };

            var response = new ApiResponse<IEnumerable<UsuarioListDto>>(itemsDto) {
                Meta = metadata
            };

            // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _usuarioServices.GetAsync(id);
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

            await _usuarioServices.AddAsync(item);

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

            var result = await _usuarioServices.UpdateAsync(item);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _usuarioServices.DeleteAsync(id);
            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}
