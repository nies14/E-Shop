using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;


namespace Catalog.Infrastructure.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProducts = productCollection.Find(b => true).Any();
            
            // Fix the path construction to correctly locate the seed file
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            string catalogDirectory = Directory.GetParent(projectDirectory).FullName; // Go up one more level to reach the Catalog directory
            string filePath = Path.Combine(catalogDirectory, "Catalog.Infrastructure", "Data", "SeedData", "products.json");
            
            if (!checkProducts)
            {
                if (File.Exists(filePath))
                {
                    var productsData = File.ReadAllText(filePath);
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products != null)
                    {
                        foreach (var item in products)
                        {
                            productCollection.InsertOneAsync(item);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Seed file not found at: {filePath}");
                    
                    // Try alternative paths as fallback
                    string altPath1 = Path.Combine(projectDirectory, "Data", "SeedData", "products.json");
                    string altPath2 = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData", "products.json");
                    
                    Console.WriteLine($"Trying alternative paths:\n{altPath1}\n{altPath2}");
                    
                    if (File.Exists(altPath1))
                    {
                        Console.WriteLine($"Found at alternative path 1: {altPath1}");
                        var productsData = File.ReadAllText(altPath1);
                        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                        if (products != null)
                        {
                            foreach (var item in products)
                            {
                                productCollection.InsertOneAsync(item);
                            }
                        }
                    }
                    else if (File.Exists(altPath2))
                    {
                        Console.WriteLine($"Found at alternative path 2: {altPath2}");
                        var productsData = File.ReadAllText(altPath2);
                        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                        if (products != null)
                        {
                            foreach (var item in products)
                            {
                                productCollection.InsertOneAsync(item);
                            }
                        }
                    }
                }
            }
        }
    }
}
