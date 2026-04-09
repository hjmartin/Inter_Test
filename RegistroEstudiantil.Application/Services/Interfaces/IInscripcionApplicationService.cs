using RegistroEstudiantil.Application.DTOs;

namespace RegistroEstudiantil.Application.Services.Interfaces
{
    public interface IInscripcionApplicationService
    {
        Task CrearAsync(InscripcionCreateDto dto);
        Task<IReadOnlyList<string>> VerCompanerosAsync(int grupoId);
        Task<IReadOnlyList<InscripcionPublicaDto>> VerRegistrosDeOtroAsync(int estudianteId, string periodo);
        Task<IReadOnlyList<InscripcionInfoDto>> GetByOtroEstudianteAsync(int estudianteId);
        Task<CreditosDto> MisCreditosAsync(string periodo);
        Task DeleteAsync(int inscripcionId);
        Task<IReadOnlyList<GrupoInfoDto>> GetGruposInfoAsync();
        Task<IReadOnlyList<InscripcionInfoDto>> GetByEstudianteAsync();
        Task<IReadOnlyList<string>> ObtenerInscripcionesYCompanierosAsync();
    }
}
