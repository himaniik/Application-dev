using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// initializing attributes
namespace InventoryMangementSystem.Data.Models
{
    public class User
    {
        public Guid  Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        //property to be match wih role class
        public Role Role { get; set; }

        // who created this users
        public Guid CreatedBy { get; set; }
        //date of user creation
        public DateTime CreatedAt { get; set; } = DateTime.Now;
       
       
        

      

    }
}
