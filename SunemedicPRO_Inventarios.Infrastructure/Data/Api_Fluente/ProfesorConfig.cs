using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.Data.Api_Fluente
{
    public class ProfesorConfig : IEntityTypeConfiguration<Profesor>
    {
        public void Configure(EntityTypeBuilder<Profesor> e)
        {
            e.ToTable("Profesores");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombres).IsRequired().HasMaxLength(120);
            e.Property(x => x.Apellidos).IsRequired().HasMaxLength(120);

            // Seed: 5 profesores
            e.HasData(
                new Profesor { Id = 1, Nombres = "Ana", Apellidos = "García" },
                new Profesor { Id = 2, Nombres = "Luis", Apellidos = "Pérez" },
                new Profesor { Id = 3, Nombres = "Marta", Apellidos = "López" },
                new Profesor { Id = 4, Nombres = "Carlos", Apellidos = "Ramírez" },
                new Profesor { Id = 5, Nombres = "Sofía", Apellidos = "Torres" }
            );
        }
    }
}
