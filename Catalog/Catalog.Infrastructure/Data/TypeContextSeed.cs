using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public static class TypeContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<ProductType> typeCollection)
    {
        bool checkTypes = await typeCollection.Find(b => true).AnyAsync();
        
        if (!checkTypes) 
        {
            try 
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SeedData", "types.json");
                if (!File.Exists(path))
                {
                    // Try alternative path in development
                    path = Path.Combine("Data", "SeedData", "types.json");
                }

                if (File.Exists(path))
                {
                    string typesData = await File.ReadAllTextAsync(path);
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types?.Any() == true)
                    {
                        await typeCollection.InsertManyAsync(types);
                        Console.WriteLine($"Successfully seeded {types.Count} types");
                    }
                }
                else 
                {
                    Console.WriteLine($"Could not find types.json seed file at: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding types: {ex.Message}");
                throw;
            }
        }
    }
}
