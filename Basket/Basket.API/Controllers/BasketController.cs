using System.Net;
using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EShop.Logging.Correlation;
using Eventbus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiVersion("1")]
public class BasketController : ApiController
{
    public readonly IMediator _mediator;
    private readonly ILogger<BasketController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICorrelationIdGenerator _correlationIdGenerator;

    public BasketController(IMediator mediator, ILogger<BasketController> logger, IPublishEndpoint publishEndpoint, ICorrelationIdGenerator correlationIdGenerator)
    {
        _mediator = mediator;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _correlationIdGenerator = correlationIdGenerator;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var basket = await _mediator.Send(query);
        return Ok(basket);
    }

    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
    {
        var basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);
    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteBasket(string userName)
    {
        var cmd = new DeleteBasketByUserNameCommand(userName);
        return Ok(await _mediator.Send(cmd));
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        //Get the existing basket with username
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);
        if (basket == null)
        {
            return BadRequest();
        }

        var eventMsg = Convert(basketCheckout);
        eventMsg.TotalPrice = basket.TotalPrice;
        eventMsg.CorrelationId = _correlationIdGenerator.Get();
        await _publishEndpoint.Publish(eventMsg);
        _logger.LogInformation($"Basket Published for {basket.UserName}");
        //remove the basket
        var deleteCmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
        await _mediator.Send(deleteCmd);
        return Accepted();
    }

    #region Helper Methods

    private static BasketCheckoutEvent? Convert(BasketCheckout basketCheckout)
    {
        return basketCheckout == null
            ? null
            : new BasketCheckoutEvent
            {
                UserName = basketCheckout.UserName,
                AddressLine = basketCheckout.AddressLine,
                CardName = basketCheckout.CardName,
                CardNumber = basketCheckout.CardNumber,
                Country = basketCheckout.Country,
                Cvv = basketCheckout.Cvv,
                EmailAddress = basketCheckout.EmailAddress,
                Expiration = basketCheckout.Expiration,
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                PaymentMethod = basketCheckout.PaymentMethod,
                State = basketCheckout.State,
                TotalPrice = basketCheckout.TotalPrice,
                ZipCode = basketCheckout.ZipCode
            };
    }


    #endregion
}
