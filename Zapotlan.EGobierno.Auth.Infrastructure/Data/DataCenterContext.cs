using Microsoft.EntityFrameworkCore;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Infrastructure.Data.Configurations;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Data
{
    public class DataCenterContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<Derecho> Derechos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DataCenterContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AreaConfiguration());
            modelBuilder.ApplyConfiguration(new DerechoConfiguration());
            modelBuilder.ApplyConfiguration(new EmpleadoConfiguration());
            modelBuilder.ApplyConfiguration(new GrupoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());

            modelBuilder.ApplyConfiguration(new DerechoGrupoConfiguration());
            modelBuilder.ApplyConfiguration(new DerechoUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new GrupoUsuarioConfiguration());

        }
    }
}
