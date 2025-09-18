# E-Commerce Microservices API Testing with Postman

This directory contains a comprehensive Postman collection for testing the e-commerce microservices architecture. The collection includes tests for all four microservices: Authentication, Product, Order, and Stock services.

## Files Included

- `ecommerce-microservices-api-tests.json` - Main Postman collection with all API tests
- `ecommerce-microservices-environment.json` - Environment variables for the collection
- `README.md` - This documentation file

## Prerequisites

1. **Running Services**: Ensure all microservices are running via Docker Compose:
   ```bash
   cd /path/to/ecommerce-microservices-architecture
   docker-compose up -d
   ```

2. **Postman**: Install Postman Desktop application or use Postman Web

3. **Service Health**: Verify all services are healthy:
   ```bash
   docker-compose ps
   ```

## Import Instructions

### 1. Import the Collection
1. Open Postman
2. Click "Import" button
3. Select `ecommerce-microservices-api-tests.json`
4. Click "Import"

### 2. Import the Environment
1. Click the gear icon (⚙️) in the top right to manage environments
2. Click "Import"
3. Select `ecommerce-microservices-environment.json`
4. Click "Import"
5. Select "E-Commerce Microservices Environment" from the environment dropdown

## Service Endpoints

| Service | Direct URL | Gateway URL | Port |
|---------|------------|-------------|------|
| API Gateway | N/A | http://localhost:8000 | 8000 |
| Auth API | http://localhost:7001 | http://localhost:8000/auth | 7001 |
| Product API | http://localhost:7002 | http://localhost:8000/products | 7002 |
| Order API | http://localhost:7003 | http://localhost:8000/orders | 7003 |
| Stock API | http://localhost:7004 | http://localhost:8000/stocks | 7004 |

## Collection Structure

### 1. Authentication Service
- **Register User** - Create a new user account
- **Login User** - Authenticate and receive JWT token
- **Health Check (Protected)** - Test JWT authentication

### 2. Product Service (Requires Authentication)
- **Create Product** - Add a new product
- **Get All Products** - Retrieve all products
- **Get Product by ID** - Retrieve specific product
- **Update Product** - Modify existing product
- **Delete Product** - Remove product

### 3. Order Service
- **Create Order** - Place a new order
- **Get Orders by User ID** - Retrieve user's orders
- **Get Order by ID** - Retrieve specific order

### 4. Stock Service
- **Get All Stocks** - Retrieve all stock records
- **Get Stock by Product ID** - Check stock for specific product
- **Update Stock Quantity** - Modify stock levels
- **Reserve Stock** - Reserve stock for orders
- **Release Stock** - Release reserved stock

### 5. Health Checks
- **Gateway Health** - Test API Gateway availability

### 6. End-to-End Test Scenario
Complete workflow testing:
1. Register New User
2. Create New Product
3. Check Stock for Product
4. Reserve Stock
5. Create Order
6. Verify Order

## Usage Instructions

### Basic Testing

1. **Start with Authentication**:
   - Run "Register User" or "Login User" to get an authentication token
   - The token is automatically stored in environment variables

2. **Test Individual Services**:
   - Use the organized folders to test each service
   - Most Product API endpoints require authentication

3. **Run End-to-End Scenario**:
   - Execute the "End-to-End Test Scenario" folder in order
   - This simulates a complete user journey

### Running Collections

#### Option 1: Manual Execution
- Select individual requests and click "Send"
- Monitor responses and test results

#### Option 2: Collection Runner
1. Click on the collection name
2. Click "Run collection"
3. Select specific folders or run entire collection
4. Configure iterations and delays
5. Click "Run E-Commerce Microservices API Tests"

#### Option 3: Newman (CLI)
```bash
# Install Newman
npm install -g newman

# Run the collection
newman run ecommerce-microservices-api-tests.json \
  -e ecommerce-microservices-environment.json \
  --reporters cli,html \
  --reporter-html-export results.html
```

## Environment Variables

### Default Values
The environment includes default values for testing:

| Variable | Default Value | Description |
|----------|---------------|-------------|
| `gateway_url` | http://localhost:8000 | API Gateway URL |
| `user_email` | test@example.com | Test user email |
| `user_password` | TestPassword123! | Test user password |
| `product_name` | Sample Product | Default product name |
| `product_price` | 99.99 | Default product price |

### Dynamic Variables
These are set automatically during test execution:
- `auth_token` - JWT token from authentication
- `product_id` - Created product ID
- `order_id` - Created order ID
- `test_product_id` - Product ID for E2E tests
- `test_order_id` - Order ID for E2E tests

## Test Assertions

Each request includes comprehensive test assertions:

### Response Status Tests
- Validates correct HTTP status codes
- Ensures proper error handling

### Response Content Tests
- Verifies response structure
- Validates required fields
- Checks data types

### Performance Tests
- Monitors response times
- Ensures acceptable performance thresholds

### Business Logic Tests
- Validates authentication flows
- Ensures proper authorization
- Tests data consistency

## Troubleshooting

### Common Issues

1. **Services Not Running**
   ```bash
   # Check service status
   docker-compose ps
   
   # Restart if needed
   docker-compose restart
   ```

2. **Authentication Failures**
   - Ensure you've run "Register User" or "Login User" first
   - Check that `auth_token` environment variable is set
   - Verify token hasn't expired

3. **Network Errors**
   - Confirm Docker containers are running
   - Check port availability (8000-8004)
   - Verify no firewall blocking

4. **Database Connection Issues**
   ```bash
   # Check database containers
   docker-compose logs postgres-auth
   docker-compose logs postgres-product
   docker-compose logs postgres-order
   docker-compose logs postgres-stock
   ```

### Debug Mode
Enable detailed logging by:
1. Opening Postman Console (View > Show Postman Console)
2. Running requests to see detailed logs
3. Checking environment variable values

## Advanced Usage

### Custom Test Scripts
You can modify the test scripts in each request to:
- Add custom validations
- Extract additional data
- Implement complex test scenarios

### Environment Customization
Create additional environments for:
- Different deployment stages (dev, staging, prod)
- Load testing scenarios
- Integration with CI/CD pipelines

### Automated Testing
Integrate with CI/CD:
```yaml
# GitHub Actions example
- name: Run API Tests
  run: |
    newman run postman-collection/ecommerce-microservices-api-tests.json \
      -e postman-collection/ecommerce-microservices-environment.json \
      --reporters cli,junit \
      --reporter-junit-export results.xml
```

## Performance Testing

The collection includes response time assertions:
- Authentication: < 3000ms
- Product operations: < 2000ms
- Order operations: < 5000ms
- Stock operations: < 3000ms
- Health checks: < 1000ms

## Security Testing

Authentication and authorization tests:
- JWT token validation
- Protected endpoint access
- Token expiration handling
- Unauthorized access prevention

## Support

For issues or questions:
1. Check Docker container logs
2. Verify environment configuration
3. Review Postman console output
4. Ensure all prerequisites are met

## Contributing

When adding new API endpoints:
1. Add corresponding Postman requests
2. Include comprehensive test assertions
3. Update environment variables if needed
4. Document any new prerequisites
5. Test the complete flow