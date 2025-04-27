using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productTobeCreated = request?.Convert();

        if (productTobeCreated is null)
        {
            throw new ApplicationException("There is an issue with mapping while creating new product");
        }

        var newProduct = await _productRepository.CreateProduct(productTobeCreated);

        return newProduct.Convert();
    }
}
