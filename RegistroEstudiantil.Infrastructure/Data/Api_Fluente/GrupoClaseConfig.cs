using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Infrastructure.Data.Api_Fluente
{
    public class GrupoClaseConfig : IEntityTypeConfiguration<GrupoClase>
    {
        public void Configure(EntityTypeBuilder<GrupoClase> e)
        {
            e.ToTable("Grupos");
            e.HasKey(x => x.Id);

            e.Property(x => x.Periodo).IsRequired().HasMaxLength(20);
            e.Property(x => x.NombreGrupo).HasMaxLength(10);
            e.Property(x => x.Cupo).HasDefaultValue(40);

            e.HasIndex(x => new { x.MateriaId, x.ProfesorId, x.Periodo, x.NombreGrupo }).IsUnique();

            e.HasOne(x => x.Materia)
                .WithMany(x => x.Grupos)
                .HasForeignKey(x => x.MateriaId);

            e.HasOne(x => x.Profesor)
                .WithMany(x => x.Grupos)
                .HasForeignKey(x => x.ProfesorId);

            // Seed: un grupo "A" por materia en 2025-2
            e.HasData(
                new GrupoClase { Id = 1, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 1, ProfesorId = 1, Cupo = 40 },
                new GrupoClase { Id = 2, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 2, ProfesorId = 1, Cupo = 40 },
                new GrupoClase { Id = 3, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 3, ProfesorId = 2, Cupo = 40 },
                new GrupoClase { Id = 4, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 4, ProfesorId = 2, Cupo = 40 },
                new GrupoClase { Id = 5, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 5, ProfesorId = 3, Cupo = 40 },
                new GrupoClase { Id = 6, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 6, ProfesorId = 3, Cupo = 40 },
                new GrupoClase { Id = 7, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 7, ProfesorId = 4, Cupo = 40 },
                new GrupoClase { Id = 8, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 8, ProfesorId = 4, Cupo = 40 },
                new GrupoClase { Id = 9, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 9, ProfesorId = 5, Cupo = 40 },
                new GrupoClase { Id = 10, Periodo = "2025-2", NombreGrupo = "A", MateriaId = 10, ProfesorId = 5, Cupo = 40 }
            );
        }
    }
}


