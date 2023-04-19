using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Services
{
    public class GrupoService : IGrupoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        // CONSTRUCTOR

        public GrupoService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        // METHODS

        public PagedList<Grupo> Gets(GrupoQueryFilter filters)
        {
            var items = _unitOfWork.GrupoRepository.Gets();

            // Filters

            if (filters.UsuarioID != null && filters.UsuarioID != Guid.Empty)
            {   
                items = items.Where(i => i.Usuarios != null 
                    && i.Usuarios.Where(u => u.ID == filters.UsuarioID).Any());
            }

            if (filters.DerechoID != null)
            {
                items = items.Where(e => e.Derechos != null
                    && e.Derechos.Where(d => d.DerechoID == filters.DerechoID).Any());
            }

            if (filters.UsuarioActualizacionID != null && filters.UsuarioActualizacionID != Guid.Empty)
            {
                items = items.Where(i => i.Equals(filters.UsuarioActualizacionID));
            }

            if (!string.IsNullOrEmpty(filters.Texto))
            {
                filters.Texto = filters.Texto.ToLower();
                items = items.Where(i => 
                    (i.Nombre != null && i.Nombre.ToLower().Contains(filters.Texto))
                    || (i.Descripcion != null && i.Descripcion.ToLower().Contains(filters.Texto)));
            }

            // Order

            switch (filters.Orden)
            {
                case GrupoOrderFilterType.Nombre:
                    items = items.OrderBy(i => i.Nombre);
                    break;
                case GrupoOrderFilterType.FechaActualizacion:
                    items = items.OrderBy(i => i.FechaActualizacion);
                    break;
                case GrupoOrderFilterType.NombreDesc:
                    items = items.OrderByDescending(i => i.Nombre);
                    break;
                case GrupoOrderFilterType.FechaActualizacionDesc:
                    items = items.OrderByDescending(i => i.FechaActualizacion);
                    break;
                default:
                    items = items.OrderBy(i => i.Nombre);
                    break;
            }

            var pagedItems = PagedList<Grupo>.Create(items, filters.PageNumber, filters.PageSize);

            return pagedItems;
        }

        public async Task<Grupo?> GetAsync(Guid id)
        {
            return await _unitOfWork.GrupoRepository.GetAsync(id);
        }

        public async Task<Grupo> AddAsync(Grupo item)
        {
            if (item.UsuarioActualizacionID == Guid.Empty)
            {
                throw new BusinessException("Faltó especificar el identificador del usuario que ejecuta la aplicación");
            }

            if (string.IsNullOrEmpty(item.Nombre))
            {
                throw new BusinessException("Faltó especificar el nombre del Grupo");
            }

            await _unitOfWork.GrupoRepository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return item;
        }

        public async Task<Grupo> UpdateAsync(Grupo item)
        {
            if (item.UsuarioActualizacionID == Guid.Empty)
            {
                throw new BusinessException("Faltó especificar el identificador del usuario que ejecuta la aplicación");
            }

            if (string.IsNullOrEmpty(item.Nombre))
            {
                throw new BusinessException("Faltó especificar el nombre del Grupo");
            }

            // HACK: Considerar si es necesario una validación de nombre duplicado

            await _unitOfWork.GrupoRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return item;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.GrupoRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddDerechoAsync(Guid id, int derechoID)
        { 
            var derecho = await _unitOfWork.DerechoRepository.GetSingleAsync(derechoID);

            if (derecho != null)
            { 
                await _unitOfWork.GrupoRepository.AddDerechoAsync(id, derecho);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AddUsuarioAsync(Guid id, Guid usuarioID)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetSingleAsync(usuarioID);

            if (usuario != null)
            {
                await _unitOfWork.GrupoRepository.AddUsuarioAsync(id, usuario);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

    }
}
