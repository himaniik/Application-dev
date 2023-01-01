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
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid ItemId { get; set; }

        public Guid OrderedBy { get; set; }
        
        public int Quantity { get; set; }
        
        public Guid ApprovedBy { get; set; }
        
        public DateTime OrderedAt { get; set; } = DateTime.Now;
       
        public bool IsApproved { get; set; } = false;

    }
}
