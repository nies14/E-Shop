using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Discount.Grpc.Protos;
using FluentAssertions;
using Moq;
using Xunit;

namespace Basket.Tests.Handlers;

public class CreateShoppingCartCommandHandlerTests
{
    private readonly Mock<IBasketRepository> _basketRepositoryMock;
    private readonly Mock<IDiscountGrpcService> _discountGrpcServiceMock;
    private readonly CreateShoppingCartCommandHandler _handler;

    public CreateShoppingCartCommandHandlerTests()
    {
        _basketRepositoryMock = new Mock<IBasketRepository>();
        _discountGrpcServiceMock = new Mock<IDiscountGrpcService>();
        _handler = new CreateShoppingCartCommandHandler(
            _basketRepositoryMock.Object,
            _discountGrpcServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesShoppingCart()
    {
        // Arrange
        var command = new CreateShoppingCartCommand("testuser", new List<ShoppingCartItem>
        {
            new() { ProductId = "1", ProductName = "Test Product", Price = 100.00m, Quantity = 1, ImageFile = "test.jpg" }
        });

        var expectedShoppingCart = new ShoppingCart
        {
            UserName = command.UserName,
            Items = command.Items
        };

        _discountGrpcServiceMock.Setup(d => d.GetDiscount(It.IsAny<string>()))
                                .ReturnsAsync((CouponModel?)null);

        _basketRepositoryMock.Setup(b => b.UpdateBasket(It.IsAny<ShoppingCart>()))
                            .ReturnsAsync(expectedShoppingCart);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.UserName.Should().Be(command.UserName);
        result.Items.Should().HaveCount(1);
        result.Items.First().ProductName.Should().Be("Test Product");

        _basketRepositoryMock.Verify(b => b.UpdateBasket(
            It.Is<ShoppingCart>(sc => 
                sc.UserName == command.UserName && 
                sc.Items.Count == 1)), Times.Once);

        _discountGrpcServiceMock.Verify(d => d.GetDiscount("Test Product"), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDiscountAvailable_AppliesDiscount()
    {
        // Arrange
        var command = new CreateShoppingCartCommand("testuser", new List<ShoppingCartItem>
        {
            new() { ProductId = "1", ProductName = "Test Product", Price = 100.00m, Quantity = 1, ImageFile = "test.jpg" }
        });

        var discount = new CouponModel
        {
            ProductName = "Test Product",
            Amount = 10,
            Description = "Test Discount"
        };

        var expectedShoppingCart = new ShoppingCart
        {
            UserName = command.UserName,
            Items = command.Items
        };

        _discountGrpcServiceMock.Setup(d => d.GetDiscount("Test Product"))
                                .ReturnsAsync(discount);

        _basketRepositoryMock.Setup(b => b.UpdateBasket(It.IsAny<ShoppingCart>()))
                            .ReturnsAsync(expectedShoppingCart);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        
        // Verify discount was applied
        command.Items.First().Price.Should().Be(90.00m); // 100 - 10 discount

        _basketRepositoryMock.Verify(b => b.UpdateBasket(
            It.Is<ShoppingCart>(sc => 
                sc.Items.First().Price == 90.00m)), Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleItems_AppliesDiscountsToEachItem()
    {
        // Arrange
        var command = new CreateShoppingCartCommand("testuser", new List<ShoppingCartItem>
        {
            new() { ProductId = "1", ProductName = "Product 1", Price = 100.00m, Quantity = 1, ImageFile = "img1.jpg" },
            new() { ProductId = "2", ProductName = "Product 2", Price = 50.00m, Quantity = 2, ImageFile = "img2.jpg" }
        });

        var discount1 = new CouponModel { ProductName = "Product 1", Amount = 15 };
        var discount2 = new CouponModel { ProductName = "Product 2", Amount = 5 };

        _discountGrpcServiceMock.Setup(d => d.GetDiscount("Product 1")).ReturnsAsync(discount1);
        _discountGrpcServiceMock.Setup(d => d.GetDiscount("Product 2")).ReturnsAsync(discount2);

        var expectedShoppingCart = new ShoppingCart
        {
            UserName = command.UserName,
            Items = command.Items
        };

        _basketRepositoryMock.Setup(b => b.UpdateBasket(It.IsAny<ShoppingCart>()))
                            .ReturnsAsync(expectedShoppingCart);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        
        // Verify discounts were applied
        command.Items[0].Price.Should().Be(85.00m); // 100 - 15
        command.Items[1].Price.Should().Be(45.00m); // 50 - 5

        _discountGrpcServiceMock.Verify(d => d.GetDiscount("Product 1"), Times.Once);
        _discountGrpcServiceMock.Verify(d => d.GetDiscount("Product 2"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsNull_ReturnsNull()
    {
        // Arrange
        var command = new CreateShoppingCartCommand("testuser", new List<ShoppingCartItem>
        {
            new() { ProductId = "1", ProductName = "Test Product", Price = 100.00m, Quantity = 1, ImageFile = "test.jpg" }
        });

        _discountGrpcServiceMock.Setup(d => d.GetDiscount(It.IsAny<string>()))
                                .ReturnsAsync((CouponModel?)null);

        _basketRepositoryMock.Setup(b => b.UpdateBasket(It.IsAny<ShoppingCart>()))
                            .ReturnsAsync((ShoppingCart?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
