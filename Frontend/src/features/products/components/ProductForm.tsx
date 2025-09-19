import React, { useState, useEffect } from 'react';
import { Button, Input, Card } from '../../../shared/components/ui';
import { productService } from '../services';
import type { CreateProductRequest, Product } from '../types';

interface ProductFormProps {
  product?: Product | null;
  onSuccess?: () => void;
  onClose?: () => void;
}

export const ProductForm: React.FC<ProductFormProps> = ({ product, onSuccess, onClose }) => {
  const [formData, setFormData] = useState<CreateProductRequest>({
    name: '',
    price: 0,
    stock: 0,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const isEditing = !!product;

  useEffect(() => {
    if (product) {
      setFormData({
        name: product.name,
        price: product.price,
        stock: product.stock,
      });
    }
  }, [product]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      if (isEditing && product) {
        await productService.update(product.id, {
          id: product.id,
          ...formData
        });
      } else {
        await productService.create(formData);
      }
      onSuccess?.();
    } catch (err: any) {
      setError(err.message || 'Failed to save product');
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
      <Card>
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-semibold">
            {isEditing ? 'Edit Product' : 'Create Product'}
          </h2>
          {onClose && (
            <Button variant="ghost" size="sm" onClick={onClose}>
              ✕
            </Button>
          )}
        </div>

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
              onClick={onClose}
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