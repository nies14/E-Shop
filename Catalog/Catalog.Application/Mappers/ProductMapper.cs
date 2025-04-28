using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Application.Mappers;

public static class ProductMapper
{
    public static IEnumerable<ProductResponse> Convert(this IEnumerable<Product> products)
    {
        return products == null
            ? Enumerable.Empty<ProductResponse>()
            : products.Select(product => product.Convert());
    }

    public static ProductResponse Convert(this Product product)
    {
        if (product == null)
            return null;

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageFile = product.ImageFile,
            Price = product.Price,
            Summary = product.Summary,
            Brands = product.Brands,
            Types = product.Types
        };
    }

    public static Product? Convert(this CreateProductCommand product)
    {
        return product == null
            ? null
            : new Product
            {
            Name = product.Name,
            Description = product.Description,
            ImageFile = product.ImageFile,
            Price = product.Price,
            Summary = product.Summary,
            Brands = product.Brands,
            Types = product.Types
        };
    }

    public static Product? Convert(this UpdateProductCommand product)
    {
        return product == null
            ? null
            : new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageFile = product.ImageFile,
                Price = product.Price,
                Summary = product.Summary,
                Brands = product.Brands,
                Types = product.Types
            };
    }
}