using InventoryMangementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryMangementSystem.Data
{
    //backend for product crud
    public class ProductService
    {
        //getall converts json to  lists
        public static List<Product> GetAll()
        {
            string productPath = Utils.GetAppProductsFilePath();

            if (!File.Exists(productPath))
            {
                return new List<Product>();
            }

            //reads the json file
            var json = File.ReadAllText(productPath);

            var result = JsonSerializer.Deserialize<List<Product>>(json);

            return result;
        }

        //to store the list of products into json file into a specified folder
        public static void SaveAll(List<Product> products)
        {
            var directoryPath = Utils.GetAppDirectoryPath();
            var productPath = Utils.GetAppProductsFilePath();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var json = JsonSerializer.Serialize(products);

            File.WriteAllText(productPath, json);
        }
        //user id ==> created by id
        public static List<Product> CreateProduct(Guid userId, string name, int quantity)
        {
            if (quantity <= 0)
            {
                throw new Exception("Add a positive integer value for the item.");
            }

            var getProducts = GetAll();

            var addProduct = new Product()
            {
                Name = name,
                Quantity = quantity,
                CreatedBy = userId
            };

            //adding a product into an existing list 
            getProducts.Add(addProduct);

            //Saving all the changes into an existing file

            SaveAll(getProducts);

            return getProducts;
        }

        public static List<Product> UpdateProduct(Guid productId, string name, int quantity)
        {
            var getProducts = GetAll();

            //matching the item id
            var product = getProducts.FirstOrDefault(x => x.Id == productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            product.Name = name;
            product.Quantity = quantity;
            product.ModifiedAt = DateTime.Now;

            SaveAll(getProducts);

            return  getProducts;
        }

        public static List<Product> DeleteProduct(Guid productId)

        {
            var getProducts = GetAll();

            //matching the item id
            var product = getProducts.FirstOrDefault(x => x.Id == productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            getProducts.Remove(product);
            SaveAll(getProducts);

            return getProducts;
        }
    }

    
}
