using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Data.Configurations
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ToTable("Areas");
            builder.HasKey(e => e.ID);

            builder.Property(e => e.ID)
                .HasColumnName("IdArea");

            builder.Property(e => e.AreaPadreID)
                .HasColumnName("IdAreaPadre");

            builder.Property(e => e.Clave)
                .HasMaxLength(25);

            builder.Property(e => e.Abreviacion)
                .HasMaxLength(10);

            builder.Property(e => e.Nombre)
                .HasMaxLength(100);

            builder.Property(e => e.NombreNomina)
                .HasMaxLength(100);

            builder.Property(e => e.Descripcion)
                .HasMaxLength(100);

            builder.Property(e => e.Activo)
                .IsRequired();

            builder.Property(e => e.Tipo)
                .IsRequired();

            builder.Property(e => e.FechaAlta)
                .IsRequired();

            builder.Property(e => e.FechaActualizacion)
                .IsRequired();

            builder.Property(e => e.UsuarioActualizacionID)
                .HasColumnName("IdUsuarioActualizacion")
                .IsRequired();

            builder.HasOne(e => e.AreaPadre)
                .WithMany(e => e.AreasHijo)
                .HasForeignKey(e => e.AreaPadreID);

            builder.HasMany(e => e.Empleados)
                .WithOne(e => e.Area)
                .HasForeignKey(e => e.AreaID);
        }
    }
}
