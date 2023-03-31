﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;

namespace Zapotlan.EGobierno.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServices;
        private readonly IConfiguration _configuration;

        public TokenController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioServices = usuarioService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Autentication(UsuarioLoginDto login)
        {
            // Validar el usuario
            var usuario = await _usuarioServices.LoginAsync(login.Username, login.Password);

            if (usuario != null)
            {
                var token = GenerateToken(usuario);
                return Ok(new { token });
            }

            return NotFound();
        }

        private string GenerateToken(Usuario item)
        {
            // Generating token header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Generating token claims
            var claims = new[]
            {
                new Claim("idx", item.ID.ToString()),
                new Claim("usr", item.Username ?? "(null)")
                //new Claim(ClaimTypes.Role, "Admin")
            };

            // Generating token payload
            var payload = new JwtPayload(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(20)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
