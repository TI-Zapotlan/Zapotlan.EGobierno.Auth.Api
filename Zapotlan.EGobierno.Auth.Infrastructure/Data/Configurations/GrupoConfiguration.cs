using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Data.Configurations
{
    public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("Grupos");

            builder.HasKey(e => e.ID);
            builder.Property(e => e.ID)
                .HasColumnName("IdGrupo")
                .IsRequired();

            builder.Property(e => e.Nombre)
                .HasMaxLength(50);

            builder.Property(e => e.Descripcion)
                .HasMaxLength(250);

            builder.Property(e => e.FechaActualizacion)
                .IsRequired();

            builder.Property(e => e.UsuarioActualizacionID)
                .HasColumnName("IdUsuarioActualizacion")
                .IsRequired();

            // RELATIONS

            builder.HasOne(e => e.UsuarioActualizacion)
                .WithMany()
                .HasForeignKey("UsuarioActualizacionID");

            builder.HasMany(e => e.Derechos)
                .WithMany(g => g.Grupos)
                .UsingEntity<DerechoGrupo>(
                    gu => gu.HasOne(prop => prop.Derecho)
                        .WithMany()
                        .HasForeignKey(prop => prop.DerechoID),
                    gu => gu.HasOne(prop => prop.Grupo)
                        .WithMany()
                        .HasForeignKey(prop => prop.GrupoID),
                    gu => {
                        gu.HasKey(prop => new { prop.DerechoID, prop.GrupoID });
                    }
                );
        }
    }
}
