# E-Shop Microservices

This project is a microservices-based e-commerce application built with .NET 8.0. The current implementation includes a Catalog microservice that manages products, brands, and product types.

## Technologies Used

- .NET 8.0
- MongoDB
- Docker
- MediatR (CQRS pattern)
- Swagger/OpenAPI

## Project Structure

```
E-Shop/
├── Catalog.API/          # REST API endpoints and controllers
├── Catalog.Application/  # Application logic, CQRS commands/queries
├── Catalog.Core/         # Domain entities and interfaces
└── Catalog.Infrastructure/ # Data access and implementations
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker Desktop
- MongoDB (containerized)

### Running the Application

1. Start the containers:
```bash
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

2. Access Swagger UI:
```
http://localhost:8000/swagger
```

## API Endpoints

### Products

#### Get All Products
- **GET** `/api/v1/Catalog/GetAllProducts`
  - Gets all products with optional filtering, sorting, and pagination
  - Query Parameters:
    - `PageIndex` (int): Page number, starts at 1 (default: 1)
    - `PageSize` (int): Number of items per page, max 70 (default: 10)
    - `BrandId` (string): Filter by brand ID
    - `TypeId` (string): Filter by product type ID
    - `Sort` (string): Sort parameter (e.g., "price_asc", "price_desc", "name_asc")
    - `Search` (string): Search term to filter products
  - Example: `/api/v1/Catalog/GetAllProducts?PageIndex=2&PageSize=20&BrandId=1&TypeId=2&Sort=price_asc&Search=gaming`
  - Response Model:
    ```json
    {
      "products": [
        {
          "id": "string",
          "name": "string",
          "description": "string",
          "price": 0.0,
          "pictureUrl": "string",
          "productBrand": "string",
          "productType": "string"
        }
      ]
    }
    ```

#### Get Product By ID
- **GET** `/api/v1/Catalog/GetProductById/{id}`
  - Parameters: id (string)
  - Response Model:
    ```json
    {
      "id": "string",
      "name": "string",
      "description": "string",
      "price": 0.0,
      "pictureUrl": "string",
      "productBrand": "string",
      "productType": "string"
    }
    ```

#### Get Product By Name
- **GET** `/api/v1/Catalog/GetProductByProductName/{productName}`
  - Parameters: productName (string)
  - Response Model: Same as Get Product By ID

#### Get Products By Brand
- **GET** `/api/v1/Catalog/GetProductsByBrandName/{brand}`
  - Parameters: brand (string)
  - Response Model:
    ```json
    {
      "products": [
        {
          "id": "string",
          "name": "string",
          "description": "string",
          "price": 0.0,
          "pictureUrl": "string",
          "productBrand": "string",
          "productType": "string"
        }
      ]
    }
    ```

#### Create Product
- **POST** `/api/v1/Catalog/CreateProduct`
  - Request Model:
    ```json
    {
      "name": "string",
      "description": "string",
      "price": 0.0,
      "pictureUrl": "string",
      "productBrand": "string",
      "productType": "string"
    }
    ```
  - Response Model:
    ```json
    {
      "id": "string",
      "name": "string",
      "description": "string",
      "price": 0.0,
      "pictureUrl": "string",
      "productBrand": "string",
      "productType": "string"
    }
    ```

#### Update Product
- **PUT** `/api/v1/Catalog/UpdateProduct`
  - Request Model:
    ```json
    {
      "id": "string",
      "name": "string",
      "description": "string",
      "price": 0.0,
      "pictureUrl": "string",
      "productBrand": "string",
      "productType": "string"
    }
    ```
  - Response: 200 OK

#### Delete Product
- **DELETE** `/api/v1/Catalog/{id}`
  - Parameters: id (string)
  - Response: 200 OK

### Brands

#### Get All Brands
- **GET** `/api/v1/Catalog/GetAllBrands`
  - Response Model:
    ```json
    {
      "brands": [
        {
          "id": "string",
          "name": "string"
        }
      ]
    }
    ```

### Types

#### Get All Types
- **GET** `/api/v1/Catalog/GetAllTypes`
  - Response Model:
    ```json
    {
      "types": [
        {
          "id": "string",
          "name": "string"
        }
      ]
    }
    ```

## Troubleshooting

### MongoDB Connection Issues

If you're encountering MongoDB connection issues when running the application from Visual Studio:

1. Make sure MongoDB is running in Docker with proper port mapping:
   ```bash
   docker run --name mongodb -d -p 27017:27017 mongo:latest
   ```

2. Verify MongoDB Connection String:
   - When running in Docker: `mongodb://catalogdb:27017`
   - When running on localhost: `mongodb://127.0.0.1:27017`

3. Check MongoDB logs:
   ```bash
   docker logs mongodb
   ```

## Database Configuration

MongoDB connection settings in appsettings.json:
```json
"DatabaseSettings": {
    "ConnectionString": "mongodb://catalogdb:27017",
    "DatabaseName": "CatalogDb",
    "CollectionName": "Products",
    "BrandsCollection": "Brands",
    "TypesCollection": "Types"
}
```

For local development (in appsettings.Development.json):
```json
"DatabaseSettings": {
    "ConnectionString": "mongodb://127.0.0.1:27017",
    "DatabaseName": "CatalogDb",
    "CollectionName": "Products",
    "BrandsCollection": "Brands",
    "TypesCollection": "Types"
}
```

## Docker Support

The application uses Docker Compose with two services:
- **catalogdb**: MongoDB instance
- **catalog.api**: .NET Web API application

## Development

To run the application in development mode:

1. Build the containers:
```bash
docker compose build
```

2. Start the services:
```bash
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

3. Stop the services:
```bash
docker compose down
```