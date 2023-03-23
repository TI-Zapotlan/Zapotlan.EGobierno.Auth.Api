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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataCenterContext _context;

        private readonly IRepository<Area> _areaRepository;
        private readonly IRepository<Persona> _personaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public UnitOfWork(DataCenterContext context)
        {
            _context = context; 
        }

        public IRepository<Area> AreaRepository => _areaRepository ?? new BaseRepository<Area>(_context);

        public IRepository<Persona> PersonaRepository => _personaRepository ?? new BaseRepository<Persona>(_context);

        public IUsuarioRepository UsuarioRepository => _usuarioRepository ?? new UsuarioRepository(_context);

        public void Dispose() => _context?.Dispose();        

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
