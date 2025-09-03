using Basket.Application.Handlers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Basket.Tests.Handlers;

public class GetBasketByUserNameHandlerTests
{
    private readonly Mock<IBasketRepository> _basketRepositoryMock;
    private readonly GetBasketByUserNameHandler _handler;

    public GetBasketByUserNameHandlerTests()
    {
        _basketRepositoryMock = new Mock<IBasketRepository>();
        _handler = new GetBasketByUserNameHandler(_basketRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingUser_ReturnsShoppingCartResponse()
    {
        // Arrange
        var userName = "testuser";
        var shoppingCart = new ShoppingCart { UserName = userName, Items = new List<ShoppingCartItem>() };
        _basketRepositoryMock.Setup(r => r.GetBasket(userName)).ReturnsAsync(shoppingCart);

        var query = new GetBasketByUserNameQuery(userName);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result!.UserName.Should().Be(userName);
        _basketRepositoryMock.Verify(r => r.GetBasket(userName), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ReturnsNull()
    {
        // Arrange
        var userName = "nonexistentuser";
        _basketRepositoryMock.Setup(r => r.GetBasket(userName)).ReturnsAsync((ShoppingCart?)null);

        var query = new GetBasketByUserNameQuery(userName);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().BeNull();
        _basketRepositoryMock.Verify(r => r.GetBasket(userName), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var userName = "erroruser";
        _basketRepositoryMock.Setup(r => r.GetBasket(userName)).ThrowsAsync(new InvalidOperationException("Repository error"));

        var query = new GetBasketByUserNameQuery(userName);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, default));
        _basketRepositoryMock.Verify(r => r.GetBasket(userName), Times.Once);
    }
}
