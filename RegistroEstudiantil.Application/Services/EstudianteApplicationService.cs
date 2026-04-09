using AutoMapper;
using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Application.Common.Models;
using RegistroEstudiantil.Application.Common.Security;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.DTOs.Shared;
using RegistroEstudiantil.Application.Interfaces.Persistence;
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

        public async Task<PagedResult<EstudianteDTO>> GetCurrentAsync(PaginacionDTO paginacionDTO)
        {
            var userId = GetRequiredUserId();
            var estudiante = (await _unitOfWork.Repository<Estudiante>().ListAsync(e => e.UsuarioId == userId)).FirstOrDefault();

            if (estudiante is null)
            {
                return new PagedResult<EstudianteDTO>();
            }

            var estudiantes = await _unitOfWork.Repository<Estudiante>().ListAsync(x => x.Id == estudiante.Id);
            var ordenados = estudiantes.OrderBy(x => x.Nombres).ToList();
            var items = ordenados
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina)
                .Take(paginacionDTO.RecordsPorPagina)
                .ToList();

            return new PagedResult<EstudianteDTO>
            {
                Items = _mapper.Map<List<EstudianteDTO>>(items),
                TotalCount = ordenados.Count
            };
        }

        public async Task<IReadOnlyList<EstudianteDTO>> GetAllAsync()
        {
            var estudiantes = await _unitOfWork.Repository<Estudiante>().ListAsync();
            var ordenados = estudiantes.OrderBy(x => x.Nombres).ToList();
            return _mapper.Map<List<EstudianteDTO>>(ordenados);
        }

        public async Task<EstudianteDTO> CreateAsync(EstudianteCreacionDTO dto)
        {
            var userId = GetRequiredUserId();
            if (await _unitOfWork.Repository<Estudiante>().AnyAsync(e => e.UsuarioId == userId))
            {
                throw new ConflictException("El usuario ya tiene perfil de estudiante.");
            }

            var estudiante = _mapper.Map<Estudiante>(dto);
            estudiante.UsuarioId = userId;

            _unitOfWork.Repository<Estudiante>().Add(estudiante);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<EstudianteDTO>(estudiante);
        }

        public async Task UpdateAsync(int id, EstudianteUpdateDTO dto)
        {
            var userId = GetRequiredUserId();
            var estudiante = await _unitOfWork.Repository<Estudiante>().GetByIdAsync(id);
            if (estudiante is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
            }

            if (!estudiante.PerteneceAUsuario(userId))
            {
                throw new ForbiddenException("No puedes modificar este perfil.");
            }

            _mapper.Map(dto, estudiante);
            _unitOfWork.Repository<Estudiante>().UpdateAsync(estudiante);
            await _unitOfWork.SaveAsync();
        }

        public async Task<EstudianteDTO> GetByIdAsync(int id)
        {
            var estudiante = await _unitOfWork.Repository<Estudiante>().GetByIdAsync(id);
            if (estudiante is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
            }

            return _mapper.Map<EstudianteDTO>(estudiante);
        }

        public async Task DeleteAsync(int id)
        {
            var userId = GetRequiredUserId();
            var estudiante = await _unitOfWork.Repository<Estudiante>().GetByIdAsync(id);
            if (estudiante is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
            }

            if (!estudiante.PerteneceAUsuario(userId))
            {
                throw new ForbiddenException("No puedes eliminar este perfil.");
            }

            var tieneInscripciones = await _unitOfWork.InscripcionRepo.AnyAsync(i => i.EstudianteId == id);
            if (tieneInscripciones)
            {
                throw new ConflictException("No puedes eliminar tu perfil porque tienes inscripciones activas.");
            }

            _unitOfWork.Repository<Estudiante>().Remove(estudiante);
            await _unitOfWork.SaveAsync();
        }

        private int GetRequiredUserId()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedException("Usuario no autenticado.");
            }

            return _currentUserService.UserId.Value;
        }
    }
}
