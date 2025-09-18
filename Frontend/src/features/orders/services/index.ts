import { apiCall } from '../../../shared/services/apiClient';
import type { 
  Order, 
  CreateOrderRequest 
} from '../types';

export const orderService = {
  // Create new order
  create: async (data: CreateOrderRequest): Promise<string> => {
    return apiCall<string>({
      method: 'POST',
      url: '/orders',
      data,
    });
  },

  // Get orders by user ID
  getByUserId: async (userId: string): Promise<Order[]> => {
    return apiCall<Order[]>({
      method: 'GET',
      url: `/orders/user/${userId}`,
    });
  },

  // Get order by ID
  getById: async (id: string): Promise<Order> => {
    return apiCall<Order>({
      method: 'GET',
      url: `/orders/${id}`,
    });
  },
};