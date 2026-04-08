using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunemedicPRO_Inventarios.Infrastructure.Repository;
using SunemedicPRO_Inventarios.Server.Application.Common.Security;
using SunemedicPRO_Inventarios.Server.Data;
using SunemedicPRO_Inventarios.Server.Infrastructure.Security;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;
using SunemedicPRO_Inventarios.Server.Security;
using SunemedicPRO_Inventarios.Server.Security.IReository;

namespace SunemedicPRO_Inventarios.Infrastructure
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
