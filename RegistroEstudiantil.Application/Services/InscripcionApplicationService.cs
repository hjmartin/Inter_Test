using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Application.Common.Security;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Application.Services.Interfaces;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Domain.Services;
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
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var estudiante = await GetRequiredEstudianteAsync();
                var periodo = dto.Periodo.Trim().ToUpperInvariant();
                var grupo = await _unitOfWork.Repository<GrupoClase>().GetByIdAsync(dto.GrupoClaseId);

                if (grupo is null)
                {
                    throw new NotFoundException("Grupo no encontrado.");
                }

                var count = await _unitOfWork.InscripcionRepo.CountByEstudiantePeriodoAsync(estudiante.Id, periodo);
                var yaTieneConEseProfesor =
                    await _unitOfWork.InscripcionRepo.ExistsProfesorEnPeriodoAsync(estudiante.Id, grupo.ProfesorId, periodo);
                var yaInscritoMismaMateria =
                    await _unitOfWork.InscripcionRepo.ExistsMateriaEnPeriodoAsync(estudiante.Id, grupo.MateriaId, periodo);

                InscripcionRules.ValidarNuevaInscripcion(count, yaTieneConEseProfesor, yaInscritoMismaMateria);

                _unitOfWork.InscripcionRepo.Add(new Inscripcion
                {
                    EstudianteId = estudiante.Id,
                    GrupoClaseId = dto.GrupoClaseId,
                    Periodo = periodo
                });

                await _unitOfWork.SaveAsync();
            }, IsolationLevel.Serializable);
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

            var estudianteObjetivo = await _unitOfWork.Repository<Estudiante>().GetByIdAsync(estudianteId);
            if (estudianteObjetivo is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
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
                throw new NotFoundException("Inscripcion no encontrada.");
            }

            if (inscripcion.EstudianteId != estudiante.Id)
            {
                throw new ForbiddenException("No puedes eliminar esta inscripcion.");
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
                throw new NotFoundException("No se encontraron inscripciones para este estudiante.");
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
                throw new UnauthorizedException("Usuario no autenticado.");
            }

            var estudiante = (await _unitOfWork.Repository<Estudiante>().ListAsync(
                e => e.UsuarioId == _currentUserService.UserId.Value)).FirstOrDefault();

            if (estudiante is null)
            {
                throw new ValidationException("Primero debes crear tu perfil de estudiante.");
            }

            return estudiante;
        }
    }
}
