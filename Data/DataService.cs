using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using InventoryMangementSystem.Data.Models;

namespace InventoryMangementSystem.Data
{
    public class Graph
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Order> GetApprovedOrders()
        {
            string ordersPath = UtilsService.GetAppOrdersFilePath();

            if (!File.Exists(ordersPath))
            {
                return new List<Order>();
            }

            var json = File.ReadAllText(ordersPath);

            var orders = JsonSerializer.Deserialize<List<Order>>(json);

            var result = from order in orders
                         where order.IsApproved == true
                         select order;

            return result.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Statistics> GetOrderedItemsQuantity()
        {
            var orders = GetApprovedOrders();

            var result = from order in orders
                         group order by order.ItemId
                         into item
                         select new Statistics
                         {
                             ItemId = item.Key,
                             Quantity = item.Sum(x => x.Quantity)
                         };

            return result.ToList();
        }
    }
}
