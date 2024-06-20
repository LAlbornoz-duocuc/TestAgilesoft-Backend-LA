using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Config
{
    public class TareasGenerales_Configuration : IEntityTypeConfiguration<TareasGenerales>
    {
        public void Configure(EntityTypeBuilder<TareasGenerales> builder)
        {
            builder.ToTable("TareasGenerales", "dbo");

            builder.Property(e => e.Id).HasColumnName("Id");

            builder.Property(e => e.Nombre).HasMaxLength(50).HasColumnName("Nombre");

            builder.Property(e => e.Descripcion).HasMaxLength(250).HasColumnName("Descripcion");

            builder.Property(e => e.Estado).HasColumnType("int").HasColumnName("Estado");

            builder.Property(x => x.FechaCreacion).HasColumnType("datetime").HasColumnName("FechaCreacion");

            builder.Property(x => x.FechaUltimaActualizacion).HasColumnType("datetime").HasColumnName("FechaUltimaActualizacion");

        }
    }
}
