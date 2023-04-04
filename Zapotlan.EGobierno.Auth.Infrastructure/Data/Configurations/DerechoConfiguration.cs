using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Data.Configurations
{
    internal class DerechoConfiguration : IEntityTypeConfiguration<Derecho>
    {
        public void Configure(EntityTypeBuilder<Derecho> builder)
        {
            builder.ToTable("Derechos");

            builder.HasKey(e => e.DerechoID);
            builder.Property(e => e.DerechoID)
                .HasColumnName("IdDerecho")
                .IsRequired();

            builder.Property(e => e.Nombre)
                .HasMaxLength(100);

            builder.Property(e => e.Descripcion)
                .HasMaxLength(500);

            builder.Property(e => e.Acceso)
                .IsRequired();

            builder.Property(e => e.FechaActualizacion)
                .IsRequired();

            builder.Property(e => e.UsuarioActualizacionID)
                .HasColumnName("IdUsuarioActualizacion")
                .IsRequired();

            builder.Ignore(e => e.ID);
            //builder.Ignore(e => e.UsuarioActualizacion);

            // RELATIONS

            builder.HasOne(e => e.UsuarioActualizacion)
                .WithMany()
                .HasForeignKey("UsuarioActualizacionID");
        }
    }
}
