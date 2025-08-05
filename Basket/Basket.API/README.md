# Basket API

The Basket API manages shopping carts for users in the E-Shop application. It provides endpoints for creating, retrieving, updating, and deleting user shopping carts, as well as checkout functionality with event publishing.

## Base URL
```
/api/v1/Basket
```

## API Endpoints

### 1. Get Basket by Username
**GET** `/GetBasket/{userName}`

Retrieves the shopping cart for a specific user.

**Parameters:**
- `userName` (string, path) - The username to get the basket for

**Response:**
- **200 OK** - Returns the user's shopping cart

**Response Model:**
```json
{
  "userName": "string",
  "items": [
    {
      "quantity": 0,
      "imageFile": "string",
      "price": 0.00,
      "productId": "string",
      "productName": "string"
    }
  ],
  "totalPrice": 0.00
}
```

### 2. Create/Update Basket
**POST** `/CreateBasket`

Creates a new shopping cart or updates an existing one for a user.

**Request Body:**
```json
{
  "userName": "string",
  "items": [
    {
      "quantity": 0,
      "imageFile": "string",
      "price": 0.00,
      "productId": "string",
      "productName": "string"
    }
  ]
}
```

**Response:**
- **200 OK** - Returns the created/updated shopping cart

**Response Model:**
```json
{
  "userName": "string",
  "items": [
    {
      "quantity": 0,
      "imageFile": "string",
      "price": 0.00,
      "productId": "string",
      "productName": "string"
    }
  ],
  "totalPrice": 0.00
}
```

### 3. Delete Basket
**DELETE** `/DeleteBasket/{userName}`

Deletes the shopping cart for a specific user.

**Parameters:**
- `userName` (string, path) - The username to delete the basket for

**Response:**
- **200 OK** - Returns success confirmation

### 4. Checkout
**POST** `/Checkout`

Processes the checkout for a user's basket and publishes a checkout event to the message bus.

**Request Body:**
```json
{
  "userName": "string",
  "totalPrice": 0.00,
  "firstName": "string",
  "lastName": "string",
  "emailAddress": "string",
  "addressLine": "string",
  "country": "string",
  "state": "string",
  "zipCode": "string",
  "cardName": "string",
  "cardNumber": "string",
  "expiration": "string",
  "cvv": "string",
  "paymentMethod": 0
}
```

**Response:**
- **202 Accepted** - Checkout process initiated successfully
- **400 Bad Request** - Basket not found or invalid data

## Data Models

### ShoppingCartResponse
```json
{
  "userName": "string",
  "items": [ShoppingCartItemResponse],
  "totalPrice": "decimal (calculated)"
}
```

### ShoppingCartItemResponse
```json
{
  "quantity": "integer",
  "imageFile": "string",
  "price": "decimal",
  "productId": "string",
  "productName": "string"
}
```

### BasketCheckout
```json
{
  "userName": "string",
  "totalPrice": "decimal",
  "firstName": "string",
  "lastName": "string",
  "emailAddress": "string",
  "addressLine": "string",
  "country": "string",
  "state": "string",
  "zipCode": "string",
  "cardName": "string",
  "cardNumber": "string",
  "expiration": "string",
  "cvv": "string",
  "paymentMethod": "integer"
}
```

## Event Publishing

The Basket API publishes events to a message bus during checkout:

### BasketCheckoutEvent
Published when a user completes checkout. Contains all basket and customer information needed for order processing.

**Event Properties:**
- All BasketCheckout properties
- `CorrelationId` - Unique identifier for tracking the transaction
- Automatically calculated `TotalPrice` from basket items

## Error Handling

All endpoints return appropriate HTTP status codes:
- **200 OK** - Successful operation
- **202 Accepted** - Checkout accepted for processing
- **400 Bad Request** - Invalid request data or basket not found
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

## Technology Stack

- ASP.NET Core Web API
- Redis (Caching/Session Storage)
- MediatR (CQRS Pattern)
- MassTransit (Message Bus)
- Serilog (Logging)
- API Versioning (v1)
- Correlation ID tracking

## Development Notes

The API uses:
- CQRS pattern with MediatR for command/query separation
- Redis for basket persistence and caching
- Event-driven architecture with MassTransit for checkout processing
- Correlation ID generation for distributed tracing
- Dependency injection for service management
- CORS enabled for cross-origin requests
- Background event publishing for asynchronous order processing

## Integration Points

- **Catalog API**: References product information in basket items
- **Ordering API**: Receives checkout events for order creation
- **Message Bus**: Publishes basket checkout events for downstream processing
