using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
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
                .Include(u => u.Empleado)
                .Include(u => u.Persona)
                .Include(u => u.UsuarioActualizacion)
                .AsEnumerable();

            return items;
        }

        public override async Task<Usuario?> GetAsync(Guid id)
        {   
            return await _entity.Where(e => e.ID == id)
                .Include(u => u.Area)
                .Include(u => u.Grupos)
                    .ThenInclude(g => g.Derechos)
                .Include(u => u.Derechos)
                .Include(u => u.Empleado)
                .Include(u => u.Persona)
                .Include(u => u.UsuarioActualizacion)
                .FirstOrDefaultAsync();
        }

        //public override async Task AddAsync(Usuario item)
        //{   
        //    await _entity.AddAsync(item);
        //}

        public async Task UpdateAsync(Usuario item)
        {   
            var currentItem = await _entity.FindAsync(item.ID);
            if (currentItem != null)
            {
                currentItem.PersonaID = item.PersonaID;
                currentItem.AreaID = item.AreaID;
                currentItem.EmpleadoID = item.EmpleadoID;
                currentItem.UsuarioJefeID = item.UsuarioJefeID;
                currentItem.Username = item.Username;
                currentItem.Password = string.IsNullOrEmpty(item.Password) ? currentItem.Password : GetMD5Hash(item.Password);
                currentItem.Correo = item.Correo;
                currentItem.Puesto = item.Puesto;
                currentItem.Estatus = item.Estatus;
                currentItem.Rol = item.Rol;
                currentItem.FechaActualizacion = item.FechaActualizacion;

                _entity.Update(currentItem);
            }
            else
            {
                throw new BusinessException("No se encontró el registro en la base de datos.");
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
                .Where(e => e.UsuarioActualizacionID == usuarioActualizacionID && e.Estatus == UsuarioEstatusType.Ninguno)
                .ToListAsync();

            foreach (var user in users)
            {
                _entity.Remove(user);
            }
        }

        public async Task<bool> IsUserValid(Guid id)
        {
            var user = await _entity.FindAsync(id);
            if (user != null)
            {
                // HACK: Aqui valtan más validaciones como:
                // - El empleado esta activo
                // - La persona no ha fallecido

                if (user.Estatus == UsuarioEstatusType.Activo)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<Usuario?> LoginAsync(string username, string password)
        {
            var encryptedPwd = GetMD5Hash(password);
            var user = await _entity
                .Include(e => e.Derechos)
                .Include(e => e.Grupos)
                    .ThenInclude(g => g.Derechos)
                .Where(u => u.Username == username && u.Password == encryptedPwd)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> HasPermisionAsync(Guid id, int derechoID)
        {
            bool hasPermision = false;

            var user = await _entity
                .Include(e => e.Derechos)
                .Include(e => e.Grupos)
                    .ThenInclude(g => g.Derechos)
                .Where(e => e.ID == id).FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.Derechos != null)
                { 
                    var tieneDerecho = user.Derechos.Where(d => d.DerechoID == derechoID).Any();
                    if (tieneDerecho) return true;
                }

                if (user.Grupos != null)
                {
                    foreach (var grupo in user.Grupos)
                    {
                        if (grupo.Derechos != null)
                        {
                            var tieneDerecho = grupo.Derechos.Where(d => d.DerechoID == derechoID).Any();
                            if (tieneDerecho) return true;
                        }
                    }
                }
            }
            
            //var hasPermision = await _entity
            //    .Include(e => e.Derechos)
            //    .Include(e => e.Grupos)
            //        .ThenInclude(g => g.Derechos)
            //    .Where(e => e.ID == id
            //        && 
            //        ((
            //            e.Derechos != null 
            //            && e.Derechos.Where(d => d.DerechoID == derechoID).Any()
            //        )
            //        || 
            //        (
            //            e.Grupos != null
            //            && e.Grupos.Where(g => g.Derechos != null 
            //                && g.Derechos.Where(d => d.DerechoID == derechoID).Any())
            //            .Any()
            //        ))
            //    ).AnyAsync();
            //return hasPermision;

            return false;
        }

        public async Task<bool> AddDerechoAsync(Guid id, Derecho item)
        {
            var user = await _entity
                .Include(e => e.Derechos)
                .Where(e => e.ID == id)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.Derechos == null) { user.Derechos = new List<Derecho>(); }
                user.Derechos.Add(item);
            }

            return true;
        }

        // PRIVATE METHODS

        private string GetMD5Hash(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            MD5 mD5 = MD5.Create();
            byte[] data = mD5.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new();
            int i;

            for (i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        } // GetMD5Hash
    }
}
