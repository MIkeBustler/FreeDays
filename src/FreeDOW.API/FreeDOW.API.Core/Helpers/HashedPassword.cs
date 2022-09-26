using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FreeDOW.API.Core.Helpers
{
    /// <summary>
    /// Password conversion service for secure storage - store hash with salt
    /// </summary>
    public static class HashedPassword
    {
        public static string GetPasswordHash(string salt, string password)
        {
            var hSalt = MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(salt)).ToString();
            var hash = MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes($"{hSalt}.{password}"));
            return Convert.ToHexString(hash);
        }

        public static bool VerifyHashedPassword(string hash, string salt, string password)
        {
            if (hash == null) return false;
            if(password == null) return false;
            if(salt == null) return false;
            var hSalt = MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(salt)).ToString();
            var calchash = MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes($"{hSalt}.{password}"));
            var inphash = Convert.ToHexString(calchash);
            return inphash.Equals(hash);
        }
    }
}
