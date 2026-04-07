using Microsoft.Extensions.DependencyInjection;
using SunemedicPRO_Inventarios.Server.Application.Services;
using SunemedicPRO_Inventarios.Server.Application.Services.Interfaces;
using SunemedicPRO_Inventarios.Server.Utilidades;

namespace SunemedicPRO_Inventarios.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddScoped<IUsuarioApplicationService, UsuarioApplicationService>();
            services.AddScoped<IEstudianteApplicationService, EstudianteApplicationService>();
            services.AddScoped<IInscripcionApplicationService, InscripcionApplicationService>();

            return services;
        }
    }
}
