using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mappers;

public static class CouponMapper
{
    public static IEnumerable<CouponModel?> Convert(this IEnumerable<Coupon> coupons)
    {
        return coupons == null
            ? Enumerable.Empty<CouponModel>()
            : coupons.Select(coupon => coupon.Convert());
    }

    public static CouponModel? Convert(this Coupon coupon)
    {
        return coupon == null
            ? null
            : new CouponModel
            {
                Id = coupon.Id,
                Amount = coupon.Amount,
                Description = coupon.Description,
                ProductName = coupon.ProductName
            };
    }

    public static Coupon? Convert(this CreateDiscountCommand couponModel)
    {
        return couponModel == null
            ? null
            : new Coupon
            {
                Amount = couponModel.Amount,
                Description = couponModel.Description,
                ProductName = couponModel.ProductName
            };
    }

    public static Coupon? Convert(this UpdateDiscountCommand couponModel)
    {
        return couponModel == null
            ? null
            : new Coupon
            {
                Id = couponModel.Id,
                Amount = couponModel.Amount,
                Description = couponModel.Description,
                ProductName = couponModel.ProductName
            };
    }

    public static CouponModel? ConvertToCouponModel(this CreateDiscountCommand? coupon)
    {
        return coupon == null
            ? null
            : new CouponModel
            {
                Amount = coupon.Amount,
                Description = coupon.Description,
                ProductName = coupon.ProductName
            };
    }

    public static CouponModel? ConvertToCouponModel(this UpdateDiscountCommand? coupon)
    {
        return coupon == null
            ? null
            : new CouponModel
            {
                Amount = coupon.Amount,
                Description = coupon.Description,
                ProductName = coupon.ProductName
            };
    }
}