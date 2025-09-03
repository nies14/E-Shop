using System.Net;
using Basket.API.Controllers;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EShop.Logging.Correlation;
using Eventbus.Messages.Events;
using FluentAssertions;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Basket.Tests.Controllers;

public class BasketControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<BasketController>> _loggerMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<ICorrelationIdGenerator> _correlationIdGeneratorMock;
    private readonly BasketController _controller;

    public BasketControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<BasketController>>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _correlationIdGeneratorMock = new Mock<ICorrelationIdGenerator>();

        _controller = new BasketController(
            _mediatorMock.Object,
            _loggerMock.Object,
            _publishEndpointMock.Object,
            _correlationIdGeneratorMock.Object);
    }

    [Fact]
    public async Task GetBasket_WithValidUserName_ReturnsOkWithBasket()
    {
        // Arrange
        var userName = "testuser";
        var expectedBasket = new ShoppingCartResponse
        {
            UserName = userName,
            Items = new List<ShoppingCartItemResponse>
            {
                new() { ProductId = "1", ProductName = "Test Product", Price = 100.50m, Quantity = 1, ImageFile = "test.jpg" }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBasketByUserNameQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedBasket);

        // Act
        var result = await _controller.GetBasket(userName);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(expectedBasket);

        _mediatorMock.Verify(m => m.Send(
            It.Is<GetBasketByUserNameQuery>(q => q.UserName == userName),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBasket_WithValidCommand_ReturnsOkWithUpdatedBasket()
    {
        // Arrange
        var command = new CreateShoppingCartCommand(
            "testuser",
            new List<ShoppingCartItem>
            {
                new() { ProductId = "1", ProductName = "Test Product", Price = 50.00m, Quantity = 2, ImageFile = "test.jpg" }
            });

        var expectedResponse = new ShoppingCartResponse
        {
            UserName = command.UserName,
            Items = new List<ShoppingCartItemResponse>
            {
                new() { ProductId = "1", ProductName = "Test Product", Price = 50.00m, Quantity = 2, ImageFile = "test.jpg" }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateShoppingCartCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.UpdateBasket(command);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(expectedResponse);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteBasket_WithValidUserName_ReturnsOk()
    {
        // Arrange
        var userName = "testuser";
        var expectedResult = Unit.Value;

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBasketByUserNameCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.DeleteBasket(userName);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().Be(expectedResult);

        _mediatorMock.Verify(m => m.Send(
            It.Is<DeleteBasketByUserNameCommand>(c => c.UserName == userName),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Checkout_WithValidBasket_PublishesEventAndDeletesBasket()
    {
        // Arrange
        var basketCheckout = new BasketCheckout
        {
            UserName = "testuser",
            FirstName = "John",
            LastName = "Doe",
            EmailAddress = "john.doe@example.com",
            AddressLine = "123 Main St",
            Country = "USA",
            State = "CA",
            ZipCode = "12345",
            CardName = "John Doe",
            CardNumber = "1234567890123456",
            Expiration = "12/25",
            Cvv = "123",
            PaymentMethod = 1,
            TotalPrice = 100.00m
        };

        var existingBasket = new ShoppingCartResponse
        {
            UserName = basketCheckout.UserName,
            Items = new List<ShoppingCartItemResponse>()
        };

        var correlationId = Guid.NewGuid().ToString();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBasketByUserNameQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingBasket);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBasketByUserNameCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Unit.Value);

        _correlationIdGeneratorMock.Setup(c => c.Get()).Returns(correlationId);

        // Act
        var result = await _controller.Checkout(basketCheckout);

        // Assert
        result.Should().NotBeNull();
        var acceptedResult = result.Should().BeOfType<AcceptedResult>().Subject;
        acceptedResult.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

        // Verify basket query was called
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetBasketByUserNameQuery>(q => q.UserName == basketCheckout.UserName),
            It.IsAny<CancellationToken>()), Times.Once);

        // Verify event was published
        _publishEndpointMock.Verify(p => p.Publish(
            It.Is<BasketCheckoutEvent>(e => 
                e.UserName == basketCheckout.UserName &&
                e.CorrelationId == correlationId),
            It.IsAny<CancellationToken>()), Times.Once);

        // Verify basket was deleted
        _mediatorMock.Verify(m => m.Send(
            It.Is<DeleteBasketByUserNameCommand>(c => c.UserName == basketCheckout.UserName),
            It.IsAny<CancellationToken>()), Times.Once);

        // Verify correlation ID was generated
        _correlationIdGeneratorMock.Verify(c => c.Get(), Times.Once);
    }

    [Fact]
    public async Task Checkout_WithNonExistentBasket_ReturnsBadRequest()
    {
        // Arrange
        var basketCheckout = new BasketCheckout
        {
            UserName = "nonexistentuser",
            FirstName = "John",
            LastName = "Doe",
            EmailAddress = "john.doe@example.com",
            AddressLine = "123 Main St",
            Country = "USA",
            State = "CA",
            ZipCode = "12345",
            CardName = "John Doe",
            CardNumber = "1234567890123456",
            Expiration = "12/25",
            Cvv = "123",
            PaymentMethod = 1,
            TotalPrice = 100.00m
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBasketByUserNameQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ShoppingCartResponse?)null);

        // Act
        var result = await _controller.Checkout(basketCheckout);

        // Assert
        result.Should().NotBeNull();
        var badRequestResult = result.Should().BeOfType<BadRequestResult>().Subject;
        badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        // Verify no event was published
        _publishEndpointMock.Verify(p => p.Publish(
            It.IsAny<BasketCheckoutEvent>(),
            It.IsAny<CancellationToken>()), Times.Never);

        // Verify no deletion occurred
        _mediatorMock.Verify(m => m.Send(
            It.IsAny<DeleteBasketByUserNameCommand>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetBasket_WithInvalidUserName_StillCallsMediator(string userName)
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBasketByUserNameQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ShoppingCartResponse?)null);

        // Act
        var result = await _controller.GetBasket(userName);

        // Assert
        result.Should().NotBeNull();
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetBasketByUserNameQuery>(q => q.UserName == userName),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
