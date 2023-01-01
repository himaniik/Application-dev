using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangementSystem.Data.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; }
        public Guid ApprovedBy { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderedBy { get; set; }
        public DateTime CreatedAt { get; set; }
       
        public bool IsApproved { get; set; } = false;

    }
}
