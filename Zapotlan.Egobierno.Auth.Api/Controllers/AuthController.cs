using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Zapotlan.EGobierno.Auth.Api.Responses;
using Zapotlan.EGobierno.Auth.Core.CustomEntities;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
using Zapotlan.EGobierno.Auth.Core.Exceptions;
using Zapotlan.EGobierno.Auth.Core.Interfaces;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const int TOKEN_EXPIRED_DAYS = 1;
        private const string REFRESH_TOKEN_NAME = "zap-ti-rt";

        private readonly IUsuarioService _usuarioServices;
        private readonly IConfiguration _configuration;

        // CONSTRUCTOR 

        public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioServices = usuarioService;
            _configuration = configuration;
        }

        // ENDPOINTS

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = User?.Identity?.Name;

            return Ok(userName);
        }

        /// <summary>
        /// Obtiene el inicio de sesión de un usuario si coincide su usuario y contraseña
        /// </summary>
        /// <param name="login">
        ///     Recibe usuario (Username) y contraseña (Password) en objeto para validar 
        ///     su acceso, los limites de DerechoInicio y DerechoTermino es para limitar el 
        ///     número de Derechos necesarios en la aplicación
        /// </param>
        /// <returns>Web token validado para realizar solicitudes.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authentication(UsuarioLoginDto login)
        {
            // Validar el usuario
            var usuario = await _usuarioServices.LoginAsync(login.Username, login.Password)
                ?? throw new BusinessException("Nombre de usuario y/o contraseña no validos");                        
            var token = GenerateToken(usuario, login.DerechosInicio, login.DerechosTermino);
            var refreshToken = GenerateRefreshToken();

            await SetRefreshTokenAsync(usuario, refreshToken);

            var response = new ApiResponse<string>(token);
            return Ok(response);            
        } // Autentication

        [HttpPost("refresh-token")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken(UsuarioRefreshTokenDto itemDto)
        {
            var refreshToken = Request.Cookies[REFRESH_TOKEN_NAME];
            var item = await _usuarioServices.GetAsync(itemDto.ID)
                ?? throw new BusinessException("No se encontró el usuario al cual actualizar el token");

            if (string.IsNullOrEmpty(item.RefreshToken))
                return Unauthorized("El Refresh Token esta vacio");
                
            if (!string.IsNullOrEmpty(item.RefreshToken) && !item.RefreshToken.Equals(refreshToken))
                return Unauthorized("Refresh Token no es valido");

            if (item.TokenExpires < DateTime.Now)
                return Unauthorized("El token ha expirado");

            var token = GenerateToken(item, itemDto.DerechosInicio, itemDto.DerechosTermino);
            var newRefreshToken = GenerateRefreshToken();
            await SetRefreshTokenAsync(item, newRefreshToken);
            var response = new ApiResponse<string>(token);

            return Ok(response);
        } // RefreshToken

        /// <summary>
        /// Obtiene si el usuario indicado tiene el permiso especificado en
        /// cualquiera de sus grupos o de forma directa.
        /// </summary>
        /// <param name="values">
        ///     Recibe el id del usuario (UsuarioID) y el id del derecho (DerechoID) 
        ///     a consultar
        /// </param>
        /// <returns>
        ///     Valor de tipo booleano que indica si tiene el derecho (true) o 
        ///     si no (false) especificado.
        /// </returns>
        [HttpPost("has-permission")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> HasPermission(UsuarioHasPermissionDto values)
        {
            var hasPermission = await _usuarioServices.HasPermissionAsync(values.UsuarioID, values.DerechoID);
            var response = new ApiResponse<bool>(hasPermission);

            return Ok(response);
        }

        // PRIVATE METHODS

        private string GenerateToken(Usuario item, int? derechoInicio, int? derechoTermino)
        {
            // Generating token header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Generating claims data
            var derechos = new List<int>();
            var givename = string.Empty;

            if (item.Derechos != null)
            { 
                foreach (var derecho in item.Derechos) { derechos.Add(derecho.DerechoID); }
            }
            if (item.Grupos != null)
            { 
                foreach (var grupo in item.Grupos) 
                {
                    if (grupo.Derechos != null)
                    {
                        foreach (var derecho in grupo.Derechos)
                        {
                            if (!derechos.Exists(d => d == derecho.DerechoID)) derechos.Add(derecho.DerechoID);
                        }
                    }
                }
            }
            if (derechoInicio != null && derechoTermino != null && derechoInicio <= derechoTermino) 
            {
                derechos = derechos.Where(d => d >= derechoInicio && d <= derechoTermino).ToList();
            }

            if (item.Persona != null)
            {
                givename = item.Persona.Nombres;
                givename += !string.IsNullOrEmpty(item.Persona.PrimerApellido) ? " " + item.Persona.PrimerApellido : "";
                givename += !string.IsNullOrEmpty(item.Persona.SegundoApellido) ? " " + item.Persona.SegundoApellido : "";
            }

            // Generating token claim
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, item.Username ?? "(null)"),
                new Claim("idx", item.ID.ToString()),
                new Claim("username", item.Username ?? "(null)"),
                new Claim("givename", givename),
                new Claim("codigo", item.Empleado?.Codigo ?? ""),
                new Claim(ClaimTypes.Role, RoleType.Administrador.ToString()),
                new Claim("derechos", string.Join(",", derechos))
            };

            // Generating token payload
            var payload = new JwtPayload(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.Now.AddDays(TOKEN_EXPIRED_DAYS)
            );
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        } // GenerateToken

        private RefreshToken GenerateRefreshToken()
        {
            var item = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(TOKEN_EXPIRED_DAYS),
                Created = DateTime.Now
            };

            return item;
        } // GenerateRefreshToken

        private async Task SetRefreshTokenAsync(Usuario usuario, RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append(REFRESH_TOKEN_NAME, newRefreshToken.Token, cookieOptions);

            await _usuarioServices.UpdateTokenDataAsync(usuario.ID, newRefreshToken);
        }
    }
}
