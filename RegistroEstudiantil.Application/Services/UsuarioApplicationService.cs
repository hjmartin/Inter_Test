using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Application.Common.Security;
using RegistroEstudiantil.Application.DTOs.Auth;
using RegistroEstudiantil.Application.Services.Interfaces;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Application.Interfaces.Security;
using System.Data;

namespace RegistroEstudiantil.Application.Services
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
            Usuario? usuario = null;
            var email = dto.Email.Trim().ToLowerInvariant();
            var documento = dto.Documento.Trim();
            var nombres = dto.Nombres.Trim();
            var apellidos = dto.Apellidos.Trim();

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                if (await _unitOfWork.UsuarioRepo.ExistsEmailAsync(email))
                {
                    throw new ApiException(StatusCodes.Status409Conflict, "Email ya registrado.");
                }

                if (await _unitOfWork.Repository<Estudiante>().AnyAsync(e => e.Documento == documento))
                {
                    throw new ApiException(StatusCodes.Status409Conflict, "El documento ya esta registrado.");
                }

                usuario = new Usuario
                {
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Pass),
                    Rol = "Estudiante"
                };

                var estudiante = new Estudiante
                {
                    Documento = documento,
                    Nombres = nombres,
                    Apellidos = apellidos,
                    Usuario = usuario
                };

                _unitOfWork.UsuarioRepo.Add(usuario);
                _unitOfWork.Repository<Estudiante>().Add(estudiante);
                await _unitOfWork.SaveAsync();
            }, IsolationLevel.ReadCommitted);

            return _jwtTokenService.CreateToken(usuario!);
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



