using SunemedicPRO_Inventarios.Server.Application.DTOs.Auth;
using SunemedicPRO_Inventarios.Server.DTOs.Auth;

namespace SunemedicPRO_Inventarios.Server.Application.Services.Interfaces
{
    public interface IUsuarioApplicationService
    {
        Task<LoginResponseDTO> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto);
        CurrentUserDto GetCurrentUser();
    }
}
