using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Core.QueryFilters;

namespace Zapotlan.EGobierno.Auth.Core.Services
{
    public class UsuarioService : IUsuarioService
    {   
        private readonly IUnitOfWork _unitOfWork;

        // CONSTRUCTOR 

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // METHODS

        public IEnumerable<Usuario> Gets(UsuarioQueryFilter filters)
        {
            var items = _unitOfWork.UsuarioRepository.Gets();

            //// Filtros de busqueda 
            //if (filters.AreaID != null && filters.AreaID != Guid.Empty)
            //{
            //    items = items.Where(u => u.AreaID == filters.AreaID);
            //}

            ////if (filters.GrupoID != Guid.Empty)
            ////{
            ////    items = items.Where(u => u.Grupos?.Where(g => g.ID == filters.GrupoID).Count() > 0);
            ////}

            //if (filters.UsuarioActualizacionID != null && filters.UsuarioActualizacionID != Guid.Empty)
            //{
            //    items = items.Where(u => u.UsuarioActualizacionID == filters.UsuarioActualizacionID);
            //}

            //if (!string.IsNullOrEmpty(filters.Username))
            //{
            //    items = items.Where(predicate: u => u.Username.ToLower().Contains(filters.Username.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(filters.Nombre))
            //{
            //    items = items.Where(u => u.Persona.Nombres.ToLower().Contains(filters.Nombre.ToLower())
            //        || u.Persona.PrimerApellido.ToLower().Contains(filters.Nombre.ToLower())
            //        || u.Persona.SegundoApellido.ToLower().Contains(filters.Nombre.ToLower()));
            //}

            return items.ToList();

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

            if (item.Estatus == 0) { // Es un registo nuevo
                item.Estatus = 1;
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
