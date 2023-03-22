using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(e => e.ID);
            builder.Property(e => e.ID)
                .HasColumnName("IdUsuario")
                .IsRequired();

            builder.Property(e => e.PersonaID)
                .HasColumnName("IdPersona")
                .IsRequired();

            builder.Property(e => e.AreaID)
                .HasColumnName("IdArea");

            builder.Property(e => e.EmpleadoID)
                .HasColumnName("IdEmpleado");

            builder.Property(e => e.UsuarioJefeID)
                .HasColumnName("IdUsuarioJefe");

            builder.Property(e => e.Username)
                .HasColumnName("NickName")
                .HasMaxLength(25);

            builder.Property(e => e.Password)
                .HasMaxLength(32);

            builder.Property(e => e.Correo)
                .HasMaxLength(255);

            builder.Property(e => e.Puesto)
                .HasMaxLength(150);

            builder.Property(e => e.Estatus)
                .HasColumnName("Status");

            builder.Property(e => e.FechaAlta)
                .IsRequired();

            builder.Property(e => e.ArchivoCartaResponsabilidad)
                .HasMaxLength(255);

            builder.Property(e => e.FechaActualizacion)
                .IsRequired();

            builder.Property(e => e.UsuarioActualizacionID)
                .HasColumnName("IdUsuarioActualizacion")
                .IsRequired();

            builder.HasOne(e => e.Area)
                .WithMany(a => a.Usuarios)
                .HasForeignKey(e => e.AreaID);

            builder.HasOne(e => e.Empleado)
                .WithOne()
                .HasForeignKey<Usuario>(e => e.EmpleadoID);

            builder.HasOne(e => e.Persona)
                .WithOne()
                .HasForeignKey<Usuario>(e => e.PersonaID);

            builder.HasOne(e => e.UsuarioActualizacion)
                .WithOne()
                .HasForeignKey<Usuario>(e => e.UsuarioActualizacionID);

            builder.HasMany(e => e.Grupos)
                .WithMany(g => g.Usuarios)
                .UsingEntity<GrupoUsuario>(
                    gu => gu.HasOne(prop => prop.Grupo)
                        .WithMany()
                        .HasForeignKey(prop => prop.GrupoID),
                    gu => gu.HasOne(prop => prop.Usuario)
                        .WithMany()
                        .HasForeignKey(prop => prop.UsuarioID),
                    gu => {
                        gu.HasKey(prop => new { prop.GrupoID, prop.UsuarioID });
                    }
                );

            builder.HasMany(e => e.Derechos)
                .WithMany(g => g.Usuarios)
                .UsingEntity<DerechoUsuario>(
                    gu => gu.HasOne(prop => prop.Derecho)
                        .WithMany()
                        .HasForeignKey(prop => prop.DerechoID),
                    gu => gu.HasOne(prop => prop.Usuario)
                        .WithMany()
                        .HasForeignKey(prop => prop.UsuarioID),
                    gu => {
                        gu.HasKey(prop => new { prop.DerechoID, prop.UsuarioID });
                    }
                );
        }
    }
}
