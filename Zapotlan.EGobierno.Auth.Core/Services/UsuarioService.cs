using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Services
{
    public class UsuarioService : IUsuarioService
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        // CONSTRUCTOR 

        public UsuarioService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        // METHODS

        public PagedList<Usuario> Gets(UsuarioQueryFilter filters)
        {
            //filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            //filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPagesize : filters.PageSize;

            var items = _unitOfWork.UsuarioRepository.Gets();

            // Filtros de busqueda 
            
            if (filters.AreaID != null && filters.AreaID != Guid.Empty)
            {
                items = items.Where(u => u.AreaID == filters.AreaID);
            }

            if (filters.UsuarioActualizacionID != null && filters.UsuarioActualizacionID != Guid.Empty)
            {
                items = items.Where(u => u.UsuarioActualizacionID == filters.UsuarioActualizacionID);
            }

            if (!string.IsNullOrEmpty(filters.Codigo))
            {
                items = items.Where(u => 
                    u.Empleado != null && (u.Empleado.Codigo != null && u.Empleado.Codigo.ToLower().Contains(filters.Codigo.ToLower()))
                );
            }

            if (!string.IsNullOrEmpty(filters.Username))
            {
                items = items.Where(u =>
                    u.Username != null && u.Username.ToLower().Contains(filters.Username.ToLower()));
            }

            if (!string.IsNullOrEmpty(filters.Nombre))
            {
                items = items.Where(u => 
                    u.Persona != null && (
                        u.Persona.Nombres.ToLower().Contains(filters.Nombre.ToLower())
                        || (u.Persona.PrimerApellido != null && u.Persona.PrimerApellido.ToLower().Contains(filters.Nombre.ToLower()))
                        || (u.Persona.SegundoApellido != null && u.Persona.SegundoApellido.ToLower().Contains(filters.Nombre.ToLower()))
                    )
                );
            }

            // Ordenamiento 

            switch (filters.Orden)
            {
                case Enumerations.UsuarioOrdenFilterTipo.Username:
                    items = items.OrderBy(u => u.Username);
                    break;
                case Enumerations.UsuarioOrdenFilterTipo.Nombre:
                    items = items.OrderBy(u => u.Persona != null ? u.Persona.Nombres : u.Username);
                    break;
                case Enumerations.UsuarioOrdenFilterTipo.FechaAlta:
                    items = items.OrderBy(u => u.FechaAlta);
                    break;
                case Enumerations.UsuarioOrdenFilterTipo.UsernameDesc:
                    items = items.OrderByDescending(u => u.Username);
                    break;
                case Enumerations.UsuarioOrdenFilterTipo.NombreDesc:
                    items = items.OrderByDescending(u => u.Persona != null ? u.Persona.Nombres : u.Username);
                    break;
                case Enumerations.UsuarioOrdenFilterTipo.FechaAltaDesc:
                    items = items.OrderByDescending(u => u.FechaAlta);
                    break;
                default:
                    items = items.OrderBy(u => u.Username);
                    break;
            }

            var pagedItems = PagedList<Usuario>.Create(items, filters.PageNumber, filters.PageSize);

            return pagedItems;

            // return _unitOfWork.UsuarioRepository.Gets();
        }

        public async Task<Usuario?> GetAsync(Guid id)
        {
            return await _unitOfWork.UsuarioRepository.GetAsync(id);
        }

        public async Task AddAsync(Usuario item)
        {
            // INFO: Validaciones minimas, solo es para crear registros temporales

            // Eliminar registros temporales del usuario
            await _unitOfWork.UsuarioRepository.DeleteTmpByUpdaterUserIDAsync(item.UsuarioActualizacionID);

            await _unitOfWork.UsuarioRepository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Usuario item)
        {
            // INFO: Aquí van las validaciones

            if (item.Estatus == Enumerations.UsuarioEstatusTipo.Ninguno) { // Es un registo nuevo
                item.Estatus = Enumerations.UsuarioEstatusTipo.Activo;
            }

            // Validar que el username no exista
            if (!string.IsNullOrEmpty(item.Username))
            {
                bool existUsername = await _unitOfWork.UsuarioRepository.ExistUsernameAsync(item.Username, item.ID);
                if (existUsername)
                {
                    throw new BusinessException("El nombre de usuario ya existe");
                }
            }

            await _unitOfWork.UsuarioRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return true; //Todo: Cambiarlo por el objeto completo que se actualizó
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            //Todo: Aquí van las validaciones

            await _unitOfWork.UsuarioRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true; //Todo: Cambiarlo por el objeto completo que se eliminó
        }
    }
}
