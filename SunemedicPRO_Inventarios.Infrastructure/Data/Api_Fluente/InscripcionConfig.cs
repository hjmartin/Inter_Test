using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.Data.Api_Fluente
{
    public class InscripcionConfig : IEntityTypeConfiguration<Inscripcion>
    {
        public void Configure(EntityTypeBuilder<Inscripcion> e)
        {
            e.ToTable("Inscripciones");
            e.HasKey(x => x.Id);

            e.Property(x => x.Fecha).HasColumnType("datetime2");
            e.Property(x => x.Periodo).IsRequired().HasMaxLength(20);

            // Evita duplicar una misma inscripción
            e.HasIndex(x => new { x.EstudianteId, x.GrupoClaseId }).IsUnique();

            e.HasOne(x => x.Estudiante)
                .WithMany(x => x.Inscripciones)
                .HasForeignKey(x => x.EstudianteId);

            e.HasOne(x => x.GrupoClase)
                .WithMany(x => x.Inscripciones)
                .HasForeignKey(x => x.GrupoClaseId);
        }
    }
}
