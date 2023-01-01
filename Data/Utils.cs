using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangementSystem.Data
{
    public class Utils
        
    {
        private const char _segmentDelimiter = ':';
        //hash --> process to encrypt passowrd
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

        public static bool VerifyHash(string input, string hashString)
            // process to verify the user entered pw with the stored encrypted pw
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
        //spacifying a folder for the details of the program
        public static string GetAppDirectoryPath()
        {
            return @"C:\Users\Himani\Desktop\semester-5\ad\InventoryMangementSystem\wwwroot\data";
        }

        public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

        public static string GetAppProductsFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "products.json");
        }

        public static string GetAppOrdersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "orders.json");
        }
    }

}

