using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.DTOs
{
    public class InscripcionesYCompanierosDTO
    {
        public List<Inscripcion> Inscripciones { get; set; }
        public List<string> NombresCompanieros { get; set; }
    }

}
