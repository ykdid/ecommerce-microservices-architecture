import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Layout } from '../shared/components/layout';
import { Card } from '../shared/components/ui/Card';
import { Button } from '../shared/components/ui/Button';
import { authService } from '../features/auth/services';

export const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const isAuthenticated = authService.isAuthenticated();

  const services = [
    {
      title: 'Authentication',
      description: 'Secure user registration and login system with JWT tokens',
      path: '/login',
      icon: '🔐',
      color: 'from-blue-500 to-blue-600',
      available: true
    },
    {
      title: 'Products',
      description: 'Comprehensive product catalog and inventory management',
      path: '/products',
      icon: '📦',
      color: 'from-green-500 to-green-600',
      available: isAuthenticated
    },
    {
      title: 'Orders',
      description: 'Streamlined order processing and customer tracking',
      path: '/orders',
      icon: '🛒',
      color: 'from-purple-500 to-purple-600',
      available: isAuthenticated
    },
    {
      title: 'Stock Management',
      description: 'Real-time stock monitoring and inventory control',
      path: '/stocks',
      icon: '📊',
      color: 'from-orange-500 to-orange-600',
      available: isAuthenticated
    }
  ];

  const architectureFeatures = {
    microservices: [
      { name: 'Auth Service', port: '7001', status: 'active' },
      { name: 'Product Service', port: '7002', status: 'active' },
      { name: 'Order Service', port: '7003', status: 'active' },
      { name: 'Stock Service', port: '7004', status: 'active' }
    ],
    infrastructure: [
      { name: 'API Gateway', port: '8000', status: 'active' },
      { name: 'PostgreSQL DB', port: '5432', status: 'active' },
      { name: 'RabbitMQ', port: '5672', status: 'active' },
      { name: 'Docker', port: '-', status: 'active' }
    ]
  };

  return (
    <Layout variant="centered" background="gradient">
      <div className="space-y-12">
        {/* Hero Section */}
        <div className="text-center space-y-6">
          <div className="space-y-4">
            <h1 className="text-5xl lg:text-6xl font-bold bg-gradient-to-r from-gray-900 via-blue-800 to-purple-800 bg-clip-text text-transparent">
              E-Commerce Microservices
            </h1>
            <p className="text-xl lg:text-2xl text-gray-600 max-w-3xl mx-auto leading-relaxed">
              A modern, scalable microservices architecture for comprehensive e-commerce operations
            </p>
          </div>
          
          {!isAuthenticated && (
            <div className="flex flex-col sm:flex-row gap-4 justify-center items-center">
              <Button
                variant="primary"
                size="lg"
                onClick={() => navigate('/register')}
                className="min-w-32"
              >
                Get Started
              </Button>
              <Button
                variant="outline"
                size="lg"
                onClick={() => navigate('/login')}
                className="min-w-32"
              >
                Sign In
              </Button>
            </div>
          )}
        </div>

        {/* Services Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-6">
          {services.map((service, index) => (
            <Card
              key={index}
              variant="elevated"
              hover={service.available}
              clickable={service.available}
              onClick={() => service.available && navigate(service.path)}
              className={`relative overflow-hidden ${!service.available ? 'opacity-60' : ''}`}
            >
              <div className="space-y-4">
                <div className="flex items-center space-x-3">
                  <div className={`w-12 h-12 bg-gradient-to-br ${service.color} rounded-xl flex items-center justify-center text-2xl`}>
                    {service.icon}
                  </div>
                  <div>
                    <h3 className="font-semibold text-gray-900">{service.title}</h3>
                    {!service.available && (
                      <span className="text-xs text-amber-600 font-medium">Login Required</span>
                    )}
                  </div>
                </div>
                <p className="text-gray-600 text-sm leading-relaxed">
                  {service.description}
                </p>
                {service.available && (
                  <Button
                    variant="ghost"
                    size="sm"
                    className="w-full justify-between group"
                    onClick={(e) => {
                      e.stopPropagation();
                      navigate(service.path);
                    }}
                  >
                    <span>Explore</span>
                    <span className="transform group-hover:translate-x-1 transition-transform">→</span>
                  </Button>
                )}
              </div>
            </Card>
          ))}
        </div>

        {/* Architecture Overview */}
        <Card variant="gradient" size="lg" className="max-w-5xl mx-auto">
          <div className="space-y-8">
            <div className="text-center">
              <h2 className="text-3xl font-bold text-gray-900 mb-2">Architecture Overview</h2>
              <p className="text-gray-600">Distributed microservices with modern infrastructure</p>
            </div>
            
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
              {/* Microservices */}
              <div className="space-y-4">
                <div className="flex items-center space-x-2">
                  <div className="w-8 h-8 bg-gradient-to-br from-blue-500 to-blue-600 rounded-lg flex items-center justify-center">
                    <span className="text-white text-sm">⚡</span>
                  </div>
                  <h4 className="text-lg font-semibold text-gray-900">Microservices</h4>
                </div>
                <div className="space-y-3">
                  {architectureFeatures.microservices.map((service, index) => (
                    <div key={index} className="flex items-center justify-between p-3 bg-white rounded-lg border border-gray-200">
                      <div className="flex items-center space-x-3">
                        <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
                        <span className="font-medium text-gray-900">{service.name}</span>
                      </div>
                      <span className="text-sm text-gray-500 font-mono">:{service.port}</span>
                    </div>
                  ))}
                </div>
              </div>

              {/* Infrastructure */}
              <div className="space-y-4">
                <div className="flex items-center space-x-2">
                  <div className="w-8 h-8 bg-gradient-to-br from-purple-500 to-purple-600 rounded-lg flex items-center justify-center">
                    <span className="text-white text-sm">🏗️</span>
                  </div>
                  <h4 className="text-lg font-semibold text-gray-900">Infrastructure</h4>
                </div>
                <div className="space-y-3">
                  {architectureFeatures.infrastructure.map((component, index) => (
                    <div key={index} className="flex items-center justify-between p-3 bg-white rounded-lg border border-gray-200">
                      <div className="flex items-center space-x-3">
                        <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
                        <span className="font-medium text-gray-900">{component.name}</span>
                      </div>
                      <span className="text-sm text-gray-500 font-mono">
                        {component.port !== '-' ? `:${component.port}` : component.port}
                      </span>
                    </div>
                  ))}
                </div>
              </div>
            </div>

            <div className="bg-blue-50 p-6 rounded-xl border border-blue-200">
              <div className="flex items-start space-x-3">
                <div className="w-6 h-6 bg-blue-500 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5">
                  <span className="text-white text-xs">ℹ️</span>
                </div>
                <div>
                  <h5 className="font-semibold text-blue-900 mb-1">Enterprise Ready</h5>
                  <p className="text-blue-800 text-sm">
                    Built with scalability, security, and maintainability in mind. Each service is independently deployable 
                    and can be scaled based on demand.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </Card>
      </div>
    </Layout>
  );
};