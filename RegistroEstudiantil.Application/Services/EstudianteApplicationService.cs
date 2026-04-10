using AutoMapper;
using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Application.Interfaces.Security;
using RegistroEstudiantil.Application.Services.Interfaces;
using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.Services
{
    public class EstudianteApplicationService : IEstudianteApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EstudianteApplicationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyList<EstudianteDTO>> ObtenerActualAsync()
        {
            var userId = ObtenerIdUsuarioRequerido();
            var estudiantes = await _unitOfWork.EstudianteRepo.ObtenerPorUsuarioAsync(userId);
            return _mapper.Map<List<EstudianteDTO>>(estudiantes);
        }

        public async Task<IReadOnlyList<EstudianteDTO>> ObtenerTodosAsync()
        {
            var estudiantes = await _unitOfWork.EstudianteRepo.ObtenerTodosOrdenadosAsync();
            return _mapper.Map<List<EstudianteDTO>>(estudiantes);
        }

        public async Task<EstudianteDTO> CrearAsync(EstudianteCreacionDTO dto)
        {
            var userId = ObtenerIdUsuarioRequerido();
            if (await _unitOfWork.EstudianteRepo.ExistePorUsuarioIdAsync(userId))
            {
                throw new ConflictException("El usuario ya tiene perfil de estudiante.");
            }

            var estudiante = _mapper.Map<Estudiante>(dto);
            estudiante.UsuarioId = userId;

            _unitOfWork.EstudianteRepo.Agregar(estudiante);
            await _unitOfWork.GuardarCambiosAsync();

            return _mapper.Map<EstudianteDTO>(estudiante);
        }

        public async Task ActualizarAsync(int id, EstudianteUpdateDTO dto)
        {
            var userId = ObtenerIdUsuarioRequerido();
            var estudiante = await _unitOfWork.EstudianteRepo.ObtenerPorIdAsync(id);
            if (estudiante is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
            }

            if (!estudiante.PerteneceAUsuario(userId))
            {
                throw new ForbiddenException("No puedes modificar este perfil.");
            }

            _mapper.Map(dto, estudiante);
            _unitOfWork.EstudianteRepo.Actualizar(estudiante);
            await _unitOfWork.GuardarCambiosAsync();
        }

        public async Task<EstudianteDTO> ObtenerPorIdAsync(int id)
        {
            var estudiante = await _unitOfWork.EstudianteRepo.ObtenerPorIdAsync(id);
            if (estudiante is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
            }

            return _mapper.Map<EstudianteDTO>(estudiante);
        }

        public async Task EliminarAsync(int id)
        {
            var userId = ObtenerIdUsuarioRequerido();
            var estudiante = await _unitOfWork.EstudianteRepo.ObtenerPorIdAsync(id);
            if (estudiante is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
            }

            if (!estudiante.PerteneceAUsuario(userId))
            {
                throw new ForbiddenException("No puedes eliminar este perfil.");
            }

            var tieneInscripciones = await _unitOfWork.InscripcionRepo.ExisteAlgunaPorEstudianteAsync(id);
            if (tieneInscripciones)
            {
                throw new ConflictException("No puedes eliminar tu perfil porque tienes inscripciones activas.");
            }

            _unitOfWork.EstudianteRepo.Eliminar(estudiante);
            await _unitOfWork.GuardarCambiosAsync();
        }

        private int ObtenerIdUsuarioRequerido()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedException("Usuario no autenticado.");
            }

            return _currentUserService.UserId.Value;
        }
    }
}
