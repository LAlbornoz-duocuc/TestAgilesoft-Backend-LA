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
    public class Usuario_Configuration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario", "dbo");

            builder.Property(e => e.Id).HasColumnName("Id");

            builder.Property(e => e.Username).HasMaxLength(50).HasColumnName("Username");

            builder.Property(e => e.Password).HasMaxLength(250).HasColumnName("Password");
        }
    }
}
