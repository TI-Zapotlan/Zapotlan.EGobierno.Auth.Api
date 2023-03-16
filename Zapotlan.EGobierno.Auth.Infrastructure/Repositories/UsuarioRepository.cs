using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;
using Zapotlan.EGobierno.Auth.Infrastructure.Data;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataCenterContext _context;

        public UsuarioRepository(DataCenterContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> Gets()
        {
            var items = await _context
                .Usuarios
                //.Include(u => u.Grupos)
                //.Include(u => u.Derechos)
                .Include(u => u.UsuarioActualizacion)
                .ToListAsync();

            return items;
        }

        public async Task<Usuario?> Get(Guid id)
        {
            var item = await _context.Usuarios.FirstOrDefaultAsync(i => i.ID == id);

            return item;
        }

        public async Task Insert(Usuario item)
        {
            _context.Usuarios.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Usuario item)
        {
            var currentItem = await Get(item.ID); // Obteniendolo de la base de datos para hacer las actualizaciones al registro
            if (currentItem != null)
            {
                currentItem.PersonaID = item.PersonaID;
                currentItem.AreaID = item.AreaID;
                currentItem.EmpleadoID = item.EmpleadoID;
                currentItem.UsuarioJefeID = item.UsuarioJefeID;
                currentItem.Username = item.Username;
                currentItem.Password = item.Password;
                currentItem.Correo = item.Correo;
                currentItem.Puesto = item.Puesto;
                currentItem.Estatus = item.Estatus;
                currentItem.Rol = item.Rol;
                currentItem.FechaActualizacion = item.FechaActualizacion;
            }
            else return false;

            int rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var currentItem = await Get(id);
            if (currentItem != null)
            {
                _context.Usuarios.Remove(currentItem);
                int rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0;
            }

            return false;
        }
    }
}
