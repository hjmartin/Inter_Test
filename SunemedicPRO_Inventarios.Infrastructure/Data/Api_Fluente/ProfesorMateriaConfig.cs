using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.Data.Api_Fluente
{
    public class ProfesorMateriaConfig : IEntityTypeConfiguration<ProfesorMateria>
    {
        public void Configure(EntityTypeBuilder<ProfesorMateria> e)
        {
            e.ToTable("ProfesorMaterias");
            e.HasKey(x => new { x.ProfesorId, x.MateriaId });

            e.HasOne(x => x.Profesor)
                .WithMany(x => x.ProfesorMaterias)
                .HasForeignKey(x => x.ProfesorId);

            e.HasOne(x => x.Materia)
                .WithMany(x => x.ProfesorMaterias)
                .HasForeignKey(x => x.MateriaId);

            // Seed: cada profesor dicta 2 materias
            e.HasData(
                new ProfesorMateria { ProfesorId = 1, MateriaId = 1 },
                new ProfesorMateria { ProfesorId = 1, MateriaId = 2 },
                new ProfesorMateria { ProfesorId = 2, MateriaId = 3 },
                new ProfesorMateria { ProfesorId = 2, MateriaId = 4 },
                new ProfesorMateria { ProfesorId = 3, MateriaId = 5 },
                new ProfesorMateria { ProfesorId = 3, MateriaId = 6 },
                new ProfesorMateria { ProfesorId = 4, MateriaId = 7 },
                new ProfesorMateria { ProfesorId = 4, MateriaId = 8 },
                new ProfesorMateria { ProfesorId = 5, MateriaId = 9 },
                new ProfesorMateria { ProfesorId = 5, MateriaId = 10 }
            );
        }
    }
    }
