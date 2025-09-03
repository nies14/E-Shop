using System.Net;
using System.Text;
using System.Text.Json;
using Basket.Application.Commands;
using Basket.Core.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Basket.Tests.Integration;

public class BasketApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BasketApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetBasket_WithNonExistentUser_ReturnsOkWithEmptyBasket()
    {
        // Arrange
        var userName = "nonexistentuser";

        // Act
        var response = await _client.GetAsync($"/api/v1/Basket/GetBasket/{userName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateBasket_WithValidData_ReturnsOk()
    {
        // Arrange
        var command = new CreateShoppingCartCommand("integrationtestuser", new List<ShoppingCartItem>
        {
            new()
            {
                ProductId = "test-product-1",
                ProductName = "Integration Test Product",
                Price = 29.99m,
                Quantity = 2,
                ImageFile = "test.jpg"
            }
        });

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/Basket/CreateBasket", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DeleteBasket_WithExistingUser_ReturnsOk()
    {
        // Arrange
        var userName = "usertoDelete";
        
        // First create a basket
        var createCommand = new CreateShoppingCartCommand(userName, new List<ShoppingCartItem>
        {
            new()
            {
                ProductId = "test-product-delete",
                ProductName = "Product to Delete",
                Price = 10.00m,
                Quantity = 1,
                ImageFile = "delete.jpg"
            }
        });

        var createJson = JsonSerializer.Serialize(createCommand);
        var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/v1/Basket/CreateBasket", createContent);

        // Act - Delete the basket
        var response = await _client.DeleteAsync($"/api/v1/Basket/DeleteBasket/{userName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Checkout_WithInvalidBasket_ReturnsBadRequest()
    {
        // Arrange
        var basketCheckout = new BasketCheckout
        {
            UserName = "nonexistentcheckoutuser",
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

        var json = JsonSerializer.Serialize(basketCheckout);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/Basket/Checkout", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
