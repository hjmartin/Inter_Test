using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SunemedicPRO_Inventarios.Server.Application.Common.Exceptions;
using SunemedicPRO_Inventarios.Server.Application.Common.Security;
using SunemedicPRO_Inventarios.Server.Application.DTOs.Auth;
using SunemedicPRO_Inventarios.Server.Application.Services.Interfaces;
using SunemedicPRO_Inventarios.Server.DTOs.Auth;
using SunemedicPRO_Inventarios.Server.Entities;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;
using SunemedicPRO_Inventarios.Server.Security.IReository;

namespace SunemedicPRO_Inventarios.Server.Application.Services
{
    public class UsuarioApplicationService : IUsuarioApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICurrentUserService _currentUserService;

        public UsuarioApplicationService(
            IUnitOfWork unitOfWork,
            IJwtTokenService jwtTokenService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _currentUserService = currentUserService;
        }

        public async Task<LoginResponseDTO> RegisterAsync(RegisterDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            if (await _unitOfWork.UsuarioRepo.ExistsEmailAsync(email))
            {
                throw new ApiException(StatusCodes.Status409Conflict, "Email ya registrado.");
            }

            var usuario = new Usuario
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Pass),
                Rol = "Estudiante"
            };

            _unitOfWork.UsuarioRepo.Add(usuario);
            await _unitOfWork.SaveAsync();

            return _jwtTokenService.CreateToken(usuario);
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            var usuario = await _unitOfWork.UsuarioRepo.GetByEmailAsync(email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Pass, usuario.PasswordHash))
            {
                var errores = new List<IdentityError>
                {
                    new()
                    {
                        Description = "Credenciales incorrectas"
                    }
                };

                throw new ApiException(StatusCodes.Status400BadRequest, "Credenciales incorrectas.", errores);
            }

            return _jwtTokenService.CreateToken(usuario);
        }

        public CurrentUserDto GetCurrentUser()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new ApiException(StatusCodes.Status401Unauthorized, "Usuario no autenticado.");
            }

            return new CurrentUserDto
            {
                Id = _currentUserService.UserId.Value,
                Email = _currentUserService.Email,
                Rol = _currentUserService.Role
            };
        }
    }
}
