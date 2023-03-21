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
    internal class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.ToTable("Empleados");
            builder.HasKey(e => e.ID);

            builder.Property(e => e.PersonaID)
                .HasColumnName("IdPersona")
                .IsRequired();

            builder.Property(e => e.AreaID)
                .HasColumnName("IdArea")
                .IsRequired();

            builder.Property(e => e.AreaComisionID)
                .HasColumnName("IdAreaComision");

            builder.Property(e => e.Codigo)
                .HasMaxLength(20);

            builder.Property(e => e.TipoTrabajador)
                .HasMaxLength(40);

            builder.Property(e => e.ClavePuesto)
                .HasMaxLength(13);

            builder.Property(e => e.NombrePuesto)
                .HasMaxLength(50);

            builder.Property(e => e.SueldoDiario)
                .HasColumnType("smallmoney");

            builder.Property(e => e.ModalidadIMSS)
                .HasMaxLength(10);

            builder.Property(e => e.TipoNomina)
                .HasMaxLength(50);
        }
    }
}
