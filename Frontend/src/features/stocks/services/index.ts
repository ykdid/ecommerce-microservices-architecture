import { apiCall } from '../../../shared/services/apiClient';
import type { 
  Stock, 
  UpdateStockRequest, 
  ReserveStockRequest, 
  ReleaseStockRequest 
} from '../types';

export const stockService = {
  // Get all stocks
  getAll: async (): Promise<Stock[]> => {
    return apiCall<Stock[]>({
      method: 'GET',
      url: '/stocks',
    });
  },

  // Get stock by product ID
  getByProductId: async (productId: string): Promise<Stock> => {
    return apiCall<Stock>({
      method: 'GET',
      url: `/stocks/product/${productId}`,
    });
  },

  // Update stock quantity
  updateQuantity: async (data: UpdateStockRequest): Promise<Stock> => {
    return apiCall<Stock>({
      method: 'PUT',
      url: '/stocks/update-quantity',
      data,
    });
  },

  // Reserve stock
  reserve: async (data: ReserveStockRequest): Promise<Stock> => {
    return apiCall<Stock>({
      method: 'POST',
      url: '/stocks/reserve',
      data,
    });
  },

  // Release stock
  release: async (data: ReleaseStockRequest): Promise<Stock> => {
    return apiCall<Stock>({
      method: 'POST',
      url: '/stocks/release',
      data,
    });
  },
};