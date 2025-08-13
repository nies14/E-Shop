# Gemini CLI for E-Shop Microservices

This directory contains a custom Gemini CLI tool for the E-Shop microservices project.

## Setup

1. **Get Gemini API Key:**
   - Go to [Google AI Studio](https://aistudio.google.com/)
   - Create an API key
   - Copy the `.env.example` to `.env`
   - Add your API key to `.env`

2. **Install Dependencies:**
   ```bash
   npm install
   ```

## Usage

### Available Commands

```bash
# Analyze the entire project structure
npm run analyze

# Generate API documentation for a specific service
npm run docs Catalog
npm run docs Basket
npm run docs Ordering
npm run docs Discount

# Generate test suggestions for a service
npm run tests Catalog

# Optimize Dockerfiles
npm run optimize-docker

# Show help
npm run gemini help
```

### Direct CLI Usage

```bash
# Using the CLI directly
node gemini-cli.js analyze
node gemini-cli.js docs Catalog
node gemini-cli.js tests Basket
node gemini-cli.js optimize-docker
```

## Features

- **Project Analysis**: AI-powered analysis of your microservices architecture
- **Documentation Generation**: Automatic API documentation generation
- **Test Suggestions**: AI-generated test case recommendations
- **Docker Optimization**: Dockerfile analysis and optimization suggestions
- **Service-Specific Commands**: Targeted analysis for individual microservices

## Configuration

The CLI uses the following environment variables:

```env
GEMINI_API_KEY=your_gemini_api_key_here
CATALOG_API_URL=http://localhost:8000
BASKET_API_URL=http://localhost:8001
ORDERING_API_URL=http://localhost:8002
DISCOUNT_API_URL=http://localhost:8003
```

## Examples

### Analyze Project
```bash
npm run analyze
```
This will provide insights on:
- Architecture patterns used
- Potential improvements
- Missing components
- Best practices recommendations

### Generate Documentation
```bash
npm run docs Catalog
```
Creates comprehensive API documentation including:
- API endpoints with HTTP methods
- Request/response models
- Status codes
- Usage examples

### Test Suggestions
```bash
npm run tests Basket
```
Provides test recommendations including:
- Unit test cases for controllers
- Integration test scenarios
- Test data setup
- Mock configurations

## Integration with VS Code

You can also run these commands from VS Code terminal or add them to your VS Code tasks.
