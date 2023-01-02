using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangementSystem.Data
{
    public class UtilsService
        
    {
        private const char _segmentDelimiter = ':';

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HashSecret(string input)
        {
            var saltSize = 16;
            var iterations = 100_000;
            var keySize = 32;
            var algorithm = HashAlgorithmName.SHA256;
            var salt = RandomNumberGenerator.GetBytes(saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

            var result = string.Join(
                _segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hashString"></param>
        /// <returns></returns>
        public static bool VerifyPasswordHash(string input, string hashString)
        {
            var segments = hashString.Split(_segmentDelimiter);
            var hash = Convert.FromHexString(segments[0]);
            var salt = Convert.FromHexString(segments[1]);
            var iterations = int.Parse(segments[2]);
            var algorithm = new HashAlgorithmName(segments[3]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetAppDirectoryPath()
        {
            return @"C:\Users\ROG\OneDrive\Desktop\New folder\Application-dev\wwwroot\data";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetAppItemsFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "items.json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetAppOrdersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "orders.json");
        }
    }

}

