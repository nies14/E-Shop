using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers;

public static class TypeMapper
{
    public static IEnumerable<TypesResponse?> Convert(this IEnumerable<ProductType> productTypes)
    {
        return productTypes == null
            ? Enumerable.Empty<TypesResponse>()
            : productTypes.Select(product => product.Convert());
    }

    public static TypesResponse? Convert(this ProductType productType)
    {
        return productType == null
            ? null
            :new TypesResponse
            {
                Id = productType.Id,
                Name = productType.Name
            };
    }
}
