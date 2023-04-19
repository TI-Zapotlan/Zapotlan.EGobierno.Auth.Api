using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Infrastructure.Data;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Repositories
{
    public class DerechoRepository : BaseRepository<Derecho>, IDerechoRepository
    {
        // CONSTRUCTOR 

        public DerechoRepository(DataCenterContext context) : base(context) { }

        // METHODS

        public override IEnumerable<Derecho> Gets()
        {
            var items = _entity
                .Include(e => e.Grupos)
                .Include(e => e.Usuarios)
                .Include(e => e.UsuarioActualizacion)
                .AsEnumerable();

            return items;
        }

        public override async Task<Derecho?> GetAsync(int id)
        { 
            return await _entity.Where(e => e.DerechoID == id)
                .Include(e => e.Grupos)
                .Include(e => e.Usuarios)
                .Include(e => e.UsuarioActualizacion)
                .FirstOrDefaultAsync();
        }

        public async Task<Derecho?> GetSingleAsync(int id)
        {
            return await _entity.FindAsync(id);
        }

        public async Task UpdateAsync(Derecho item)
        { 
            var currentItem = await _entity.FindAsync(item.DerechoID);

            if (currentItem != null)
            {
                currentItem.Nombre = item.Nombre;
                currentItem.Descripcion = item.Descripcion;
                currentItem.Acceso = item.Acceso;
                currentItem.FechaActualizacion = item.FechaActualizacion;
                currentItem.UsuarioActualizacionID = item.UsuarioActualizacionID;

                _entity.Update(currentItem);
            }
            else {
                throw new BusinessException("No se encontró el registro a actualizar en la base de datos.");
            }
        }

        public async Task<bool> ExistNameAsync(string name, int exceptionID = default)
        {
            name = name.ToLower();
            var result = await _entity
                .Where(e => e.Nombre != null
                    && e.Nombre.ToLower().Equals(name)
                    && e.DerechoID != exceptionID)
                .AnyAsync();

            return result;
        }

        public async Task<bool> ExistIDAsync(int id)
        {   
            var result = await _entity
                .Where(e => e.DerechoID == id)
                .AnyAsync();

            return result;
        }
    }
}
