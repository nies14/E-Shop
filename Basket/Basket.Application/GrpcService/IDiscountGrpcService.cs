using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService;

public interface IDiscountGrpcService
{
    Task<CouponModel> GetDiscount(string productName);
}
