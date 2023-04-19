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
    public class GrupoRepository : BaseRepository<Grupo>, IGrupoRepository
    {
        // CONSTRUCTOR

        public GrupoRepository(DataCenterContext context) : base(context) { }

        // METHODS

        public override IEnumerable<Grupo> Gets()
        {
            var items = _entity
                .Include(i => i.Usuarios)
                .Include(i => i.Derechos)
                .Include(i => i.UsuarioActualizacion)
                .AsEnumerable();

            return items;
        }

        public override async Task<Grupo?> GetAsync(Guid id)
        {
            return await _entity.Where(i => i.ID == id)
                .Include(i => i.Usuarios)
                .Include(i => i.Derechos)
                .Include(i => i.UsuarioActualizacion)
                .FirstOrDefaultAsync();
        }

        public async Task<Grupo?> GetSingleAsync(Guid id)
        { 
            return await _entity.FindAsync(id);
        }

        public async Task UpdateAsync(Grupo item)
        { 
            var currentItem = await _entity.FindAsync(item.ID);

            if (currentItem != null)
            {
                currentItem.Nombre = item.Nombre;
                currentItem.Descripcion = item.Descripcion;
                currentItem.FechaActualizacion = item.FechaActualizacion;
                currentItem.UsuarioActualizacionID = item.UsuarioActualizacionID;

                _entity.Update(currentItem);
            }
            else
            {
                throw new BusinessException("No se encontró el registro a actualizar en la base de datos.");
            }
        }

        public async Task<bool> ExistNameAsync(string name, Guid exceptionID = default(Guid))
        {
            name = name.ToLower();
            var total = await _entity
                .Where(e => e.Nombre != null 
                    && e.Nombre.ToLower() == name
                    && e.ID != exceptionID)
                .CountAsync();

            return total > 0;
        }

        public async Task AddDerechoAsync(Guid id, Derecho item)
        {
            var grupo = await _entity
                .Include(e => e.Derechos)
                .Where(e => e.ID == id)
                .FirstOrDefaultAsync();

            if (grupo != null)
            {
                (grupo.Derechos ??= new List<Derecho>()).Add(item); // Ver asignacion similar en UsuarioRepository                
            }
        }

        public async Task AddUsuarioAsync(Guid id, Usuario item)
        {
            var grupo = await _entity
                .Include(e => e.Usuarios)
                .Where(e => e.ID == id)
                .FirstOrDefaultAsync();

            if (grupo != null)
            {
                (grupo.Usuarios ??= new List<Usuario>()).Add(item);
            }
        }
    }
}
