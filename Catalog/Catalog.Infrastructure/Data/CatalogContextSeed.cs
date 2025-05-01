using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public static class CatalogContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<Product> productCollection)
    {
        bool checkProducts = await productCollection.Find(b => true).AnyAsync();
        
        if (!checkProducts) 
        {
            try 
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SeedData", "products.json");
                if (!File.Exists(path))
                {
                    // Try alternative path in development
                    path = Path.Combine("Data", "SeedData", "products.json");
                }

                if (File.Exists(path))
                {
                    string productsData = await File.ReadAllTextAsync(path);
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products?.Any() == true)
                    {
                        await productCollection.InsertManyAsync(products);
                        Console.WriteLine($"Successfully seeded {products.Count} products");
                    }
                }
                else 
                {
                    Console.WriteLine($"Could not find products.json seed file at: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding products: {ex.Message}");
                throw;
            }
        }
    }
}
