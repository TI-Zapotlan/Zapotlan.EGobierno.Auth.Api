using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IUnitOfWork _unitOfWork;

        // CONSTRUCTOR

        public PersonaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // METHODS 

        public PagedList<Persona> Gets(PersonaQueryFilter filters)
        {
            var items = _unitOfWork.PersonaRepository.Gets();

            if (!string.IsNullOrEmpty(filters.Texto)) { 
                filters.Texto = filters.Texto.ToLower().Trim();
                items = items.Where(p => 
                    p.Nombres.ToLower().Contains(filters.Texto)
                    || (p.PrimerApellido != null && p.PrimerApellido.ToLower().Contains(filters.Texto))
                    || (p.SegundoApellido != null && p.SegundoApellido.ToLower().Contains(filters.Texto))
                );
            }

            if (filters.EstadoVida != null && filters.EstadoVida != Enumerations.PersonaEstadoVidaType.Ninguno)
            {
                items = items.Where(p => p.EstadoVida == filters.EstadoVida);
            }

            //if (filters.Orden != null && filters.Orden != Enumerations.PersonaOrdenFilterTipo.Ninguno)
            //{
            switch (filters.Orden) 
            {
                case PersonaOrderFilterType.Nombre:
                    items = items.OrderBy(p => p.Nombres)
                        .ThenBy(p => p.PrimerApellido);
                    break;
                case PersonaOrderFilterType.PrimerApellido:
                    items = items.OrderBy(p => p.PrimerApellido)
                        .ThenBy(p => p.SegundoApellido);
                    break;
                case PersonaOrderFilterType.EstadoVida:
                    items = items.OrderBy(p => p.EstadoVida);
                    break;
                case PersonaOrderFilterType.NombreDesc:
                    items = items.OrderByDescending(p => p.Nombres)
                        .ThenByDescending(p => p.PrimerApellido);
                    break;
                case PersonaOrderFilterType.PrimerApellidoDesc:
                    items = items.OrderByDescending(p => p.PrimerApellido)
                        .ThenByDescending(p => p.SegundoApellido);
                    break;
            }
            //}

            var pagedItems = PagedList<Persona>.Create(items, filters.PageNumber, filters.PageSize);

            return pagedItems;
        }

        public async Task<Persona?> GetAsync(Guid id) 
        {
            return await _unitOfWork.PersonaRepository.GetAsync(id);
        }
    }
}
