using Eventbus.Messages.Events;
using MassTransit;
using Ordering.Application.Commands;
using MediatR;

namespace Ordering.API.EventbusConsumer;

public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<BasketOrderingConsumer> _logger;

    public BasketOrderingConsumer(IMediator mediator, ILogger<BasketOrderingConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        using var scope = _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}", context.Message.CorrelationId);
        var cmd = Convert(context.Message);
        var result = await _mediator.Send(cmd);
        _logger.LogInformation("Basket Checkout Event completed!!!");
    }

    #region Helper Methods

    private static CheckoutOrderCommand? Convert(BasketCheckoutEvent basketCheckoutEvent)
    {
        return basketCheckoutEvent == null
            ? null
            : new CheckoutOrderCommand
            {
                UserName = basketCheckoutEvent.UserName,
                AddressLine = basketCheckoutEvent.AddressLine,
                CardName = basketCheckoutEvent.CardName,
                CardNumber = basketCheckoutEvent.CardNumber,
                Country = basketCheckoutEvent.Country,
                Cvv = basketCheckoutEvent.Cvv,
                EmailAddress = basketCheckoutEvent.EmailAddress,
                Expiration = basketCheckoutEvent.Expiration,
                FirstName = basketCheckoutEvent.FirstName,
                LastName = basketCheckoutEvent.LastName,
                PaymentMethod = basketCheckoutEvent.PaymentMethod,
                State = basketCheckoutEvent.State,
                TotalPrice = basketCheckoutEvent.TotalPrice,
                ZipCode = basketCheckoutEvent.ZipCode
            };
    }

    #endregion
}
