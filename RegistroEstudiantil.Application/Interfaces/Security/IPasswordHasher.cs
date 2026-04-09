namespace RegistroEstudiantil.Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string GenerarHash(string value);
        bool Verificar(string value, string hash);
    }
}
