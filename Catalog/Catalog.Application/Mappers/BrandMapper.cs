using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers;

public static class BrandMapper
{
    public static IEnumerable<BrandResponse?> Convert(this IEnumerable<ProductBrand> brands)
    {
        return brands == null 
            ? Enumerable.Empty<BrandResponse>() 
            : brands.Select(brand => brand.Convert());
    }

    public static BrandResponse? Convert(this ProductBrand brand)
    {
        return brand == null
            ? null
            : new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name
            };
    }
}