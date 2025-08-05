# Ordering API

The Ordering API manages customer orders in the E-Shop application. It provides endpoints for retrieving, creating, updating, and deleting orders. This service typically receives checkout events from the Basket API to create new orders.

## Base URL
```
/api/v1/Order
```

## API Endpoints

### 1. Get Orders by Username
**GET** `/{userName}`

Retrieves all orders for a specific user.

**Parameters:**
- `userName` (string, path) - The username to get orders for

**Response:**
- **200 OK** - Returns array of user orders

**Response Model:**
```json
[
  {
    "id": 0,
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
]
```

### 2. Checkout Order (Create Order)
**POST** `/`

Creates a new order. This is typically used for testing purposes as orders are usually created through basket checkout events.

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
- **200 OK** - Returns the created order ID

**Response Model:**
```json
{
  "orderId": 0
}
```

### 3. Update Order
**PUT** `/`

Updates an existing order.

**Request Body:**
```json
{
  "id": 0,
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
- **204 No Content** - Order updated successfully
- **404 Not Found** - Order not found

### 4. Delete Order
**DELETE** `/{id}`

Deletes an order by its ID.

**Parameters:**
- `id` (integer, path) - The order ID to delete

**Response:**
- **204 No Content** - Order deleted successfully
- **404 Not Found** - Order not found

## Data Models

### OrderResponse
```json
{
  "id": "integer",
  "userName": "string (nullable)",
  "totalPrice": "decimal (nullable)",
  "firstName": "string (nullable)",
  "lastName": "string (nullable)",
  "emailAddress": "string (nullable)",
  "addressLine": "string (nullable)",
  "country": "string (nullable)",
  "state": "string (nullable)",
  "zipCode": "string (nullable)",
  "cardName": "string (nullable)",
  "cardNumber": "string (nullable)",
  "expiration": "string (nullable)",
  "cvv": "string (nullable)",
  "paymentMethod": "integer (nullable)"
}
```

### CheckoutOrderCommand
```json
{
  "userName": "string (nullable)",
  "totalPrice": "decimal (nullable)",
  "firstName": "string (nullable)",
  "lastName": "string (nullable)",
  "emailAddress": "string (nullable)",
  "addressLine": "string (nullable)",
  "country": "string (nullable)",
  "state": "string (nullable)",
  "zipCode": "string (nullable)",
  "cardName": "string (nullable)",
  "cardNumber": "string (nullable)",
  "expiration": "string (nullable)",
  "cvv": "string (nullable)",
  "paymentMethod": "integer (nullable)"
}
```

### UpdateOrderCommand
Same structure as CheckoutOrderCommand but includes an `id` field:
```json
{
  "id": "integer",
  "userName": "string (nullable)",
  "totalPrice": "decimal (nullable)",
  // ... rest of fields same as CheckoutOrderCommand
}
```

## Error Handling

All endpoints return appropriate HTTP status codes:
- **200 OK** - Successful operation
- **204 No Content** - Successful update/delete operation
- **400 Bad Request** - Invalid request data
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

## Payment Methods

The `paymentMethod` field accepts integer values representing different payment types:
- Specific values depend on business requirements and payment gateway integration

## Technology Stack

- ASP.NET Core Web API
- SQL Server (Relational Database)
- Entity Framework Core
- MediatR (CQRS Pattern)
- Serilog (Logging)

## Development Notes

The API uses:
- CQRS pattern with MediatR for command/query separation
- Entity Framework Core for data persistence
- Domain-driven design principles
- Dependency injection for service management
- Comprehensive logging with Serilog

## Integration Points

- **Basket API**: Receives checkout events to create orders automatically
- **Message Bus**: Listens for BasketCheckoutEvent messages
- **Payment Services**: Integrates with payment gateways (implementation dependent)
- **Email Services**: Sends order confirmation emails (implementation dependent)

## Event Handling

The Ordering API typically includes message bus consumers that listen for:
- **BasketCheckoutEvent**: Creates new orders from basket checkout data
- **PaymentProcessedEvent**: Updates order status after payment processing

## Security Considerations

- Sensitive payment information (card numbers, CVV) should be properly encrypted
- PCI compliance requirements for payment data handling
- User authorization to ensure users can only access their own orders
- Audit logging for order modifications
