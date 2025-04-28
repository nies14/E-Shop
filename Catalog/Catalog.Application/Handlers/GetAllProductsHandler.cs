using System.Text;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var productList = await _productRepository.GetProducts(request.CatalogSpecParams);

        return productList == null || productList.Count == 0
            ? null
            : new Pagination<ProductResponse>(request.CatalogSpecParams.PageIndex, 
            request.CatalogSpecParams.PageSize,
            productList.Count,
            productList.Data.Convert().ToList());
    }
}
