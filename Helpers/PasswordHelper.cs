using System.Security.Cryptography;
using System.Text;

namespace DASALUD.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Genera un hash SHA256 de la contraseña
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
