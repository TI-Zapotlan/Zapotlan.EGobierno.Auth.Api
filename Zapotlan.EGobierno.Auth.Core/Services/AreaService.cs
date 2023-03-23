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
    public class AreaService : IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;

        // CONSTRUCTOR

        public AreaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // METHODS

        public PagedList<Area> Gets(AreaQueryFilter filters)
        {
            var items = _unitOfWork.AreaRepository.Gets();

            // Filtros

            if (filters.AreaPadreID != null && filters.AreaPadreID != Guid.Empty)
            { 
                items = items.Where(a => a.AreaPadreID == filters.AreaPadreID);
            }

            if (!string.IsNullOrEmpty(filters.Nombre))
            { 
                items = items.Where(a => a.Nombre != null && a.Nombre.ToLower().Contains(filters.Nombre.ToLower()));
            }

            if (filters.Activo != null && filters.Activo != Enumerations.AreaEstatusTipo.Ninguno)
            {
                items = items.Where(a => a.Activo == filters.Activo);
            }

            // Ordenamiento

            switch (filters.Orden)
            {
                case AreaOrdenFilterTipo.Clave:
                    items = items.OrderBy(a => a.Clave);
                    break;
                case AreaOrdenFilterTipo.Nombre:
                    items = items.OrderBy(a => a.Nombre);
                    break;
                case AreaOrdenFilterTipo.FechaActualizacion:
                    items = items.OrderBy(a => a.FechaActualizacion);
                    break;
                case AreaOrdenFilterTipo.ClaveDesc:
                    items = items.OrderByDescending(a => a.Clave);
                    break;
                case AreaOrdenFilterTipo.NombreDesc:
                    items = items.OrderByDescending(a => a.Nombre);
                    break;
                case AreaOrdenFilterTipo.FechaActualizacionDesc:
                    items = items.OrderByDescending(a => a.FechaActualizacion);
                    break;
                default:
                    items = items.OrderBy(a => a.Nombre);
                    break;
            }

            var pagedItems = PagedList<Area>.Create(items, filters.PageNumber, filters.PageNumber);

            return pagedItems;
        }

        public async Task<Area?> GetAsync(Guid id)
        {
            return await _unitOfWork.AreaRepository.GetAsync(id);
        }
    }
}
