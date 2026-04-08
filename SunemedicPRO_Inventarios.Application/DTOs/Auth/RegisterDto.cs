namespace SunemedicPRO_Inventarios.Server.DTOs.Auth
{
    public class RegisterDto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El email es obligatorio.")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "El email no es valido.")]
        public string Email { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "La contrasena es obligatoria.")]
        public string Pass { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El documento es obligatorio.")]
        [System.ComponentModel.DataAnnotations.StringLength(20, ErrorMessage = "El documento no debe superar los 20 caracteres.")]
        public string Documento { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Los nombres son obligatorios.")]
        [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "Los nombres no deben superar los 100 caracteres.")]
        public string Nombres { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "Los apellidos no deben superar los 100 caracteres.")]
        public string Apellidos { get; set; } = null!;
    }
}
