using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Zapotlan.EGobierno.Auth.Api.Responses;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;
using Zapotlan.EGobierno.Auth.Infrastructure.Interfaces;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaService _personaService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PersonaController(IPersonaService personaService, IMapper mapper, IUriService uriService)
        {
            _personaService = personaService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Devuelve un listado de personas de acuerdo a los filtros establecidos
        /// </summary>
        /// <param name="filters">
        ///   Objeto con los filtros disponibles puede ser paginado, agregando los
        ///   parametros PageSize y PageNumber
        /// </param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet(Name = nameof(GetsPersonas))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Persona>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetsPersonas([FromQuery] PersonaQueryFilter filters)
        {
            var items = _personaService.Gets(filters);
            var metadata = new Metadata
            {
                TotalCount = items.TotalCount,
                PageSize = items.PageSize,
                CurrentPage = items.CurrentPage,
                TotalPages = items.TotalPages,
                HasNextPage = items.HasNextPage,
                HasPreviousPage = items.HasPreviousPage,
            };

            var response = new ApiResponse<IEnumerable<Persona>>(items) {
                Meta = metadata
            };

            return Ok(response);
        }

        /// <summary>
        /// Devuelve el registro de una persona dado su identificador
        /// </summary>
        /// <param name="id">Identificador de la persona aconsultar</param>
        /// <returns>
        ///   Registro de tipo Zapotlan.EGobierno.Auth.Core.Entities.Persona con la información
        ///   del registro obtenido en la consulta.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Persona))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPersona(Guid id)
        { 
            var item = await _personaService.GetAsync(id);

            if (item == null) {
                return NotFound($"No se encontró un registro con el identificador '{ id }'");
            }

            var response = new ApiResponse<Persona>(item);
            return Ok(response);
        }
    }
}
