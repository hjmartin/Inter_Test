using RegistroEstudiantil.Application.DTOs;

namespace RegistroEstudiantil.Application.Services.Interfaces
{
    public interface IInscripcionApplicationService
    {
        Task CrearAsync(InscripcionCreateDto dto);
        Task<IReadOnlyList<string>> VerCompanerosAsync(int grupoId);
        Task<IReadOnlyList<InscripcionPublicaDto>> VerRegistrosDeOtroAsync(int estudianteId, string periodo);
        Task<IReadOnlyList<InscripcionInfoDto>> ObtenerPorOtroEstudianteAsync(int estudianteId);
        //Task<CreditosDto> ObtenerMisCreditosAsync(string periodo);
        Task EliminarAsync(int inscripcionId);
        Task<IReadOnlyList<GrupoInfoDto>> ObtenerInfoGruposAsync();
        Task<IReadOnlyList<InscripcionInfoDto>> ObtenerPorEstudianteAsync();
        Task<IReadOnlyList<string>> ObtenerInscripcionesYCompanierosAsync();
    }
}
