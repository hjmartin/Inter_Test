using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Application.Common.Security;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.Services.Interfaces;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using System.Data;

namespace RegistroEstudiantil.Application.Services
{
    public class InscripcionApplicationService : IInscripcionApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public InscripcionApplicationService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task CrearAsync(InscripcionCreateDto dto)
        {
            try
            {
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var estudiante = await GetRequiredEstudianteAsync();
                    var periodo = dto.Periodo.Trim().ToUpperInvariant();


                    var grupo = await _unitOfWork.Repository<GrupoClase>().GetByIdAsync(dto.GrupoClaseId);
                    //if (grupo is null)
                    //{
                    //    throw new ApiException(StatusCodes.Status404NotFound, "Grupo no encontrado.");
                    //}

                    var count = await _unitOfWork.InscripcionRepo.CountByEstudiantePeriodoAsync(estudiante.Id, periodo);

                    if (count >= 3)
                    {
                        throw new ApiException(StatusCodes.Status400BadRequest, "El estudiante ya tiene 3 materias en este periodo.");
                    }

                    var yaTieneConEseProfesor =
                        await _unitOfWork.InscripcionRepo.ExistsProfesorEnPeriodoAsync(estudiante.Id, grupo.ProfesorId, periodo);
                    if (yaTieneConEseProfesor)
                    {
                        throw new ApiException(StatusCodes.Status400BadRequest, "Ya tiene clases con el mismo profesor en este periodo.");
                    }

                    var yaInscritoMismaMateria =
                        await _unitOfWork.InscripcionRepo.ExistsMateriaEnPeriodoAsync(estudiante.Id, grupo.MateriaId, periodo);
                    if (yaInscritoMismaMateria)
                    {
                        throw new ApiException(StatusCodes.Status400BadRequest, "Ya esta inscrito en esa materia en este periodo.");
                    }

                    _unitOfWork.InscripcionRepo.Add(new Inscripcion
                    {
                        EstudianteId = estudiante.Id,
                        GrupoClaseId = dto.GrupoClaseId,
                        Periodo = periodo
                    });

                    await _unitOfWork.SaveAsync();
                }, IsolationLevel.Serializable);
            }
            catch (DbUpdateException)
            {
                throw new ApiException(StatusCodes.Status409Conflict, "La inscripcion no pudo completarse por concurrencia. Intenta nuevamente.");
            }
        }

        public async Task<IReadOnlyList<string>> VerCompanerosAsync(int grupoId)
        {
            var estudiante = await GetRequiredEstudianteAsync();
            return await _unitOfWork.InscripcionRepo.GetNombresCompanerosAsync(grupoId, estudiante.Id);
        }

        public async Task<IReadOnlyList<InscripcionPublicaDto>> VerRegistrosDeOtroAsync(int estudianteId, string periodo)
        {
            var periodoNormalizado = periodo.Trim().ToUpperInvariant();
            var suyas = await _unitOfWork.InscripcionRepo.GetByEstudiantePeriodoAsync(estudianteId, periodoNormalizado);

            return suyas.Select(i => new InscripcionPublicaDto
            {
                Materia = i.GrupoClase.Materia.Nombre,
                Periodo = i.Periodo
            }).ToList();
        }

        public async Task<IReadOnlyList<InscripcionInfoDto>> GetByOtroEstudianteAsync(int estudianteId)
        {
            await GetRequiredEstudianteAsync();

            var estudianteObjetivo = await
                _unitOfWork.Repository<Estudiante>()
                .GetByIdAsync(estudianteId);
            if (estudianteObjetivo is null)
            {
                throw new ApiException(StatusCodes.Status404NotFound, "Estudiante no encontrado.");
            }

            return await _unitOfWork.InscripcionRepo.GetInscripcionesByEstudianteAsync(estudianteId);
        }

        public async Task<CreditosDto> MisCreditosAsync(string periodo)
        {
            var estudiante = await GetRequiredEstudianteAsync();
            var periodoNormalizado = periodo.Trim().ToUpperInvariant();
            var countMaterias = await _unitOfWork.InscripcionRepo.CountByEstudiantePeriodoAsync(estudiante.Id, periodoNormalizado);
            var usados = countMaterias * 3;

            return new CreditosDto
            {
                Usados = usados,
                Restantes = Math.Max(0, 9 - usados)
            };
        }

        public async Task DeleteAsync(int inscripcionId)
        {
            var estudiante = await GetRequiredEstudianteAsync();
            var inscripcion = await _unitOfWork.InscripcionRepo.GetByIdAsync(inscripcionId);

            if (inscripcion is null)
            {
                throw new ApiException(StatusCodes.Status404NotFound, "Inscripcion no encontrada.");
            }

            if (inscripcion.EstudianteId != estudiante.Id)
            {
                throw new ApiException(StatusCodes.Status403Forbidden, "No puedes eliminar esta inscripcion.");
            }

            await _unitOfWork.InscripcionRepo.DeleteAsync(inscripcionId);
            await _unitOfWork.SaveAsync();
        }

        public Task<IReadOnlyList<GrupoInfoDto>> GetGruposInfoAsync()
            => _unitOfWork.InscripcionRepo.GetGruposInfoAsync();

        public async Task<IReadOnlyList<InscripcionInfoDto>> GetByEstudianteAsync()
        {
            var estudiante = await GetRequiredEstudianteAsync();
            return await _unitOfWork.InscripcionRepo.GetInscripcionesByEstudianteAsync(estudiante.Id);
        }

        public async Task<IReadOnlyList<string>> ObtenerInscripcionesYCompanierosAsync()
        {
            var estudiante = await GetRequiredEstudianteAsync();
            var inscripciones = await _unitOfWork.InscripcionRepo.ObtenerPorEstudianteAsync(estudiante.Id);

            if (!inscripciones.Any())
            {
                throw new ApiException(StatusCodes.Status404NotFound, "No se encontraron inscripciones para este estudiante.");
            }

            var grupoIds = inscripciones.Select(i => i.GrupoClaseId).Distinct().ToList();
            var nombresCompanieros = await _unitOfWork.InscripcionRepo.ObtenerNombresPorGruposAsync(grupoIds);

            return nombresCompanieros
                .Where(n => n != estudiante.Nombres)
                .ToList();
        }

        private async Task<Estudiante> GetRequiredEstudianteAsync()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new ApiException(StatusCodes.Status401Unauthorized, "Usuario no autenticado.");
            }

            var estudiante = (await _unitOfWork.Repository<Estudiante>().ListAsync(e => e.UsuarioId == _currentUserService.UserId.Value))
                .FirstOrDefault();

            if (estudiante is null)
            {
                throw new ApiException(StatusCodes.Status400BadRequest, "Primero debes crear tu perfil de estudiante.");
            }

            return estudiante;
        }
    }
}



