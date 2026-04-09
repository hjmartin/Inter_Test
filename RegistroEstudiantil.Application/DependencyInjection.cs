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
            services.AddScoped<IUsuarioApplicationService, UsuarioApplicationService>();
            services.AddScoped<IEstudianteApplicationService, EstudianteApplicationService>();
            services.AddScoped<IInscripcionApplicationService, InscripcionApplicationService>();

            return services;
        }
    }
}


