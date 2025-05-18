using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;

namespace Ordering.Application.Mappers
{
    public static class OrderMapper
    {
        public static IEnumerable<OrderResponse> Convert(this IEnumerable<Order> orders)
        {
            return orders == null || !orders.Any()
                ? new List<OrderResponse>()
                : orders.Select(o => o.Convert());
        }
        public static OrderResponse? Convert(this Order order)
        {
            return order == null
                ? null
                : new OrderResponse
                {
                    Id = order.Id,
                    UserName = order.UserName,
                    FirstName = order.FirstName,
                    LastName = order.LastName,
                    AddressLine = order.AddressLine,
                    ZipCode = order.ZipCode,
                    Country = order.Country,
                    EmailAddress = order.EmailAddress,
                    State = order.State,
                    TotalPrice = order.TotalPrice,
                    CardName = order.CardName,
                    CardNumber = order.CardNumber,
                    Cvv = order.Cvv,
                    Expiration = order.Expiration,
                    PaymentMethod = order.PaymentMethod
                };
        }

        public static Order? Convert(this CheckoutOrderCommand checkoutOrderCommand)
        {
            return checkoutOrderCommand == null
                ? null
                : new Order
                {
                    UserName = checkoutOrderCommand.UserName,
                    LastModifiedDate = DateTime.Now,
                    State = checkoutOrderCommand.State,
                    ZipCode = checkoutOrderCommand.ZipCode,
                    PaymentMethod = checkoutOrderCommand.PaymentMethod,
                    TotalPrice = checkoutOrderCommand.TotalPrice,
                    CardName = checkoutOrderCommand.CardName,
                    CardNumber = checkoutOrderCommand.CardNumber,
                    AddressLine = checkoutOrderCommand.AddressLine,
                    Country = checkoutOrderCommand.Country,
                    CreatedDate = DateTime.Now,
                    Cvv = checkoutOrderCommand.Cvv,
                    EmailAddress = checkoutOrderCommand.EmailAddress,
                    Expiration = checkoutOrderCommand.Expiration,
                    FirstName = checkoutOrderCommand.FirstName,
                    LastName = checkoutOrderCommand.LastName
                };
        }

        public static Order? Convert(this Order orderToUpdate, UpdateOrderCommand checkoutOrderCommand)
        {
            if (orderToUpdate == null)
            {
                throw new ArgumentNullException(nameof(orderToUpdate));
            }

            orderToUpdate.UserName = checkoutOrderCommand.UserName;
            orderToUpdate.State = checkoutOrderCommand.State;
            orderToUpdate.ZipCode = checkoutOrderCommand.ZipCode;
            orderToUpdate.TotalPrice = checkoutOrderCommand.TotalPrice;
            orderToUpdate.PaymentMethod = checkoutOrderCommand.PaymentMethod;
            orderToUpdate.CardName = checkoutOrderCommand.CardName;
            orderToUpdate.CardNumber = checkoutOrderCommand.CardNumber;
            orderToUpdate.AddressLine = checkoutOrderCommand.AddressLine;
            orderToUpdate.Country = checkoutOrderCommand.Country;
            orderToUpdate.LastModifiedDate = DateTime.Now;
            orderToUpdate.Cvv = checkoutOrderCommand.Cvv;
            orderToUpdate.EmailAddress = checkoutOrderCommand.EmailAddress;
            orderToUpdate.Expiration = checkoutOrderCommand.Expiration;
            orderToUpdate.FirstName = checkoutOrderCommand.FirstName;
            orderToUpdate.LastName = checkoutOrderCommand.LastName;

            return orderToUpdate;
        }

        public static Order? Convert(this UpdateOrderCommand updateOrderCommand)
        {
            return updateOrderCommand == null
                ? null
                : new Order
                {
                    UserName = updateOrderCommand.UserName,
                    State = updateOrderCommand.State,
                    ZipCode = updateOrderCommand.ZipCode,
                    TotalPrice = updateOrderCommand.TotalPrice,
                    PaymentMethod = updateOrderCommand.PaymentMethod,
                    CardName = updateOrderCommand.CardName,
                    CardNumber = updateOrderCommand.CardNumber,
                    AddressLine = updateOrderCommand.AddressLine,
                    Country = updateOrderCommand.Country,
                    CreatedDate = DateTime.Now,
                    Cvv = updateOrderCommand.Cvv,
                    EmailAddress = updateOrderCommand.EmailAddress,
                    Expiration = updateOrderCommand.Expiration,
                    FirstName = updateOrderCommand.FirstName,
                    LastName = updateOrderCommand.LastName
                };

        }
    }
}
