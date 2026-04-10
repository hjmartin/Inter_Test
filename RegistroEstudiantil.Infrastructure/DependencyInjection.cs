using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistroEstudiantil.Infrastructure.Repository;
using RegistroEstudiantil.Infrastructure.Data;
using RegistroEstudiantil.Infrastructure.Security;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Application.Interfaces.Security;

namespace RegistroEstudiantil.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AgregarInfraestructura(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Scoped crea una instancia por petición HTTP.
            // Aquí conviene porque repositorios, UnitOfWork y servicios de seguridad
            // trabajan con el mismo contexto y datos del usuario actual durante la request.
            // Transient se usaría cuando el servicio es liviano y sin estado compartido,
            // por ejemplo validadores, helpers o formateadores que pueden crearse en cada uso.
            // Singleton se usaría cuando el servicio debe ser único para toda la aplicación,
            // por ejemplo una cache global en memoria o un proveedor de configuración compartida,
            // siempre que no dependa de DbContext, HttpContext ni datos del usuario actual.
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEstudianteRepository, EstudianteRepository>();
            services.AddScoped<IGrupoClaseRepository, GrupoClaseRepository>();
            services.AddScoped<IInscripcionRepository, InscripcionRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}


