using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers;

public static class ShoppingCartMapper
{
    public static ShoppingCartResponse? Convert(this ShoppingCart shoppingCart)
    {
        return shoppingCart == null
            ? null
            : new ShoppingCartResponse
            {
                UserName = shoppingCart.UserName,
                Items = shoppingCart.Items?.Select(s => s.Convert()).ToList()
            };
    }

    public static ShoppingCartItemResponse? Convert(this ShoppingCartItem shoppingCartItem)
    {
        return shoppingCartItem == null
            ? null
            : new ShoppingCartItemResponse
            {
                Price = shoppingCartItem.Price,
                ImageFile = shoppingCartItem.ImageFile,
                ProductId = shoppingCartItem.ProductId,
                ProductName = shoppingCartItem.ProductName,
                Quantity = shoppingCartItem.Quantity
            };
    }
}