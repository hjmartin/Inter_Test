using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Application.Common.Exceptions;
using SunemedicPRO_Inventarios.Server.Application.Common.Models;
using SunemedicPRO_Inventarios.Server.Application.Common.Security;
using SunemedicPRO_Inventarios.Server.Application.Services.Interfaces;
using SunemedicPRO_Inventarios.Server.DTOs;
using SunemedicPRO_Inventarios.Server.DTOs.Shared;
using SunemedicPRO_Inventarios.Server.Entities;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;
using SunemedicPRO_Inventarios.Server.Utilidades;

namespace SunemedicPRO_Inventarios.Server.Application.Services
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
            var estudiante = (await _unitOfWork.EstudianteRepo.ListAsync(e => e.UsuarioId == userId)).FirstOrDefault();

            if (estudiante is null)
            {
                return new PagedResult<EstudianteDTO>();
            }

            var queryable = _unitOfWork.EstudianteRepo
                .GetAll()
                .Where(x => x.Id == estudiante.Id)
                .OrderBy(x => x.Nombres);

            var totalCount = await queryable.CountAsync();
            var items = await queryable
                .Paginar(paginacionDTO)
                .ProjectTo<EstudianteDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<EstudianteDTO>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<IReadOnlyList<EstudianteDTO>> GetAllAsync()
        {
            return await _unitOfWork.EstudianteRepo
                .GetAll()
                .OrderBy(x => x.Nombres)
                .ProjectTo<EstudianteDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EstudianteDTO> CreateAsync(EstudianteCreacionDTO dto)
        {
            var userId = GetRequiredUserId();
            if (await _unitOfWork.EstudianteRepo.AnyAsync(e => e.UsuarioId == userId))
            {
                throw new ApiException(StatusCodes.Status409Conflict, "El usuario ya tiene perfil de estudiante.");
            }

            var estudiante = _mapper.Map<Estudiante>(dto);
            estudiante.UsuarioId = userId;

            _unitOfWork.EstudianteRepo.Add(estudiante);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<EstudianteDTO>(estudiante);
        }

        public async Task UpdateAsync(int id, EstudianteUpdateDTO dto)
        {
            var userId = GetRequiredUserId();
            var estudiante = await _unitOfWork.EstudianteRepo.GetByIdAsync(id);
            if (estudiante is null)
            {
                throw new ApiException(StatusCodes.Status404NotFound, "Estudiante no encontrado.");
            }

            if (estudiante.UsuarioId != userId)
            {
                throw new ApiException(StatusCodes.Status403Forbidden, "No puedes modificar este perfil.");
            }

            _mapper.Map(dto, estudiante);
            _unitOfWork.EstudianteRepo.UpdateAsync(estudiante);
            await _unitOfWork.SaveAsync();
        }

        public async Task<EstudianteDTO> GetByIdAsync(int id)
        {
            var estudiante = await _unitOfWork.EstudianteRepo.GetByIdAsync(id);
            if (estudiante is null)
            {
                throw new ApiException(StatusCodes.Status404NotFound, "Estudiante no encontrado.");
            }

            return _mapper.Map<EstudianteDTO>(estudiante);
        }

        public async Task DeleteAsync(int id)
        {
            var userId = GetRequiredUserId();
            var estudiante = await _unitOfWork.EstudianteRepo.GetByIdAsync(id);
            if (estudiante is null)
            {
                throw new ApiException(StatusCodes.Status404NotFound, "Estudiante no encontrado.");
            }

            if (estudiante.UsuarioId != userId)
            {
                throw new ApiException(StatusCodes.Status403Forbidden, "No puedes eliminar este perfil.");
            }

            var tieneInscripciones = await _unitOfWork.InscripcionRepo.AnyAsync(i => i.EstudianteId == id);
            if (tieneInscripciones)
            {
                throw new ApiException(StatusCodes.Status409Conflict, "No puedes eliminar tu perfil porque tienes inscripciones activas.");
            }

            _unitOfWork.EstudianteRepo.Remove(estudiante);
            await _unitOfWork.SaveAsync();
        }

        private int GetRequiredUserId()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new ApiException(StatusCodes.Status401Unauthorized, "Usuario no autenticado.");
            }

            return _currentUserService.UserId.Value;
        }
    }
}
