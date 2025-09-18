import React, { useState, useEffect } from 'react';
import { Button, Card, Loading } from '../../../shared/components/ui';
import { productService } from '../services';
import type { Product } from '../types';

export const ProductList: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      setLoading(true);
      const data = await productService.getAll();
      setProducts(data);
    } catch (err: any) {
      setError(err.message || 'Failed to load products');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this product?')) return;
    
    try {
      await productService.delete(id);
      setProducts(prev => prev.filter(p => p.id !== id));
    } catch (err: any) {
      alert(err.message || 'Failed to delete product');
    }
  };

  if (loading) return <Loading text="Loading products..." />;

  if (error) {
    return (
      <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
        {error}
        <Button onClick={loadProducts} className="ml-4" size="sm">
          Retry
        </Button>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900">Products</h1>
        <Button onClick={() => window.location.href = '/products/create'}>
          Add Product
        </Button>
      </div>

      {products.length === 0 ? (
        <Card>
          <div className="text-center py-8">
            <p className="text-gray-500">No products found</p>
            <Button 
              onClick={() => window.location.href = '/products/create'}
              className="mt-4"
            >
              Create First Product
            </Button>
          </div>
        </Card>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {products.map(product => (
            <Card key={product.id}>
              <div className="space-y-4">
                <div>
                  <h3 className="text-lg font-semibold text-gray-900">{product.name}</h3>
                  <p className="text-2xl font-bold text-green-600">${product.price}</p>
                  <p className="text-sm text-gray-600">Stock: {product.stock}</p>
                </div>
                
                <div className="flex space-x-2">
                  <Button
                    size="sm"
                    variant="secondary"
                    onClick={() => window.location.href = `/products/${product.id}/edit`}
                  >
                    Edit
                  </Button>
                  <Button
                    size="sm"
                    variant="danger"
                    onClick={() => handleDelete(product.id)}
                  >
                    Delete
                  </Button>
                </div>
              </div>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
};