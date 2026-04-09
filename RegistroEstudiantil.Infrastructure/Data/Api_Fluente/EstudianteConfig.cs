using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Infrastructure.Data.Api_Fluente
{
    public class EstudianteConfig : IEntityTypeConfiguration<Estudiante>
    {
        public void Configure(EntityTypeBuilder<Estudiante> e)
        {
            e.ToTable("Estudiantes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Documento).IsRequired().HasMaxLength(30);
            e.HasIndex(x => x.Documento).IsUnique();
            e.Property(x => x.Nombres).IsRequired().HasMaxLength(120);
            e.Property(x => x.Apellidos).IsRequired().HasMaxLength(120);
            e.Property(x => x.FechaRegistro).HasColumnType("datetime2");

            e.HasOne(x => x.Usuario)
                .WithOne(x => x.Estudiante)
                .HasForeignKey<Estudiante>(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}


