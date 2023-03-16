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
    public class DerechoGrupoConfiguration : IEntityTypeConfiguration<DerechoGrupo>
    {
        public void Configure(EntityTypeBuilder<DerechoGrupo> builder)
        {
            builder.ToTable("GrupoDerechos");

            builder.Property(e => e.DerechoID)
                .HasColumnName("IdDerecho")
                .IsRequired();

            builder.Property(e => e.GrupoID)
                .HasColumnName("IdGrupo")
                .IsRequired();
        }
    }
}
