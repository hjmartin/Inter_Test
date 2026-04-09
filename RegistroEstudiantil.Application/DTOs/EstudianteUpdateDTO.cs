using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantil.Application.DTOs
{
    public class EstudianteUpdateDTO
    {
        [Required, StringLength(20)]
        public string Documento { get; set; }
        [Required, StringLength(100)]
        public string Nombres { get; set; }
        [Required, StringLength(100)]
        public string Apellidos { get; set; }
    }
}


