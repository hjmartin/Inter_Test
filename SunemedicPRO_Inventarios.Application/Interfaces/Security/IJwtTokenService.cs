using SunemedicPRO_Inventarios.Server.DTOs.Auth;
using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.Security.IReository
{
    public interface IJwtTokenService
    {
        LoginResponseDTO CreateToken(Usuario u);
    }
}
