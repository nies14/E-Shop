using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;
using System.IO;
using System.Reflection;

namespace Catalog.Infrastructure.Data;

public static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
    {
        bool checkBrands = brandCollection.Find(b => true).Any();
        
        // Fix the path construction to correctly locate the seed file
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
        string catalogDirectory = Directory.GetParent(projectDirectory).FullName; // Go up one more level to reach the Catalog directory
        string filePath = Path.Combine(catalogDirectory, "Catalog.Infrastructure", "Data", "SeedData", "brands.json");
        
        if (!checkBrands) {
            if (File.Exists(filePath))
            {
                var brandsData = File.ReadAllText(filePath);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands != null)
                {
                    foreach(var item in brands)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }
            else
            {
                Console.WriteLine($"Seed file not found at: {filePath}");
                
                // Try alternative paths as fallback
                string altPath1 = Path.Combine(projectDirectory, "Data", "SeedData", "brands.json");
                string altPath2 = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData", "brands.json");
                
                Console.WriteLine($"Trying alternative paths:\n{altPath1}\n{altPath2}");
                
                if (File.Exists(altPath1))
                {
                    Console.WriteLine($"Found at alternative path 1: {altPath1}");
                    var brandsData = File.ReadAllText(altPath1);
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands != null)
                    {
                        foreach(var item in brands)
                        {
                            brandCollection.InsertOneAsync(item);
                        }
                    }
                }
                else if (File.Exists(altPath2))
                {
                    Console.WriteLine($"Found at alternative path 2: {altPath2}");
                    var brandsData = File.ReadAllText(altPath2);
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands != null)
                    {
                        foreach(var item in brands)
                        {
                            brandCollection.InsertOneAsync(item);
                        }
                    }
                }
            }
        }
    }
}
