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
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        // CONSTRUCTOR
        public UsuarioRepository(DataCenterContext context) : base(context)
        { }

        // METHODS 

        public override IEnumerable<Usuario> Gets()
        {
            var items = _entity
                .Include(u => u.Area)
                //.Include(u => u.Grupos)
                //.Include(u => u.Derechos)
                .Include(u => u.Empleado)
                .Include(u => u.Persona)
                .Include(u => u.UsuarioActualizacion)
                .AsEnumerable();

            return items;
        }

        public override async Task<Usuario?> GetAsync(Guid id)
        {   
            return await _entity.Where(e => e.ID == id).FirstOrDefaultAsync();
        }

        //public override async Task AddAsync(Usuario item)
        //{   
        //    await _entity.AddAsync(item);
        //}

        public async Task UpdateAsync(Usuario item)
        {
            var currentItem = await GetAsync(item.ID); // Obteniendolo de la base de datos para hacer las actualizaciones al registro
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

                _entity.Update(currentItem);
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            var currentItem = await GetAsync(id);
            if (currentItem != null)
            {
                _entity.Remove(currentItem);
            }
        }

        // default(Guid) es la forma de decirle que va a recibir un Guid.Empty - ver: https://stackoverflow.com/questions/5117970/how-can-i-default-a-parameter-to-guid-empty-in-c
        public async Task<bool> ExistUsernameAsync(string username, Guid exceptionID = default(Guid))
        {
            username = username.ToUpper();
            var total = await _entity
                .Where(e => e.Username != null 
                    && e.Username.ToUpper() == username
                    && e.ID != exceptionID) // Cualquier ID va a ser diferente a Guid.Empty si exceptionID viene vacio
                .CountAsync();

            return total > 0;
        }

        public async Task DeleteTmpByUpdaterUserIDAsync(Guid usuarioActualizacionID)
        {
            var users = await _entity
                .Where(e => e.UsuarioActualizacionID == usuarioActualizacionID && e.Estatus == 0)
                .ToListAsync();

            foreach (var user in users)
            {
                _entity.Remove(user);
            }
        }
    }
}
