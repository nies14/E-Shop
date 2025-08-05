# Discount API (gRPC Service)

The Discount API is a gRPC service that manages discount coupons for products in the E-Shop application. It provides methods for creating, retrieving, updating, and deleting discount coupons based on product names.

## Service Type
**gRPC Service** - Not a REST API

## Service Endpoint
gRPC endpoint (host and port configuration dependent)

## Service Definition

The service implements the `DiscountProtoService` with the following gRPC methods:

### Proto Service: DiscountProtoService

## gRPC Methods

### 1. GetDiscount
Retrieves a discount coupon for a specific product.

**Request Message: GetDiscountRequest**
```protobuf
message GetDiscountRequest {
  string productName = 1;
}
```

**Response Message: CouponModel**
```protobuf
message CouponModel {
  int32 id = 1;
  string productName = 2;
  string description = 3;
  int32 amount = 4;
}
```

**Usage Example (C#):**
```csharp
var request = new GetDiscountRequest { ProductName = "Product1" };
var response = await client.GetDiscountAsync(request);
```

### 2. CreateDiscount
Creates a new discount coupon.

**Request Message: CreateDiscountRequest**
```protobuf
message CreateDiscountRequest {
  CouponModel coupon = 1;
}
```

**Response Message: CouponModel**
Returns the created coupon with assigned ID.

**Usage Example (C#):**
```csharp
var request = new CreateDiscountRequest 
{
  Coupon = new CouponModel
  {
    ProductName = "Product1",
    Description = "10% off",
    Amount = 10
  }
};
var response = await client.CreateDiscountAsync(request);
```

### 3. UpdateDiscount
Updates an existing discount coupon.

**Request Message: UpdateDiscountRequest**
```protobuf
message UpdateDiscountRequest {
  CouponModel coupon = 1;
}
```

**Response Message: CouponModel**
Returns the updated coupon.

**Usage Example (C#):**
```csharp
var request = new UpdateDiscountRequest 
{
  Coupon = new CouponModel
  {
    Id = 1,
    ProductName = "Product1",
    Description = "15% off",
    Amount = 15
  }
};
var response = await client.UpdateDiscountAsync(request);
```

### 4. DeleteDiscount
Deletes a discount coupon by product name.

**Request Message: DeleteDiscountRequest**
```protobuf
message DeleteDiscountRequest {
  string productName = 1;
}
```

**Response Message: DeleteDiscountResponse**
```protobuf
message DeleteDiscountResponse {
  bool success = 1;
}
```

**Usage Example (C#):**
```csharp
var request = new DeleteDiscountRequest { ProductName = "Product1" };
var response = await client.DeleteDiscountAsync(request);
bool wasDeleted = response.Success;
```

## Data Models

### CouponModel
```protobuf
message CouponModel {
  int32 id = 1;           // Unique identifier
  string productName = 2;  // Product name for the discount
  string description = 3;  // Description of the discount
  int32 amount = 4;       // Discount amount (percentage or fixed amount)
}
```

## gRPC Client Integration

### .NET Client Example
```csharp
// Setup gRPC channel
var channel = GrpcChannel.ForAddress("http://localhost:5003");
var client = new DiscountProtoService.DiscountProtoServiceClient(channel);

// Get discount
var getRequest = new GetDiscountRequest { ProductName = "Product1" };
var discount = await client.GetDiscountAsync(getRequest);

// Create discount
var createRequest = new CreateDiscountRequest 
{
  Coupon = new CouponModel
  {
    ProductName = "Product1",
    Description = "10% off",
    Amount = 10
  }
};
var createdDiscount = await client.CreateDiscountAsync(createRequest);
```

### Other Language Clients
gRPC supports multiple programming languages. Generate client code using the proto files:
- **Python**: Use `grpcio-tools`
- **JavaScript**: Use `@grpc/grpc-js`
- **Java**: Use gRPC Java libraries
- **Go**: Use gRPC Go libraries

## Database Integration

The service uses:
- **PostgreSQL** for data persistence
- **Dapper** for data access (lightweight ORM)
- **Database migration** on application startup

## Error Handling

gRPC uses standard status codes:
- **OK** - Successful operation
- **NOT_FOUND** - Coupon not found
- **INVALID_ARGUMENT** - Invalid request parameters
- **INTERNAL** - Server error
- **ALREADY_EXISTS** - Coupon already exists

## Technology Stack

- **gRPC** for service communication
- **PostgreSQL** for data storage
- **Dapper** for data access
- **MediatR** (CQRS Pattern)
- **Serilog** (Logging)
- **Protocol Buffers** for serialization

## Development Notes

The service uses:
- CQRS pattern with MediatR for command/query separation
- PostgreSQL with automatic database migration
- Protocol Buffers for efficient binary serialization
- Dependency injection for service management
- Comprehensive logging with Serilog

## Service Discovery & Load Balancing

For production deployments, consider:
- Service mesh integration (Istio, Linkerd)
- Load balancing strategies
- Health checks
- Circuit breaker patterns

## Security Considerations

- **TLS/SSL**: Enable for production environments
- **Authentication**: Implement token-based auth if needed
- **Authorization**: Add role-based access control
- **Rate Limiting**: Implement to prevent abuse

## Performance Features

- **Binary Protocol**: gRPC uses HTTP/2 with binary serialization
- **Streaming**: Supports bidirectional streaming (not used in current implementation)
- **Connection Multiplexing**: HTTP/2 allows multiple requests over single connection
- **Compression**: Built-in gzip compression support

## Integration Points

- **Catalog API**: Used to validate product names
- **Basket API**: May call this service to apply discounts during checkout
- **Order API**: May call this service to apply discounts to orders

## Monitoring & Observability

Consider implementing:
- gRPC metrics collection
- Distributed tracing with correlation IDs
- Health check endpoints
- Performance monitoring
