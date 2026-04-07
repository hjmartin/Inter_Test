using System.ComponentModel.DataAnnotations;

namespace SunemedicPRO_Inventarios.Server.DTOs
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
