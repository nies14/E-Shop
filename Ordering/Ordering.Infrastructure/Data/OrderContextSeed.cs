using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded!!!");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new()
                {
                    UserName = "tanvir",
                    FirstName = "Tanvir",
                    LastName = "Hassan",
                    EmailAddress = "tanvir@eCommerce.net",
                    AddressLine = "Montreal",
                    Country = "Canada",
                    TotalPrice = 750,
                    State = "QC",
                    ZipCode = "ABC DEF",

                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    CreatedBy = "Tanvir",
                    Expiration = "12/28",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifiedBy = "Tanvir",
                    LastModifiedDate = new DateTime(),
                }
            };
        }
    }
}
