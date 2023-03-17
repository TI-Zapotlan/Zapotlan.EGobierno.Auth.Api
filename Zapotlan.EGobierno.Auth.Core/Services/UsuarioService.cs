using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;

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

        public IEnumerable<Usuario> Gets()
        {
            // Filtros de busqueda 

            return _unitOfWork.UsuarioRepository.Gets();
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
            if (item.Username != null)
            {
                bool existUsername = await _unitOfWork.UsuarioRepository.ExistUsernameAsync(item.Username, item.ID);
                if (existUsername)
                {
                    throw new Exception("El nombre de usuario ya existe");
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
