import React, { useState, useEffect } from 'react';
import { Button, Input, Card } from '../../../shared/components/ui';
import { productService } from '../services';
import type { CreateProductRequest } from '../types';

interface ProductFormProps {
  productId?: string;
  onSuccess?: () => void;
}

export const ProductForm: React.FC<ProductFormProps> = ({ productId, onSuccess }) => {
  const [formData, setFormData] = useState<CreateProductRequest>({
    name: '',
    price: 0,
    stock: 0,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const isEditing = !!productId;

  useEffect(() => {
    if (productId) {
      loadProduct(productId);
    }
  }, [productId]);

  const loadProduct = async (id: string) => {
    try {
      const product = await productService.getById(id);
      setFormData({
        name: product.name,
        price: product.price,
        stock: product.stock,
      });
    } catch (err: any) {
      setError(err.message || 'Failed to load product');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      if (isEditing) {
        await productService.update(productId!, { id: productId!, ...formData });
      } else {
        await productService.create(formData);
      }
      
      if (onSuccess) {
        onSuccess();
      } else {
        window.location.href = '/products';
      }
    } catch (err: any) {
      setError(err.message || `Failed to ${isEditing ? 'update' : 'create'} product`);
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'number' ? parseFloat(value) || 0 : value,
    }));
  };

  return (
    <div className="max-w-md mx-auto">
      <Card title={isEditing ? 'Edit Product' : 'Create Product'}>
        <form onSubmit={handleSubmit} className="space-y-6">
          {error && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
              {error}
            </div>
          )}
          
          <Input
            label="Product Name"
            type="text"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
            placeholder="Enter product name"
          />
          
          <Input
            label="Price"
            type="number"
            name="price"
            value={formData.price}
            onChange={handleChange}
            required
            min="0"
            step="0.01"
            placeholder="Enter price"
          />
          
          <Input
            label="Stock"
            type="number"
            name="stock"
            value={formData.stock}
            onChange={handleChange}
            required
            min="0"
            placeholder="Enter stock quantity"
          />
          
          <div className="flex space-x-4">
            <Button
              type="submit"
              loading={loading}
              className="flex-1"
            >
              {isEditing ? 'Update' : 'Create'} Product
            </Button>
            <Button
              type="button"
              variant="secondary"
              onClick={() => window.location.href = '/products'}
              className="flex-1"
            >
              Cancel
            </Button>
          </div>
        </form>
      </Card>
    </div>
  );
};