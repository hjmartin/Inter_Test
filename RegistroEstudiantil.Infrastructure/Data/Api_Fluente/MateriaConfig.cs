using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Infrastructure.Data.Api_Fluente
{
    public class MateriaConfig : IEntityTypeConfiguration<Materia>
    {
        public void Configure(EntityTypeBuilder<Materia> e)
        {
            e.ToTable("Materias");
            e.HasKey(x => x.Id);
            e.Property(x => x.Codigo).IsRequired().HasMaxLength(20);
            e.HasIndex(x => x.Codigo).IsUnique();
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(160);
            e.Property(x => x.Creditos).HasDefaultValue(3);

            // Seed: 10 materias, 3 créditos c/u
            e.HasData(
                new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas I", Creditos = 3 },
                new Materia { Id = 2, Codigo = "PRO101", Nombre = "Programación I", Creditos = 3 },
                new Materia { Id = 3, Codigo = "BD101", Nombre = "Bases de Datos", Creditos = 3 },
                new Materia { Id = 4, Codigo = "ING101", Nombre = "Inglés I", Creditos = 3 },
                new Materia { Id = 5, Codigo = "FIS101", Nombre = "Física I", Creditos = 3 },
                new Materia { Id = 6, Codigo = "EST101", Nombre = "Estadística", Creditos = 3 },
                new Materia { Id = 7, Codigo = "RED101", Nombre = "Redes I", Creditos = 3 },
                new Materia { Id = 8, Codigo = "SOF101", Nombre = "Ingeniería de Software", Creditos = 3 },
                new Materia { Id = 9, Codigo = "SIS101", Nombre = "Sistemas Operativos", Creditos = 3 },
                new Materia { Id = 10, Codigo = "ETI101", Nombre = "Ética Profesional", Creditos = 3 }
            );
        }
    }
    }


