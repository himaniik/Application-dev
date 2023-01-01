using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InventoryMangementSystem.Data.Models;

namespace InventoryMangementSystem.Data
{
	public class OrderService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static List<Order> GetAllOrders()
		{
			string orderPath = UtilsService.GetAppOrdersFilePath();

			if (!File.Exists(orderPath))
			{
				return new List<Order>();
			}

			var json = File.ReadAllText(orderPath);

			var result = JsonSerializer.Deserialize<List<Order>>(json);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="order"></param>
		public static void SaveAllOrders(List<Order> order)
		{
			var directoryPath = UtilsService.GetAppDirectoryPath();

			var orderPath = UtilsService.GetAppOrdersFilePath();

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			var json = JsonSerializer.Serialize(order);

			File.WriteAllText(orderPath, json);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="itemId"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static List<Order> CreateOrder(Guid userId, Guid itemId, int quantity)
		{
			var getOrders = GetAllOrders();

			var createOrder = new Order()
			{
				OrderedBy = userId,
				ItemId = itemId,
				Quantity = quantity,
			};

			getOrders.Add(createOrder);

			SaveAllOrders(getOrders);

			return getOrders;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="orderId"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static List<Order> ApproveOrder(Guid userId, Guid orderId, int quantity)
		{

			///
			var day = (int)DateTime.Now.DayOfWeek + 1;

			///
			var time = (int)DateTime.Now.Hour;

			if (day > 2 && day < 6)
			{
				if (time > 9 && time < 18)
				{
					var getOrders = GetAllOrders();

					var getItems = ItemService.GetAllItems();

					var order = getOrders.FirstOrDefault(x => x.Id == orderId);

					var item = getItems.FirstOrDefault(x => x.Id == order.ItemId);

					if (order == null)
					{
						throw new Exception("Order not found, select a valid order.");
					}

					order.ApprovedBy = userId;
					order.Id = orderId;
					order.IsApproved = true;

					item.Quantity = quantity;

					SaveAllOrders(getOrders);

					ItemService.SaveAllItems(getItems);

					return getOrders;
				}
				else
				{
					throw new Exception("Order can be approved only on working days i.e Monday - Friday.");
				}
			}
			else
			{
				throw new Exception("Order can be approved only between 9am to 6pm.");
			}
		}
	}
}
