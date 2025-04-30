using System;
using System.IO;
using System.Text;

namespace Journal
{
    public static class PasswordManager
    {
        private static readonly string passwordFilePath = "password.dat";

        public static bool PasswordExists()
        {
            return File.Exists(passwordFilePath);
        }

        public static void SavePassword(string password)
        {
            string encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            File.WriteAllText(passwordFilePath, encrypted);
        }

        public static string LoadPassword()
        {
            if (!File.Exists(passwordFilePath))
                return string.Empty;

            string encrypted = File.ReadAllText(passwordFilePath);
            return Encoding.UTF8.GetString(Convert.FromBase64String(encrypted));
        }


    }
}
