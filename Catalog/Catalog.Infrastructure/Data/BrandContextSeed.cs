using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public static class BrandContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<ProductBrand> brandCollection)
    {
        bool checkBrands = await brandCollection.Find(b => true).AnyAsync();
        
        if (!checkBrands) 
        {
            try 
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SeedData", "brands.json");
                if (!File.Exists(path))
                {
                    // Try alternative path in development
                    path = Path.Combine("Data", "SeedData", "brands.json");
                }

                if (File.Exists(path))
                {
                    string brandsData = await File.ReadAllTextAsync(path);
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands?.Any() == true)
                    {
                        await brandCollection.InsertManyAsync(brands);
                        Console.WriteLine($"Successfully seeded {brands.Count} brands");
                    }
                }
                else 
                {
                    Console.WriteLine($"Could not find brands.json seed file at: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding brands: {ex.Message}");
                throw;
            }
        }
    }
}
