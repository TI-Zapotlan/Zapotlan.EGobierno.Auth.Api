using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Zapotlan.EGobierno.Auth.Api.Responses;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Enumerations;
using Zapotlan.EGobierno.Auth.Core.Interfaces;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServices;
        private readonly IConfiguration _configuration;

        // CONSTRUCTOR 

        public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioServices = usuarioService;
            _configuration = configuration;
        }

        // ENDPOINTS

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
        public async Task<IActionResult> Autentication(UsuarioLoginDto login)
        {
            // Validar el usuario
            var usuario = await _usuarioServices.LoginAsync(login.Username, login.Password);

            if (usuario != null)
            {
                var token = GenerateToken(usuario, login.DerechosInicio, login.DerechosTermino);
                var response = new ApiResponse<string>(token);

                return Ok(response);
                //return Ok(new { token });
            }

            return NotFound();
        }

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
                // if (!string.IsNullOrEmpty(item.Persona.PrimerApellido))
                givename += !string.IsNullOrEmpty(item.Persona.PrimerApellido) ? " " + item.Persona.PrimerApellido : "";
                givename += !string.IsNullOrEmpty(item.Persona.SegundoApellido) ? " " + item.Persona.SegundoApellido : "";
            }

            // Generating token claim
            var claims = new[]
            {
                new Claim("idx", item.ID.ToString()),
                new Claim("username", item.Username ?? "(null)"),
                new Claim("givename", givename),
                new Claim(ClaimTypes.Role, RoleType.Administrador.ToString()),
                new Claim("derechos", string.Join(",", derechos))
            };

            // Generating token payload
            var payload = new JwtPayload(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddDays(1)
            );

            var token = new JwtSecurityToken(header, payload);

            //var token = new JwtSecurityToken(
            //    claims: claims,
            //    expires: DateTime.Now.AddDays(1),
            //    signingCredentials: signingCredentials
            //    );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
