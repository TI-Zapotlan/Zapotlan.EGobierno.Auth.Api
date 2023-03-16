using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Data.Configurations
{
    internal class PersonaConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Personas");
            builder.HasKey(e => e.ID);

            builder.Property(e => e.ID)
                .HasColumnName("IdPersona")
                .IsRequired();

            builder.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.PrimerApellido)
                .HasColumnName("ApellidoPaterno")
                .HasMaxLength(50);

            builder.Property(e => e.SegundoApellido)
                .HasColumnName("ApellidoMaterno")
                .HasMaxLength(50);

            builder.Property(e => e.CURP)
                .HasMaxLength(18);

            builder.Property(e => e.EstadoVida)
                .IsRequired();

            builder.Property(e => e.FechaActualizacion)
                .IsRequired();

            builder.Property(e => e.UsuarioActualizacionID)
                .HasColumnName("IdUsuarioActualizacion")
                .IsRequired();
        }
    }
}
