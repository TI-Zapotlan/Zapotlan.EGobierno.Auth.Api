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
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataCenterContext _context;
        protected DbSet<T> _entity;

        public BaseRepository(DataCenterContext context)
        {
            _context = context; 
            _entity = context.Set<T>();
        }

        public virtual IEnumerable<T> Gets()
        {
            return _entity.AsEnumerable(); // Para que no llame a la base de datos y se puedan anexar filtros
        }

        public virtual async Task<T?> GetAsync(Guid id) 
        {
            return await _entity.FindAsync(id);
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _entity.FindAsync(id);
        }

        public virtual async Task AddAsync(T item)
        {
            await _entity.AddAsync(item);
            // await _context.SaveChangesAsync();
        }

        public virtual void Update(T item) // la sincronia se va a hacer en el unitOfWork
        {
            _entity.Update(item);
            // await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            T? item = await GetAsync(id);
            if (item != null)
            {
                _entity.Remove(item);
                // await _context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            T? item = await GetAsync(id);
            if (item != null)
            {
                _entity.Remove(item);
                // await _context.SaveChangesAsync();
            }
        }
    }
}
