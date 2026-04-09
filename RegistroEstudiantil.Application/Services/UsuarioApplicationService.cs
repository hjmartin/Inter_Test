using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Application.Common.Security;
using RegistroEstudiantil.Application.DTOs.Auth;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Application.Interfaces.Security;
using RegistroEstudiantil.Application.Services.Interfaces;
using RegistroEstudiantil.Domain.Entities;
using System.Data;

namespace RegistroEstudiantil.Application.Services
{
    public class UsuarioApplicationService : IUsuarioApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPasswordHasher _passwordHasher;

        public UsuarioApplicationService(
            IUnitOfWork unitOfWork,
            IJwtTokenService jwtTokenService,
            ICurrentUserService currentUserService,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _currentUserService = currentUserService;
            _passwordHasher = passwordHasher;
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
                    throw new ConflictException("Email ya registrado.");
                }

                if (await _unitOfWork.Repository<Estudiante>().AnyAsync(e => e.Documento == documento))
                {
                    throw new ConflictException("El documento ya esta registrado.");
                }

                usuario = Usuario.CrearEstudiante(email, _passwordHasher.Hash(dto.Pass));
                var estudiante = Estudiante.Crear(documento, nombres, apellidos, usuario);

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

            if (usuario is null || !_passwordHasher.Verify(dto.Pass, usuario.PasswordHash))
            {
                throw new ValidationException(
                    "Credenciales incorrectas.",
                    new { errores = new[] { "Credenciales incorrectas" } });
            }

            return _jwtTokenService.CreateToken(usuario);
        }

        public CurrentUserDto GetCurrentUser()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedException("Usuario no autenticado.");
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
