using RegistroEstudiantil.Application.Interfaces.Security;

namespace RegistroEstudiantil.Infrastructure.Security
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string value) => BCrypt.Net.BCrypt.HashPassword(value);

        public bool Verify(string value, string hash) => BCrypt.Net.BCrypt.Verify(value, hash);
    }
}
