using RegistroEstudiantil.Application.DTOs.Auth;
using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.Interfaces.Security
{
    public interface IJwtTokenService
    {
        LoginResponseDTO CreateToken(Usuario u);
    }
}


