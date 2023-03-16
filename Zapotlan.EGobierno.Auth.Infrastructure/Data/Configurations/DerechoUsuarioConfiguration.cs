using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Data.Configurations
{
    public class DerechoUsuarioConfiguration : IEntityTypeConfiguration<DerechoUsuario>
    {
        public void Configure(EntityTypeBuilder<DerechoUsuario> builder)
        {
            builder.ToTable("UsuarioDerechos");

            builder.Property(e => e.DerechoID)
                .HasColumnName("IdDerecho")
                .IsRequired();

            builder.Property(e => e.UsuarioID)
                .HasColumnName("IdUsuario")
                .IsRequired();
        }
    }
}
