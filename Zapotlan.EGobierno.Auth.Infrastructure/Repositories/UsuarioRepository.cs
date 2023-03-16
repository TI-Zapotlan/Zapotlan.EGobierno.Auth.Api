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

        public async Task<Usuario> Get(Guid id)
        {
            var item = await _context.Usuarios.FirstOrDefaultAsync(i => i.ID == id);
            return item;
        }

        public async Task Insert(Usuario item)
        {
            _context.Usuarios.Add(item);
            await _context.SaveChangesAsync();
        }
    }
}
