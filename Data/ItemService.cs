using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InventoryMangementSystem.Data.Models;

namespace InventoryMangementSystem.Data
{
    public class ItemService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Item> GetAllItems()
        {
            string itemPath = UtilsService.GetAppItemsFilePath();

            if (!File.Exists(itemPath))
            {
                return new List<Item>();
            }

            var json = File.ReadAllText(itemPath);

            var result = JsonSerializer.Deserialize<List<Item>>(json);

            return result;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		public static void SaveAllItems(List<Item> items)
        {
            var directoryPath = UtilsService.GetAppDirectoryPath();
            
            var itemPath = UtilsService.GetAppItemsFilePath();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var json = JsonSerializer.Serialize(items);

            File.WriteAllText(itemPath, json);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Item> CreateItem(Guid userId, string name, int quantity, float price)
        {
            if (quantity <= 0)
            {
                throw new Exception("Add a positive integer value for the item.");
            }

            var getItems = GetAllItems();

            var addItem = new Item()
            {
                Name = name,
                Quantity = quantity,
                CreatedBy = userId,
                Price = price
            };

			getItems.Add(addItem);

			SaveAllItems(getItems);

            return getItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Item> UpdateItem(Guid itemId, string name, int quantity, float price)
        {
            var getItems = GetAllItems();

            var updateItem = getItems.SingleOrDefault(x => x.Id == itemId);

            if (updateItem == null)
            {
                throw new Exception("Item not found.");
            }

			updateItem.Name = name;
			updateItem.Quantity = quantity;
			updateItem.Price = price;
			updateItem.ModifiedAt = DateTime.Now;

			SaveAllItems(getItems);

            return getItems;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static List<Item> DeleteItem(Guid itemId)

        {
            var getItems = GetAllItems();

            var deleteItem = getItems.SingleOrDefault(x => x.Id == itemId);

            if (deleteItem == null)
            {
                throw new Exception("Item not found.");
            }

			getItems.Remove(deleteItem);

			SaveAllItems(getItems);

            return getItems;
		}
    }
}