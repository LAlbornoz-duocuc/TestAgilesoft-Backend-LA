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
    public class TareasUsuarios_Configuration : IEntityTypeConfiguration<TareasUsuario>
    {
        public void Configure(EntityTypeBuilder<TareasUsuario> builder)
        {

            builder.ToTable("TareasUsuario", "dbo");

            builder.Property(e => e.Id).HasColumnName("Id");

            builder.Property(e => e.UsuarioId).HasColumnType("int").HasColumnName("UsuarioId");

            builder.Property(e => e.TareasId).HasColumnType("int").HasColumnName("TareasId");
        }
    }
}
