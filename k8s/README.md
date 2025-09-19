# Ecommerce Microservices - Kubernetes Deployment

This directory contains Kubernetes manifests for deploying the ecommerce microservices architecture on a local Minikube cluster.

## 📋 Architecture Overview

The application consists of the following components:

### 🗄️ Databases
- **PostgreSQL** instances for each service:
  - `product-db` - Product service database
  - `auth-db` - Authentication service database
  - `order-db` - Order service database
  - `stock-db` - Stock service database

### 🐰 Message Broker
- **RabbitMQ** - Message queue for inter-service communication
  - Management UI available for monitoring

### 🌐 Microservices
- **Product API** - Product catalog management
- **Auth API** - User authentication and authorization
- **Order API** - Order processing (with RabbitMQ integration)
- **Stock API** - Inventory management (with RabbitMQ integration)
- **Gateway** - Ocelot API Gateway for routing

## 📁 File Structure

```
k8s/
├── namespace.yaml          # Kubernetes namespace
├── secrets.yaml           # Database and RabbitMQ credentials
├── configmaps.yaml        # Application configuration
├── product-db.yaml        # Product database deployment
├── auth-db.yaml          # Auth database deployment
├── order-db.yaml         # Order database deployment
├── stock-db.yaml         # Stock database deployment
├── rabbitmq.yaml         # RabbitMQ deployment
├── product-api.yaml      # Product service deployment
├── auth-api.yaml         # Auth service deployment
├── order-api.yaml        # Order service deployment
├── stock-api.yaml        # Stock service deployment
├── gateway.yaml          # API Gateway deployment
└── ingress.yaml          # Ingress configuration
```

## 🚀 Prerequisites

1. **Minikube** installed and running
2. **kubectl** configured to communicate with Minikube
3. **Docker** images built for the services

## 🛠️ Setup Instructions

### 1. Start Minikube

```bash
minikube start
```

### 2. Enable Required Addons

```bash
# Enable ingress addon
minikube addons enable ingress
```

### 3. Build Docker Images

Set Docker environment to use Minikube's Docker daemon:

```bash
eval $(minikube docker-env)
```

Build the application images:

```bash
# From the project root directory
docker build -t product-api:latest -f Services/ProductService/Product.API/Dockerfile .
docker build -t auth-api:latest -f Services/AuthService/Auth.API/Dockerfile .
docker build -t order-api:latest -f Services/OrderService/Order.API/Dockerfile .
docker build -t stock-api:latest -f Services/StockService/Stock.API/Dockerfile .
docker build -t gateway:latest -f Gateway/ApiGateway.Ocelot/Dockerfile Gateway/ApiGateway.Ocelot
```

### 4. Deploy to Kubernetes

Navigate to the k8s directory and apply the manifests in order:

```bash
cd k8s

# Create namespace
kubectl apply -f namespace.yaml

# Create secrets and configuration
kubectl apply -f secrets.yaml
kubectl apply -f configmaps.yaml

# Deploy databases
kubectl apply -f product-db.yaml
kubectl apply -f auth-db.yaml
kubectl apply -f order-db.yaml
kubectl apply -f stock-db.yaml

# Deploy RabbitMQ
kubectl apply -f rabbitmq.yaml

# Wait for databases and RabbitMQ to be ready
kubectl wait --for=condition=available --timeout=300s deployment/product-db -n ecommerce
kubectl wait --for=condition=available --timeout=300s deployment/auth-db -n ecommerce
kubectl wait --for=condition=available --timeout=300s deployment/order-db -n ecommerce
kubectl wait --for=condition=available --timeout=300s deployment/stock-db -n ecommerce
kubectl wait --for=condition=available --timeout=300s deployment/rabbitmq -n ecommerce

# Deploy API services
kubectl apply -f product-api.yaml
kubectl apply -f auth-api.yaml
kubectl apply -f order-api.yaml
kubectl apply -f stock-api.yaml

# Deploy Gateway
kubectl apply -f gateway.yaml

# Create Ingress
kubectl apply -f ingress.yaml
```

## 🔗 Access Information

### Service Endpoints

- **API Gateway**: `http://$(minikube ip):30000`
- **RabbitMQ Management**: `http://$(minikube ip):30672`

### Ingress Access

To use the ingress, add the following to your `/etc/hosts` file:

```
$(minikube ip) ecommerce.local
```

Then access:
- **Application**: `http://ecommerce.local`
- **RabbitMQ**: `http://ecommerce.local/rabbitmq`

### Default Credentials

- **PostgreSQL**: `postgres/postgres`
- **RabbitMQ**: `guest/guest`

## 🔍 Monitoring and Debugging

### Useful Commands

```bash
# Check pod status
kubectl get pods -n ecommerce

# Check services
kubectl get services -n ecommerce

# Check deployments
kubectl get deployments -n ecommerce

# View logs
kubectl logs -f deployment/gateway -n ecommerce
kubectl logs -f deployment/product-api -n ecommerce

# Describe a pod for detailed information
kubectl describe pod <pod-name> -n ecommerce

# Access Minikube dashboard
minikube dashboard
```

### Port Forwarding for Direct Access

```bash
# Forward gateway port
kubectl port-forward svc/gateway 8080:8080 -n ecommerce

# Forward RabbitMQ management port
kubectl port-forward svc/rabbitmq 15672:15672 -n ecommerce

# Forward database ports (if needed)
kubectl port-forward svc/product-db 5432:5432 -n ecommerce
```

## 🗑️ Cleanup

To remove all deployed resources:

```bash
# Delete in reverse order
kubectl delete -f ingress.yaml
kubectl delete -f gateway.yaml
kubectl delete -f stock-api.yaml
kubectl delete -f order-api.yaml
kubectl delete -f auth-api.yaml
kubectl delete -f product-api.yaml
kubectl delete -f rabbitmq.yaml
kubectl delete -f stock-db.yaml
kubectl delete -f order-db.yaml
kubectl delete -f auth-db.yaml
kubectl delete -f product-db.yaml
kubectl delete -f configmaps.yaml
kubectl delete -f secrets.yaml
kubectl delete -f namespace.yaml
```

## 🔧 Troubleshooting

### Common Issues

1. **Pods in Pending State**
   - Check if Minikube has enough resources
   - Verify persistent volume claims are bound

2. **ImagePullBackOff Errors**
   - Ensure Docker images are built in Minikube's Docker environment
   - Check image names and tags match the deployment manifests

3. **Service Connection Issues**
   - Verify services are running and healthy
   - Check service discovery (DNS resolution within cluster)

4. **Database Connection Failures**
   - Ensure databases are fully initialized before API services start
   - Check connection strings and credentials

### Resource Requirements

Minimum recommended resources for Minikube:
- **CPU**: 4 cores
- **Memory**: 8GB
- **Disk**: 20GB

Start Minikube with adequate resources:

```bash
minikube start --cpus=4 --memory=8192 --disk-size=20g
```