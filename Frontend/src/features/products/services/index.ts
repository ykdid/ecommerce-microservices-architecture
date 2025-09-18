import { apiCall } from '../../../shared/services/apiClient';
import type { 
  Product, 
  CreateProductRequest, 
  UpdateProductRequest 
} from '../types';

export const productService = {
  // Get all products
  getAll: async (): Promise<Product[]> => {
    return apiCall<Product[]>({
      method: 'GET',
      url: '/products',
    });
  },

  // Get product by ID
  getById: async (id: string): Promise<Product> => {
    return apiCall<Product>({
      method: 'GET',
      url: `/products/${id}`,
    });
  },

  // Create new product
  create: async (data: CreateProductRequest): Promise<{ Id: string }> => {
    return apiCall<{ Id: string }>({
      method: 'POST',
      url: '/products',
      data,
    });
  },

  // Update product
  update: async (id: string, data: UpdateProductRequest): Promise<void> => {
    return apiCall<void>({
      method: 'PUT',
      url: `/products/${id}`,
      data: { ...data, id },
    });
  },

  // Delete product
  delete: async (id: string): Promise<void> => {
    return apiCall<void>({
      method: 'DELETE',
      url: `/products/${id}`,
    });
  },
};