# E-Commerce Microservices Architecture

A comprehensive e-commerce microservices system built with .NET 9, designed to demonstrate modern software architecture patterns and technologies.

## 🎯 Main Goal

The primary objective of this project is to **learn and implement new patterns, architectures, and technologies** in a real-world scenario. This project serves as a learning platform for understanding how different architectural patterns work together in a distributed system.

## 🏗️ Architecture Patterns

### **Clean Architecture**
- **Domain Layer**: Business entities, value objects, and domain events
- **Application Layer**: Use cases, CQRS commands/queries, DTOs, and validation
- **Infrastructure Layer**: Data access, external services, and persistence
- **API Layer**: Controllers, middleware, and HTTP concerns

### **Domain-Driven Design (DDD)**
- **Entities**: Core business objects with identity
- **Value Objects**: Immutable objects representing concepts
- **Domain Events**: Capture business events within the domain
- **Aggregates**: Consistency boundaries for business operations

### **CQRS (Command Query Responsibility Segregation)**
- **Commands**: Write operations that modify state
- **Queries**: Read operations that return data
- **Handlers**: Separate logic for commands and queries
- **MediatR**: Mediator pattern implementation

### **Event-Driven Architecture**
- **Domain Events**: Internal events within a service
- **Integration Events**: Cross-service communication events
- **Event Sourcing**: Audit trail of state changes
- **Asynchronous Processing**: Non-blocking event handling

### **Microservices Architecture**
- **Service Isolation**: Independent deployable services
- **Database per Service**: Each service owns its data
- **API Gateway**: Single entry point for clients
- **Service Discovery**: Dynamic service location

## 🛠️ Technology Stack

### **Backend Framework**
- **.NET 9**: Latest version of .NET framework
- **ASP.NET Core**: Web API framework
- **C#**: Programming language

### **Database & ORM**
- **PostgreSQL**: Primary database for all services
- **Entity Framework Core 9**: Object-relational mapping
- **Npgsql**: PostgreSQL provider for EF Core

### **Message Broker**
- **RabbitMQ**: Message broker for async communication
- **Custom EventBus**: Abstraction layer over RabbitMQ

### **API Gateway**
- **Ocelot**: .NET API Gateway for routing and load balancing

### **Validation & Mediator**
- **FluentValidation**: Fluent validation library
- **MediatR**: Mediator pattern implementation

### **Containerization**
- **Docker**: Container platform
- **Docker Compose**: Multi-container orchestration

### **Development Tools**
- **Swagger/OpenAPI**: API documentation
- **VS Code**: Development environment

## 📦 Services

### **AuthService** (Port: 7002)
- User authentication and authorization
- JWT token management
- Identity management

### **ProductService** (Port: 7001)
- Product catalog management
- Product information and pricing
- Inventory basics

### **OrderService** (Port: 7003)
- Order processing and management
- Order lifecycle management
- Integration with stock service

### **StockService** (Port: 7004)
- Inventory management
- Stock reservation and release
- Real-time stock tracking

### **API Gateway** (Port: 8000)
- Request routing
- Load balancing
- Centralized entry point

## 🔄 Communication Patterns

### **Synchronous Communication**
- **HTTP/REST**: Client-to-service communication via API Gateway
- **Service-to-Service**: Direct HTTP calls when needed

### **Asynchronous Communication**
- **Event-Driven**: Services communicate via events
- **Message Queues**: RabbitMQ for reliable message delivery
- **Eventual Consistency**: Data consistency across services

## 📊 Database Schema

Each service maintains its own database:

- **AuthDb**: User accounts, roles, and permissions
- **ProductDb**: Product catalog and metadata
- **OrderServiceDb**: Orders, order items, and order history
- **StockServiceDb**: Stock levels, reservations, and movements

## 🚀 Getting Started

### Prerequisites
- Docker and Docker Compose
- .NET 9 SDK (for local development)

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ecommerce-microservices-architecture
   ```

2. **Start all services**
   ```bash
   docker-compose up -d
   ```

3. **Verify services are running**
   ```bash
   docker-compose ps
   ```

### Service Endpoints

- **API Gateway**: http://localhost:8000
- **Auth Service**: http://localhost:7002/swagger
- **Product Service**: http://localhost:7001/swagger
- **Order Service**: http://localhost:7003/swagger
- **Stock Service**: http://localhost:7004/swagger
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)

## 🔍 Key Learning Areas

### **Architectural Patterns**
- How Clean Architecture promotes separation of concerns
- CQRS implementation for scalable read/write operations
- Event-driven communication between microservices
- Domain-driven design for complex business logic

### **Technology Integration**
- .NET 9 features and performance improvements
- Entity Framework Core advanced patterns
- RabbitMQ message patterns and reliability
- Docker containerization best practices

### **Microservices Challenges**
- Service boundaries and data consistency
- Distributed transaction management
- Event ordering and idempotency
- Service resilience and fault tolerance

## 📁 Project Structure

```
ecommerce-microservices-architecture/
├── BuildingBlocks/
│   └── EventBus/                    # Shared event bus infrastructure
├── Gateway/
│   └── ApiGateway.Ocelot/          # API Gateway implementation
├── Services/
│   ├── AuthService/                # Authentication service
│   ├── ProductService/             # Product catalog service
│   ├── OrderService/               # Order management service
│   └── StockService/               # Inventory management service
├── docker-compose.yml              # Container orchestration
└── README.md                       # Project documentation
```

## 🎓 Learning Outcomes

By exploring this project, you will gain hands-on experience with:

- **Modern .NET Development**: Latest features and best practices
- **Microservices Patterns**: Service design and communication
- **Event-Driven Systems**: Asynchronous processing and reliability
- **Clean Architecture**: Maintainable and testable code structure
- **Domain-Driven Design**: Business-focused software modeling
- **CQRS Implementation**: Scalable command and query separation
- **Container Orchestration**: Docker and containerization strategies

## 🤝 Contributing

This project is primarily for learning purposes. Feel free to:

- Explore the codebase
- Experiment with different patterns
- Add new features or services
- Improve existing implementations
- Share your learning experiences


This project is created for educational purposes and learning new technologies and patterns.