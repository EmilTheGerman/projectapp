using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace passwordmanager.Services
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(
                    Encoding.UTF8.GetBytes(password));

                return Convert.ToBase64String(bytes);
            }
        }
        public static string Encrypt(string text)
        {
            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes(text));
        }

        public static string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            try
            {
                byte[] bytes = Convert.FromBase64String(text);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                return text;
            }
        }
    }
}
