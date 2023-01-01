using InventoryMangementSystem.Data.Models;
using Java.Sql;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryMangementSystem.Data
{
    public class OrderService
    {
        //getting the json file
        public static List<Order> GetAll()
        {
            string orderPath = Utils.GetAppOrdersFilePath();

            if (!File.Exists(orderPath))
            {
                return new List<Order>();
            }

            //reads the json file
            var json = File.ReadAllText(orderPath);

            var result = JsonSerializer.Deserialize<List<Order>>(json);

            return result;
        }

        //savin the list again into json file
        public static void SaveAll(List<Order> order)
        {
            var directoryPath = Utils.GetAppDirectoryPath();
            var orderPath = Utils.GetAppOrdersFilePath();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var json = JsonSerializer.Serialize(order);

            File.WriteAllText(orderPath, json);
        }

        //creating an order

        public static List<Order> CreateOrder(Guid userId, Guid productId, int quantity)
        {
            var day = (int)DateTime.Now.DayOfWeek + 1;

            var time = (int)DateTime.Now.Hour;

            //day mon-fri and time 9-6
            if(day > 2 && day < 6)
            {
                if (time > 9 && time < 18)
                {
                    var getOrder = GetAll();

                    var getProducts = ProductService.GetAll();

                    var product = getProducts.FirstOrDefault(x => x.Id == productId);

                    var createOrder = new Order()
                    {
                        OrderedBy = userId,
                        ProductId = productId,
                        Quantity = quantity,
                        CreatedAt = DateTime.Now,

                    };
                    getOrder.Add(createOrder);
                    SaveAll(getOrder);

                    return getOrder;

                }
                else
                {
                    throw new Exception("Order can be place only on working days i.e Monday-Friday");
                }
            }
            else
            {
                throw new Exception("Order can be place only between 9 am to 6pm");
            }
            }

           

        //update an order
        public static List<Order> UpdateOrder(Guid userId, Guid orderId, int quantity)
        {
            var getOrder = GetAll();
            var getProducts = ProductService.GetAll();

            var order = getOrder.FirstOrDefault(x => x.Id == orderId);

            var product = getProducts.FirstOrDefault(x => x.Id == order.ProductId);

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            order.ApprovedBy = userId;
            order.Id = orderId;
            product.Quantity = quantity;
            order.IsApproved = true;

            SaveAll(getOrder);
            return getOrder;



        }
    }
}
