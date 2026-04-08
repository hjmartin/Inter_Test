using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Data.Api_Fluente;
using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<GrupoClase> Grupos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Puedes mantener tus ApplyConfiguration(...) o simplificar:
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new EstudianteConfig());
            modelBuilder.ApplyConfiguration(new GrupoClaseConfig());
            modelBuilder.ApplyConfiguration(new InscripcionConfig());
            modelBuilder.ApplyConfiguration(new MateriaConfig());
            modelBuilder.ApplyConfiguration(new ProfesorConfig());
            modelBuilder.ApplyConfiguration(new UsuarioConfig());
        }
    }
}
