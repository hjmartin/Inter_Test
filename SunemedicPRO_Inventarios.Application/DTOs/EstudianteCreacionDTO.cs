using System.ComponentModel.DataAnnotations;

namespace SunemedicPRO_Inventarios.Server.DTOs
{
    public class EstudianteCreacionDTO
    {
        [Required(ErrorMessage = "El documento es obligatorio")]
        [StringLength(20, ErrorMessage = "El documento no debe superar los 20 caracteres")]
        public string Documento { get; set; } = null!;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100, ErrorMessage = "Los nombres no deben superar los 100 caracteres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no deben superar los 100 caracteres")]
        public string Apellidos { get; set; } = null!;
    }
}
