using InventoryMangementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryMangementSystem.Data
{
    public class UserService
    {
		public const string Seedname = "admin";
		public const string SeedPassword = "admin";

        /// <summary>
        /// 
        /// </summary>
		public static void SeedUsers()
		{
			var users = GetAllUsers().FirstOrDefault(x => x.Role == Role.Admin);

			if (users == null)
			{
				CreateUser(Guid.Empty, Seedname, SeedPassword, Role.Admin);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static List<User> GetAllUsers()
        {
            string userPath = UtilsService.GetAppUsersFilePath();

            if (!File.Exists(userPath))
            {
                return new List<User>();
            }

            var json = File.ReadAllText(userPath);

            var result = JsonSerializer.Deserialize<List<User>>(json);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        public static void SaveAllUsers(List<User> users)
        {
            var directoryPath = UtilsService.GetAppDirectoryPath();

            var userPath = UtilsService.GetAppUsersFilePath();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var json = JsonSerializer.Serialize(users);

            File.WriteAllText(userPath, json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<User> CreateUser(Guid Id, string name, string password, Role role)
        {
            var getUser = GetAllUsers();

            var usernameExists = getUser.Any(x => x.Username == name);

            var numberOfAdmins = getUser.Where(x => x.Role == Role.Admin).Count();

            if (numberOfAdmins > 2 && role == Role.Admin)
            {
                throw new Exception("System already has two Admins!!");
            }

            if (usernameExists)
            {
                throw new Exception("Username already exists");
            }

            var createUser = new User()
            {
                CreatedBy = Id,
                Username = name,
                PasswordHash = UtilsService.HashSecret(password), //enetered password should be in encrypted form
                Role = role
            };

            getUser.Add(createUser);

			SaveAllUsers(getUser);
            
            return getUser;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<User> DeleteUser(Guid Id)
        {
            var getUser = GetAllUsers();

            var removeUser = getUser.FirstOrDefault(x => x.Id == Id);

            if (removeUser == null)
            {
                throw new Exception("User not Found");
            }

            getUser.Remove(removeUser);

			SaveAllUsers(getUser);
            
            return (getUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static User LoginUser (string username, string password)
        {
            
            var users = GetAllUsers();

            // Equivalent to "SELECT * FROM USERS WHERE Username = username"
            
            var user = users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            bool passwordIsValid = UtilsService.VerifyPasswordHash(password, user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new Exception("Invalid username or password");
            }

            return user;
        }
    }
}
