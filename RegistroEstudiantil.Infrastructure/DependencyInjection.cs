using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistroEstudiantil.Infrastructure.Repository;
using RegistroEstudiantil.Application.Common.Security;
using RegistroEstudiantil.Infrastructure.Data;
using RegistroEstudiantil.Infrastructure.Security;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Infrastructure.Security;
using RegistroEstudiantil.Application.Interfaces.Security;

namespace RegistroEstudiantil.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}


