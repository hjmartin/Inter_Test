namespace SunemedicPRO_Inventarios.Server.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Rol { get; set; } = "Estudiante"; // Estudiante | Admin, etc.

        public Estudiante? Estudiante { get; set; }
    }
}
