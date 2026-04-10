using Microsoft.Extensions.DependencyInjection;
using RegistroEstudiantil.Application.Services;
using RegistroEstudiantil.Application.Services.Interfaces;
using RegistroEstudiantil.Application.Utilidades;

namespace RegistroEstudiantil.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AgregarAplicacion(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            // Scoped mantiene una instancia por request, suficiente para los casos de uso
            // y consistente con los repositorios y el UnitOfWork que también son Scoped.
            // Transient serviría para componentes muy simples y sin estado compartido,
            // por ejemplo transformadores o validaciones puntuales que no dependen de la request.
            // Singleton no conviene aquí porque estos servicios dependen de componentes por request;
            // un Singleton sería más apropiado para objetos globales como caches o configuración compartida.
            services.AddScoped<IUsuarioApplicationService, UsuarioApplicationService>();
            services.AddScoped<IEstudianteApplicationService, EstudianteApplicationService>();
            services.AddScoped<IInscripcionApplicationService, InscripcionApplicationService>();

            return services;
        }
    }
}


