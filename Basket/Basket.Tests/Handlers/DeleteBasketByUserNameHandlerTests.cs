using Basket.Application.Commands;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Basket.Tests.Handlers;

public class DeleteBasketByUserNameHandlerTests
{
    private readonly Mock<IBasketRepository> _basketRepositoryMock;
    private readonly DeleteBasketByUserNameHandler _handler;

    public DeleteBasketByUserNameHandlerTests()
    {
        _basketRepositoryMock = new Mock<IBasketRepository>();
        _handler = new DeleteBasketByUserNameHandler(_basketRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidUserName_DeletesBasketAndReturnsUnit()
    {
        // Arrange
        var userName = "testuser";
        var command = new DeleteBasketByUserNameCommand(userName);
        _basketRepositoryMock.Setup(r => r.DeleteBasket(userName)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().Be(Unit.Value);
        _basketRepositoryMock.Verify(r => r.DeleteBasket(userName), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_DoesNotThrowAndReturnsUnit()
    {
        // Arrange
        var userName = "nonexistentuser";
        var command = new DeleteBasketByUserNameCommand(userName);
        _basketRepositoryMock.Setup(r => r.DeleteBasket(userName)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().Be(Unit.Value);
        _basketRepositoryMock.Verify(r => r.DeleteBasket(userName), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var userName = "erroruser";
        var command = new DeleteBasketByUserNameCommand(userName);
        _basketRepositoryMock.Setup(r => r.DeleteBasket(userName)).ThrowsAsync(new InvalidOperationException("Repository error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
        _basketRepositoryMock.Verify(r => r.DeleteBasket(userName), Times.Once);
    }
}
