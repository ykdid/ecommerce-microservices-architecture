import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Layout } from '../shared/components/layout';
import { Card } from '../shared/components/ui/Card';
import { Button } from '../shared/components/ui/Button';
import { authService } from '../features/auth/services';

export const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const isAuthenticated = authService.isAuthenticated();
  const currentUser = authService.getCurrentUser();

  const services = [
    {
      title: 'Products',
      description: 'Browse and manage product catalog',
      path: '/products',
      icon: '📦',
      color: 'from-green-500 to-green-600',
      available: true
    },
    {
      title: 'Orders',
      description: 'View and manage your orders',
      path: '/orders',
      icon: '🛒',
      color: 'from-purple-500 to-purple-600',
      available: isAuthenticated
    },
    {
      title: 'Stock Management',
      description: 'Monitor inventory levels',
      path: '/stocks',
      icon: '📊',
      color: 'from-orange-500 to-orange-600',
      available: isAuthenticated
    }
  ];

  return (
    <Layout>
      <div className="min-h-screen bg-gradient-to-br from-gray-50 to-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
          <div className="text-center mb-12">
            <h1 className="text-4xl md:text-6xl font-bold text-gray-900 mb-6">
              E-Commerce Platform
            </h1>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto mb-8">
              Modern microservices-based e-commerce solution.
            </p>
            
            {!isAuthenticated ? (
              <div className="flex justify-center space-x-4">
                <Button onClick={() => navigate('/login')} size="lg">
                  Sign In
                </Button>
                <Button 
                  onClick={() => navigate('/register')}
                  variant="outline"
                  size="lg"
                >
                  Sign Up
                </Button>
              </div>
            ) : (
              <div className="bg-blue-50 border border-blue-200 rounded-lg p-4 max-w-md mx-auto">
                <p className="text-blue-800">
                  Welcome back, <span className="font-semibold">{currentUser?.fullName}</span>!
                </p>
              </div>
            )}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {services.map((service, index) => (
              <Card key={index} className="hover:shadow-xl transition-all duration-300">
                <div className={`h-2 bg-gradient-to-r ${service.color} rounded-t-lg`}></div>
                <div className="p-6">
                  <div className="flex items-center mb-4">
                    <span className="text-3xl mr-3">{service.icon}</span>
                    <h3 className="text-xl font-semibold text-gray-900">{service.title}</h3>
                  </div>
                  <p className="text-gray-600 mb-6">{service.description}</p>
                  <Button
                    onClick={() => service.available ? navigate(service.path) : navigate('/login')}
                    variant={service.available ? 'primary' : 'secondary'}
                    className="w-full"
                  >
                    {service.available ? 'Access' : 'Login Required'}
                  </Button>
                </div>
              </Card>
            ))}
          </div>
        </div>
      </div>
    </Layout>
  );
};