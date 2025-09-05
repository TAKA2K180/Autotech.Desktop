
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.Helper
{
    public class PasswordHelper
    {
        // Hash a password
        public static string HashPassword(string password)
        {
            // Generate a hash for the password
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify if a password matches the hashed password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Compare the provided password with the stored hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
