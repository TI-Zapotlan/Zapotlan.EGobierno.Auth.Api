﻿using Microsoft.EntityFrameworkCore;
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
                .Where(e => e.UsuarioActualizacionID == usuarioActualizacionID && e.Estatus == UsuarioEstatusTipo.Ninguno)
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
                // HACK: Aqui valtan más validaciones

                if (user.Estatus == UsuarioEstatusTipo.Activo)
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
                .Where(u => u.Username == username && u.Password == encryptedPwd)
                .FirstOrDefaultAsync();

            return user;
        }

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
