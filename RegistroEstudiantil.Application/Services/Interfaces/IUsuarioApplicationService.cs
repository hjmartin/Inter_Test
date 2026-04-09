using RegistroEstudiantil.Application.DTOs.Auth;

namespace RegistroEstudiantil.Application.Services.Interfaces
{
    public interface IUsuarioApplicationService
    {
        Task<LoginResponseDTO> RegistrarAsync(RegisterDto dto);
        Task<LoginResponseDTO> IniciarSesionAsync(LoginRequestDTO dto);
        CurrentUserDto ObtenerUsuarioActual();
    }
}
