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
    public class GrupoUsuarioConfiguration : IEntityTypeConfiguration<GrupoUsuario>
    {
        public void Configure(EntityTypeBuilder<GrupoUsuario> builder)
        {
            builder.ToTable("GrupoUsuarios");

            builder.Property(e => e.GrupoID)
                .HasColumnName("IdGrupo")
                .IsRequired();

            builder.Property(e => e.UsuarioID)
                .HasColumnName("IdUsuario")
                .IsRequired();
        }
    }
}
