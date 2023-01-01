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
        //gets the users json file
        public static List<User> GetAll()
        {
            string userPath = Utils.GetAppUsersFilePath();

            if (!File.Exists(userPath))
            {
                return new List<User>();
            }

            //reads the json file
            var json = File.ReadAllText(userPath);

            var result = JsonSerializer.Deserialize<List<User>>(json);

            return result;
        }

        //to store the list of users into json file into a specified folder
        public static void SaveAll(List<User> users)
        {
            var directoryPath = Utils.GetAppDirectoryPath();
            var userPath = Utils.GetAppUsersFilePath();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var json = JsonSerializer.Serialize(users);

            File.WriteAllText(userPath, json);
        }

        //creater id
        public static List<User> CreateUser(Guid Id, string name, string password, Role role)
        {
            var getUser = GetAll();

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
                PasswordHash = Utils.HashSecret(password), //enetered password should be in encrypted form
                Role = role
            };

            getUser.Add(createUser);
            SaveAll(getUser);
            return getUser;

        }
        //user id
        public static List<User> DeleteUser(Guid Id)
        {
            var getUser = GetAll();

            var user = getUser.FirstOrDefault(x => x.Id == Id);

            if (user == null)
            {
                throw new Exception("User not Found");
            }

            getUser.Remove(user);
            SaveAll(getUser);
            return (getUser);
        }

        public static User LoginUser (string username, string password)
        {
            
            var users = GetAll();
            //select * from users where username = username
            var user = users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new Exception("Invalid username or password");
            }

            return user;
        }
    }


}
