using System.Text;
using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers;

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException("Discount create request can not be null");
        }

        var coupon = await _discountRepository.UpdateDiscount(request.Convert());

        return request.ConvertToCouponModel();

    }
}
