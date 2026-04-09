using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.DTOs
{
    public class InscripcionesYCompanierosDTO
    {
        public List<Inscripcion> Inscripciones { get; set; }
        public List<string> NombresCompanieros { get; set; }
    }

}


