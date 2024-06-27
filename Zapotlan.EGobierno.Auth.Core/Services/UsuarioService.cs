using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
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

            if (filters.DerechoID != null)
            {
                items = items.Where(e => e.Derechos != null
                    && e.Derechos.Where(d => d.DerechoID == filters.DerechoID).Any());
            }

            if (filters.GrupoID != null && filters.GrupoID != Guid.Empty)
            {
                items = items.Where(e => e.Grupos != null
                    && e.Grupos.Where(g => g.ID == filters.GrupoID).Any());
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
                case UsuarioOrderFilterType.Username:
                    items = items.OrderBy(u => u.Username);
                    break;
                case UsuarioOrderFilterType.Nombre:
                    items = items.OrderBy(u => u.Persona != null ? u.Persona.Nombres : u.Username);
                    break;
                case UsuarioOrderFilterType.FechaAlta:
                    items = items.OrderBy(u => u.FechaAlta);
                    break;
                case UsuarioOrderFilterType.UsernameDesc:
                    items = items.OrderByDescending(u => u.Username);
                    break;
                case UsuarioOrderFilterType.NombreDesc:
                    items = items.OrderByDescending(u => u.Persona != null ? u.Persona.Nombres : u.Username);
                    break;
                case UsuarioOrderFilterType.FechaAltaDesc:
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
        } // GetAsync

        /// <summary>
        /// Agrega un nuevo registro a la base de datos, con sus propiedades en blanco como temporal
        /// para ser editado en el metodo <see cref="UpdateAsync"/> 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Usuario> AddAsync(Usuario item)
        {
            // INFO: Validaciones minimas, solo es para crear registros temporales
            // - Que el usuario sea válido
            
            if (!await _unitOfWork.UsuarioRepository.IsUserValid(item.UsuarioActualizacionID))
                throw new BusinessException("El usuario no tiene los privilegios para crear un nuevo usuario.");
            

            item.ID = Guid.NewGuid();
            item.Estatus = UsuarioEstatusType.Ninguno;
            item.FechaAlta = DateTime.Now;
            item.FechaActualizacion = DateTime.Now;
            
            await _unitOfWork.UsuarioRepository.DeleteTmpByUpdaterUserIDAsync(item.UsuarioActualizacionID);
            await _unitOfWork.UsuarioRepository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return item;
        } // AddAsync

        public async Task<Usuario> UpdateAsync(Usuario item)
        {
            if (!await _unitOfWork.UsuarioRepository.IsUserValid(item.UsuarioActualizacionID))
                throw new BusinessException("El usuario no tiene los privilegios para crear un nuevo usuario.");

            var foundItem = await _unitOfWork.UsuarioRepository.GetAsync(item.ID)
                ?? throw new BusinessException("No se encuentra el registro del Usuario a actualizar");

            if (foundItem.Estatus == UsuarioEstatusType.Eliminado)
                throw new BusinessException("No se encuentra el registro del Usuario a actualizar");

            if (!string.IsNullOrEmpty(item.Username) 
                && await _unitOfWork.UsuarioRepository.ExistUsernameAsync(item.Username, item.ID))
                throw new BusinessException("El nombre de usuario ya existe");

            if (foundItem.Estatus == UsuarioEstatusType.Ninguno && string.IsNullOrEmpty(item.Password))
                throw new BusinessException("Faltó especificar la contraseña del usuario");

            if (!string.IsNullOrEmpty(item.Password))
            {
                CreatePasswordHash(item.Password ?? string.Empty, out byte[] passwordHash, out byte[] passwordSalt);
                foundItem.PasswordHash = passwordHash;
                foundItem.PasswordSalt = passwordSalt;
            }

            foundItem.PersonaID = item.PersonaID;
            foundItem.AreaID = item.AreaID;
            foundItem.EmpleadoID = item.EmpleadoID;
            foundItem.UsuarioJefeID = item.UsuarioJefeID;
            foundItem.Username = item.Username;
            foundItem.Password = string.IsNullOrEmpty(item.Password) 
                ? foundItem.Password 
                : GetMD5Hash(item.Password);
            foundItem.Correo = item.Correo;
            foundItem.Puesto = item.Puesto;
            foundItem.Estatus = foundItem.Estatus == UsuarioEstatusType.Ninguno
                ? UsuarioEstatusType.Activo
                : item.Estatus;
            foundItem.Rol = item.Rol;
            foundItem.FechaActualizacion = DateTime.Now;
            foundItem.UsuarioActualizacionID = item.UsuarioActualizacionID;

            _unitOfWork.UsuarioRepository.Update(item);
            await _unitOfWork.SaveChangesAsync();

            return item;
        } // UpdateAsync

        /// <summary>
        /// Actualiza la información del token para un usuario de acuerdo a los parametros recibidos
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <param name="refreshToken">Datos del token a actualizar</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task UpdateTokenDataAsync(Guid id, RefreshToken refreshToken)
        {
            var foundItem = await _unitOfWork.UsuarioRepository.GetAsync(id)
                ?? throw new BusinessException("No se encuentra el registro del Usuario a actualizar el token");

            foundItem.RefreshToken = refreshToken.Token;
            foundItem.TokenCreated = refreshToken.Created;
            foundItem.TokenExpires = refreshToken.Expires;

            _unitOfWork.UsuarioRepository.Update(foundItem);
            await _unitOfWork.SaveChangesAsync();
        } // UpdateTokenDataAsync

        public async Task<bool> DeleteAsync(Guid id)
        {
            //Todo: Aquí van las validaciones

            await _unitOfWork.UsuarioRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddDerechoAsync(Guid id, int derechoID)
        {
            var derecho = await _unitOfWork.DerechoRepository.GetSingleAsync(derechoID);

            if (derecho != null)
            { 
                await _unitOfWork.UsuarioRepository.AddDerechoAsync(id, derecho);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> AddGrupoAsync(Guid id, Guid grupoID)
        {
            var grupo = await _unitOfWork.GrupoRepository.GetSingleAsync(grupoID);

            if (grupo != null)
            {
                await _unitOfWork.UsuarioRepository.AddGrupoAsync(id, grupo); 
                await _unitOfWork.SaveChangesAsync();

                return true;
            }

            return false;
        }

        // USER AUTH

        /// <summary>
        /// Verifica si se puede iniciar sesión con el username y el password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Usuario?> LoginAsync(string username, string password)
        {   
            var item = await _unitOfWork.UsuarioRepository.GetByUsernameAsync(username)
                ?? throw new BusinessException("El nombre de usuario y/o contraseña no son validos.");

            if (item.PasswordHash != null && item.PasswordSalt != null)
            {
                if (!VerifyPasswordHash(password, item.PasswordHash, item.PasswordSalt))
                    throw new BusinessException("El nombre de usuario y/o contraseña no son validos.");
            }
            else
            {
                item = await _unitOfWork.UsuarioRepository.LoginAsync(username, password)
                    ?? throw new BusinessException("El nombre de usuario y/o contraseña no son validos.");

                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                item.PasswordHash = passwordHash;
                item.PasswordSalt = passwordSalt;
                _unitOfWork.UsuarioRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
            }

            if (item.Estatus != UsuarioEstatusType.Activo) 
                throw new BusinessException("El usuario no se encuentra activo.");
            if (item.Empleado != null && item.Empleado.Estatus != EmpleadoEstatusType.Activo)
                throw new BusinessException("El empleado asociado al usuario no se encuentra activo.");
            if (item.Persona != null && item.Persona.EstadoVida == PersonaEstadoVidaType.Fallecido)
                throw new BusinessException("La persona asociada al empleado esta marcada como fallecida.");

            return item;
        }

        public async Task<bool> HasPermissionAsync(Guid id, int derechoID)
        {
            // HACK - Validaciones faltantes
            // 1. Que el derecho solicitado tenga acceso total o denegacion total - YA
            // 2. Que el usuario este dado de baja - YA
            // 3. Que el empleado asociado no este activo  - YA
            // 4. Que la persona no haya fallecido - YA

            var derecho = await _unitOfWork.DerechoRepository.GetAsync(derechoID);
            var usuario = await _unitOfWork.UsuarioRepository.GetAsync(id);

            if (derecho == null) throw new BusinessException("El derecho no existe.");
            if (usuario == null) throw new BusinessException("El usuario no existe");

            if (usuario.Empleado != null && usuario.Empleado.Estatus != EmpleadoEstatusType.Activo) return false;            
            if (usuario.Estatus != UsuarioEstatusType.Activo) return false;
            if (usuario.Persona != null && usuario.Persona.EstadoVida == PersonaEstadoVidaType.Fallecido) return false;

            if (derecho.Acceso == DerechoAccesoType.PermitirTodos) return true;
            if (derecho.Acceso == DerechoAccesoType.DenegarTodos) return false;

            return await _unitOfWork.UsuarioRepository.HasPermissionAsync(id, derechoID);
        }

        // PRIVATE

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        } // CreatePasswordHash

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computeHash.SequenceEqual(passwordHash);
        } // VerifyPasswordHash

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
