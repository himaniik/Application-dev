using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangementSystem.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        public Guid  Id { get; set; } = Guid.NewGuid();
        
        public string Username { get; set; }
        
        public string PasswordHash { get; set; }

        public Role Role { get; set; }

        public Guid CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
