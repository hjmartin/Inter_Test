using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Infrastructure.Data.Api_Fluente
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> e)
        {
            e.ToTable("Usuarios");
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired().HasMaxLength(160);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.PasswordHash).IsRequired();
            e.Property(x => x.Rol).HasMaxLength(30).HasDefaultValue("Estudiante");
        }
    }
}


