using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            bool checkTypes = typeCollection.Find(b => true).Any();
            
            // Fix the path construction to correctly locate the seed file
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            string catalogDirectory = Directory.GetParent(projectDirectory).FullName; // Go up one more level to reach the Catalog directory
            string filePath = Path.Combine(catalogDirectory, "Catalog.Infrastructure", "Data", "SeedData", "types.json");
            
            if (!checkTypes)
            {
                if (File.Exists(filePath))
                {
                    var typesData = File.ReadAllText(filePath);
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types != null)
                    {
                        foreach (var item in types)
                        {
                            typeCollection.InsertOneAsync(item);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Seed file not found at: {filePath}");
                    
                    // Try alternative paths as fallback
                    string altPath1 = Path.Combine(projectDirectory, "Data", "SeedData", "types.json");
                    string altPath2 = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData", "types.json");
                    
                    Console.WriteLine($"Trying alternative paths:\n{altPath1}\n{altPath2}");
                    
                    if (File.Exists(altPath1))
                    {
                        Console.WriteLine($"Found at alternative path 1: {altPath1}");
                        var typesData = File.ReadAllText(altPath1);
                        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                        if (types != null)
                        {
                            foreach (var item in types)
                            {
                                typeCollection.InsertOneAsync(item);
                            }
                        }
                    }
                    else if (File.Exists(altPath2))
                    {
                        Console.WriteLine($"Found at alternative path 2: {altPath2}");
                        var typesData = File.ReadAllText(altPath2);
                        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                        if (types != null)
                        {
                            foreach (var item in types)
                            {
                                typeCollection.InsertOneAsync(item);
                            }
                        }
                    }
                }
            }
        }
    }
}
