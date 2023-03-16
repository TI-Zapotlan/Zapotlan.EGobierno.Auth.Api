using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;

namespace Zapotlan.EGobierno.Auth.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        public async Task<IEnumerable<Usuario>> Gets()
        {
            return await _usuarioRepository.Gets();
        }

        public async Task<Usuario?> Get(Guid id)
        {
            return await _usuarioRepository.Get(id);
        }

        public async Task Insert(Usuario item)
        { 
            await _usuarioRepository.Insert(item);
        }

        public async Task<bool> Update(Usuario item)
        {
            return await _usuarioRepository.Update(item);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _usuarioRepository.Delete(id);
        }
    }
}
