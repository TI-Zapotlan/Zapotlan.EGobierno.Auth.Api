using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Zapotlan.EGobierno.Auth.Api.Mappings;
using Zapotlan.EGobierno.Auth.Api.Responses;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IMapper _mapper;
        private readonly AreasMapping _areasMapping;

        public AreaController(IAreaService areaService, IMapper mapper)
        {
            _areaService = areaService; 
            _mapper = mapper;
            _areasMapping = new AreasMapping(mapper);
        }

        // ENDPOINTS

        /// <summary>
        /// Devuelve un listado de áreas de acuerdo a los filtros establecidos
        /// </summary>
        /// <param name="filters">
        ///   Objeto con los filtros disponibles, utilice PageSize y PageNumber para devolver los datos
        ///   de forma paginada.
        /// </param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet(Name = nameof(GetsAreas))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Area>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetsAreas([FromQuery] AreaQueryFilter filters)
        {
            var items = _areaService.Gets(filters);
            var itemsDto = _areasMapping .AreasToListDto(items);
            var metadata = new Metadata
            {
                TotalCount = items.TotalCount,
                PageSize = items.PageSize,
                CurrentPage = items.CurrentPage,
                TotalPages = items.TotalPages,
                HasNextPage = items.HasNextPage,
                HasPreviousPage = items.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<AreaDto>>(itemsDto)
            {
                Meta = metadata
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Area))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetArea(Guid id)
        {
            var item = await _areaService.GetAsync(id);

            if (item == null)
            {
                return NotFound($"No se encontró un registro con el identificador '{id}'");
            }

            var response = new ApiResponse<Area>(item);

            return Ok(response);
        }
    }
}
