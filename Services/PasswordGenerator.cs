using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectapp.Services
{
    public class PasswordGenerator
    {
        public static string Generate(int length, bool upper, bool lower, bool digits, bool symbols)
        {
            string chars = "";

            if (upper) chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (lower) chars += "abcdefghijklmnopqrstuvwxyz";
            if (digits) chars += "0123456789";
            if (symbols) chars += "!@#$%^&*()";

            if (string.IsNullOrEmpty(chars))
                return "ERROR";

            Random rnd = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
