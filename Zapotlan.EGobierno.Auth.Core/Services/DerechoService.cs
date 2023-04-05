using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Services
{
    public class DerechoService : IDerechoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        // CONSTRUCTOR

        public DerechoService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        // METHODS

        public PagedList<Derecho> Gets(DerechoQueryFilter filters)
        {
            var items = _unitOfWork.DerechoRepository.Gets();

            // Filters

            if (filters.UsuarioID != null && filters.UsuarioID != Guid.Empty)
            {
                items = items.Where(e => e.Usuarios != null
                    && e.Usuarios.Where(u => u.ID == filters.UsuarioID).Any());
            }

            if (filters.GrupoID != null && filters.GrupoID != Guid.Empty)
            {
                items = items.Where(e => e.Grupos != null
                    && e.Grupos.Where(u => u.ID == filters.GrupoID).Any());
            }

            if (filters.UsuarioActualizacionID != null && filters.UsuarioActualizacionID != Guid.Empty)
            {
                items = items.Where(e => e.Equals(filters.UsuarioActualizacionID));
            }

            if (!string.IsNullOrEmpty(filters.Texto))
            {
                filters.Texto = filters.Texto.ToLower();
                items = items.Where(e =>
                    (e.Nombre != null && e.Nombre.ToLower().Contains(filters.Texto))
                    || (e.Descripcion != null && e.Descripcion.ToLower().Contains(filters.Texto)));
            }

            if (filters.Acceso != null && filters.Acceso != Enumerations.DerechoAccesoType.Ninguno)
            {
                items = items.Where(e => e.Acceso == filters.Acceso);
            }

            // Order

            switch (filters.Orden)
            {
                case DerechoOrderFilterType.ID:
                    items = items.OrderBy(e => e.ID);
                    break;
                case DerechoOrderFilterType.Nombre:
                    items = items.OrderBy(e => e.Nombre);
                    break;
                case DerechoOrderFilterType.FechaActualizacion:
                    items = items.OrderBy(e => e.FechaActualizacion);
                    break;
                case DerechoOrderFilterType.IDDesc:
                    items = items.OrderByDescending(e => e.ID);
                    break;
                case DerechoOrderFilterType.NombreDesc:
                    items = items.OrderByDescending(e => e.Nombre);
                    break;
                case DerechoOrderFilterType.FechaActualizacionDesc:
                    items = items.OrderByDescending(e => e.FechaActualizacion);
                    break;
                default:
                    items = items.OrderBy(e => e.ID);
                    break;
            }

            var pagedItems = PagedList<Derecho>.Create(items, filters.PageNumber, filters.PageSize);

            return pagedItems;
        }

        public Task<Derecho?> GetAsync(int id)
        {
            return _unitOfWork.DerechoRepository.GetAsync(id);
        }

        public async Task<Derecho> AddAsync(Derecho item)
        {
            if (item.UsuarioActualizacionID == Guid.Empty)
            {
                throw new BusinessException("Faltó especificar el identificador del usuario que ejecuta la aplicación");
            }

            if (string.IsNullOrEmpty(item.Nombre))
            {
                throw new BusinessException("Faltó especificar el nombre del Derecho");
            }

            if (item.Acceso == DerechoAccesoType.Ninguno)
            {
                throw new BusinessException("Faltó especificar el tipo de acceso al Derecho");
            }

            if (await _unitOfWork.DerechoRepository.ExistIDAsync(item.DerechoID))
            {
                throw new BusinessException("El ID ya existe.");
            }

            if (await _unitOfWork.DerechoRepository.ExistNameAsync(item.Nombre))
            {
                throw new BusinessException("El nombre ya existe.");
            }

            await _unitOfWork.DerechoRepository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return item;
        }

        public async Task<Derecho> UpdateAsync(Derecho item)
        {
            if (item.UsuarioActualizacionID == Guid.Empty)
            {
                throw new BusinessException("Faltó especificar el identificador del usuario que ejecuta la aplicación");
            }

            if (string.IsNullOrEmpty(item.Nombre))
            {
                throw new BusinessException("Faltó especificar el nombre del Derecho");
            }

            if (item.Acceso == DerechoAccesoType.Ninguno)
            {
                throw new BusinessException("Faltó especificar el tipo de acceso al Derecho");
            }

            if (await _unitOfWork.DerechoRepository.ExistNameAsync(item.Nombre, item.DerechoID))
            {
                throw new BusinessException("El nombre ya existe.");
            }

            await _unitOfWork.DerechoRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Validaciones

            await _unitOfWork.DerechoRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
