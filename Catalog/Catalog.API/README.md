# Catalog API

The Catalog API is responsible for managing products, brands, and product types in the E-Shop application. It provides endpoints for CRUD operations on products and read operations for brands and types.

## Base URL
```
/api/v1/Catalog
```

## API Endpoints

### 1. Get Product by ID
**GET** `/GetProductById/{id}`

Retrieves a specific product by its ID.

**Parameters:**
- `id` (string, path) - The product ID

**Response:**
- **200 OK** - Returns the product details
- **404 Not Found** - Product not found

**Response Model:**
```json
{
  "id": "string",
  "name": "string",
  "summary": "string",
  "description": "string",
  "imageFile": "string",
  "brands": {
    "id": "string",
    "name": "string"
  },
  "types": {
    "id": "string",
    "name": "string"
  },
  "price": 0.00
}
```

### 2. Get Product by Name
**GET** `/GetProductByProductName/{productName}`

Retrieves products that match the given product name.

**Parameters:**
- `productName` (string, path) - The product name to search for

**Response:**
- **200 OK** - Returns array of matching products

**Response Model:**
```json
[
  {
    "id": "string",
    "name": "string",
    "summary": "string",
    "description": "string",
    "imageFile": "string",
    "brands": {
      "id": "string",
      "name": "string"
    },
    "types": {
      "id": "string",
      "name": "string"
    },
    "price": 0.00
  }
]
```

### 3. Get All Products
**GET** `/GetAllProducts`

Retrieves all products with optional filtering and pagination.

**Query Parameters:**
- `PageIndex` (int, optional) - Page number (default: 1)
- `PageSize` (int, optional) - Items per page (default: 10, max: 70)
- `BrandId` (string, optional) - Filter by brand ID
- `TypeId` (string, optional) - Filter by type ID
- `Sort` (string, optional) - Sort criteria
- `Search` (string, optional) - Search term

**Response:**
- **200 OK** - Returns array of products

**Response Model:** Same as Get Product by Name

### 4. Get All Brands
**GET** `/GetAllBrands`

Retrieves all available product brands.

**Response:**
- **200 OK** - Returns array of brands

**Response Model:**
```json
[
  {
    "id": "string",
    "name": "string"
  }
]
```

### 5. Get All Types
**GET** `/GetAllTypes`

Retrieves all available product types.

**Response:**
- **200 OK** - Returns array of types

**Response Model:**
```json
[
  {
    "id": "string",
    "name": "string"
  }
]
```

### 6. Get Products by Brand Name
**GET** `/GetProductsByBrandName/{brand}`

Retrieves all products belonging to a specific brand.

**Parameters:**
- `brand` (string, path) - The brand name

**Response:**
- **200 OK** - Returns array of products

**Response Model:** Same as Get Product by Name

### 7. Create Product
**POST** `/CreateProduct`

Creates a new product.

**Request Body:**
```json
{
  "name": "string",
  "summary": "string",
  "description": "string",
  "imageFile": "string",
  "price": 0.00,
  "brands": {
    "id": "string",
    "name": "string"
  },
  "types": {
    "id": "string",
    "name": "string"
  }
}
```

**Response:**
- **200 OK** - Returns the created product

**Response Model:** Same as Get Product by ID

### 8. Update Product
**PUT** `/UpdateProduct`

Updates an existing product.

**Request Body:**
```json
{
  "id": "string",
  "name": "string",
  "summary": "string",
  "description": "string",
  "imageFile": "string",
  "price": 0.00,
  "brands": {
    "id": "string",
    "name": "string"
  },
  "types": {
    "id": "string",
    "name": "string"
  }
}
```

**Response:**
- **200 OK** - Returns boolean indicating success

### 9. Delete Product
**DELETE** `/DeleteProduct/{id}`

Deletes a product by its ID.

**Parameters:**
- `id` (string, path) - The product ID to delete

**Response:**
- **200 OK** - Returns boolean indicating success

## Error Handling

All endpoints return appropriate HTTP status codes:
- **200 OK** - Successful operation
- **400 Bad Request** - Invalid request data
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

## Technology Stack

- ASP.NET Core Web API
- MongoDB (Document Database)
- MediatR (CQRS Pattern)
- Serilog (Logging)
- API Versioning

## Development Notes

The API uses:
- CQRS pattern with MediatR for command/query separation
- MongoDB for data persistence
- Dependency injection for service management
- API versioning (v1.0)
- Comprehensive logging with Serilog
- CORS enabled for cross-origin requests
