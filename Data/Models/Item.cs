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
    public class Item
    {
        public Guid Id  { get; set; } = Guid.NewGuid();
        
        public string Name { get; set; }
        
        public float Price { get; set; }
        
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public Guid CreatedBy { get; set; } 
        
        public DateTime ModifiedAt  { get; set; }
    }
}
