using RegistroEstudiantil.Application.DTOs.Auth;
using RegistroEstudiantil.Application.DTOs.Auth;

namespace RegistroEstudiantil.Application.Services.Interfaces
{
    public interface IUsuarioApplicationService
    {
        Task<LoginResponseDTO> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto);
        CurrentUserDto GetCurrentUser();
    }
}


