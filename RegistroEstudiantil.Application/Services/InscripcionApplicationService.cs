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
            await _unitOfWork.EjecutarEnTransaccionAsync(async () =>
            {
                var estudiante = await ObtenerEstudianteRequeridoAsync();
                var periodo = dto.Periodo.Trim().ToUpperInvariant();
                var grupo = await _unitOfWork.GrupoClaseRepo.ObtenerPorIdAsync(dto.GrupoClaseId);

                if (grupo is null)
                {
                    throw new NotFoundException("Grupo no encontrado.");
                }

                var count = await _unitOfWork.InscripcionRepo.ContarPorEstudiantePeriodoAsync(estudiante.Id, periodo);
                var yaTieneConEseProfesor =
                    await _unitOfWork.InscripcionRepo.ExisteProfesorEnPeriodoAsync(estudiante.Id, grupo.ProfesorId, periodo);
                var yaInscritoMismaMateria =
                    await _unitOfWork.InscripcionRepo.ExisteMateriaEnPeriodoAsync(estudiante.Id, grupo.MateriaId, periodo);

                InscripcionRules.ValidarNuevaInscripcion(count, yaTieneConEseProfesor, yaInscritoMismaMateria);

                _unitOfWork.InscripcionRepo.Agregar(new Inscripcion
                {
                    EstudianteId = estudiante.Id,
                    GrupoClaseId = dto.GrupoClaseId,
                    Periodo = periodo
                });

                await _unitOfWork.GuardarCambiosAsync();
            }, IsolationLevel.Serializable);
        }

        public async Task<IReadOnlyList<string>> VerCompanerosAsync(int grupoId)
        {
            var estudiante = await ObtenerEstudianteRequeridoAsync();
            return await _unitOfWork.InscripcionRepo.ObtenerNombresCompanerosAsync(grupoId, estudiante.Id);
        }

        public async Task<IReadOnlyList<InscripcionPublicaDto>> VerRegistrosDeOtroAsync(int estudianteId, string periodo)
        {
            var periodoNormalizado = periodo.Trim().ToUpperInvariant();
            var suyas = await _unitOfWork.InscripcionRepo.ObtenerPorEstudiantePeriodoAsync(estudianteId, periodoNormalizado);

            return suyas.Select(i => new InscripcionPublicaDto
            {
                Materia = i.GrupoClase.Materia.Nombre,
                Periodo = i.Periodo
            }).ToList();
        }

        public async Task<IReadOnlyList<InscripcionInfoDto>> ObtenerPorOtroEstudianteAsync(int estudianteId)
        {
            await ObtenerEstudianteRequeridoAsync();

            var estudianteObjetivo = await _unitOfWork.EstudianteRepo.ObtenerPorIdAsync(estudianteId);
            if (estudianteObjetivo is null)
            {
                throw new NotFoundException("Estudiante no encontrado.");
            }

            return await _unitOfWork.InscripcionRepo.ObtenerInscripcionesPorEstudianteAsync(estudianteId);
        }

       
        public async Task EliminarAsync(int inscripcionId)
        {
            var estudiante = await ObtenerEstudianteRequeridoAsync();
            var inscripcion = await _unitOfWork.InscripcionRepo.ObtenerPorIdAsync(inscripcionId);

            if (inscripcion is null)
            {
                throw new NotFoundException("Inscripcion no encontrada.");
            }

            if (inscripcion.EstudianteId != estudiante.Id)
            {
                throw new ForbiddenException("No puedes eliminar esta inscripcion.");
            }

            await _unitOfWork.InscripcionRepo.EliminarAsync(inscripcionId);
            await _unitOfWork.GuardarCambiosAsync();
        }

        public Task<IReadOnlyList<GrupoInfoDto>> ObtenerInfoGruposAsync()
            => _unitOfWork.InscripcionRepo.ObtenerInfoGruposAsync();

        public async Task<IReadOnlyList<InscripcionInfoDto>> ObtenerPorEstudianteAsync()
        {
            var estudiante = await ObtenerEstudianteRequeridoAsync();
            return await _unitOfWork.InscripcionRepo.ObtenerInscripcionesPorEstudianteAsync(estudiante.Id);
        }

        public async Task<IReadOnlyList<string>> ObtenerInscripcionesYCompanierosAsync()
        {
            var estudiante = await ObtenerEstudianteRequeridoAsync();
            var inscripciones = await _unitOfWork.InscripcionRepo.ObtenerPorEstudianteAsync(estudiante.Id);

            if (!inscripciones.Any())
            {
                return [];
            }

            var grupoIds = inscripciones.Select(i => i.GrupoClaseId).Distinct().ToList();
            var nombresCompanieros = await _unitOfWork.InscripcionRepo.ObtenerNombresPorGruposAsync(grupoIds);

            return nombresCompanieros
                .Where(n => n != estudiante.Nombres)
                .ToList();
        }

        private async Task<Estudiante> ObtenerEstudianteRequeridoAsync()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedException("Usuario no autenticado.");
            }

            var estudiante = await _unitOfWork.EstudianteRepo.ObtenerPorUsuarioIdAsync(_currentUserService.UserId.Value);

            if (estudiante is null)
            {
                throw new ValidationException("Primero debes crear tu perfil de estudiante.");
            }

            return estudiante;
        }
    }
}
