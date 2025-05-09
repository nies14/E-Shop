using Discount.Application.Commands;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using Discount.Application.Mappers;

namespace Discount.Application.Handlers;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public CreateDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException("Discount create request can not be null");
        }

        var coupon = await _discountRepository.CreateDiscount(request.Convert());

        return request.ConvertToCouponModel();
    }
}
