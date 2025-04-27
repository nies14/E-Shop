using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productTobeUpdated = request?.Convert();

        if (productTobeUpdated is null)
        {
            throw new ApplicationException("There is an issue with mapping while creating new product");
        }

        return await _productRepository.UpdateProduct(productTobeUpdated);
    }
}
 