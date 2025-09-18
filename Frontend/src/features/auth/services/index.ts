import { apiCall } from '../../../shared/services/apiClient';
import type { 
  AuthRequest, 
  RegisterRequest, 
  AuthResponse, 
  User 
} from '../types';

export const authService = {
  // Register new user
  register: async (data: RegisterRequest): Promise<AuthResponse> => {
    return apiCall<AuthResponse>({
      method: 'POST',
      url: '/auth/register',
      data,
    });
  },

  // Login user
  login: async (data: AuthRequest): Promise<AuthResponse> => {
    return apiCall<AuthResponse>({
      method: 'POST',
      url: '/auth/login',
      data,
    });
  },

  // Refresh token
  refreshToken: async (): Promise<AuthResponse> => {
    return apiCall<AuthResponse>({
      method: 'POST',
      url: '/auth/refresh',
    });
  },

  // Health check (protected route)
  healthCheck: async (): Promise<{ status: string }> => {
    return apiCall<{ status: string }>({
      method: 'GET',
      url: '/auth/health',
    });
  },

  // Logout (client-side)
  logout: () => {
    localStorage.removeItem('authToken');
    window.location.href = '/login';
  },

  // Get current user info from token
  getCurrentUser: (): User | null => {
    const token = localStorage.getItem('authToken');
    if (!token) return null;
    
    try {
      // Decode JWT token to get user info (simplified)
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        id: payload.sub || payload.userId,
        email: payload.email,
        fullName: payload.fullName || payload.name,
      };
    } catch {
      return null;
    }
  },

  // Check if user is authenticated
  isAuthenticated: (): boolean => {
    const token = localStorage.getItem('authToken');
    if (!token) return false;
    
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp * 1000; // Convert to milliseconds
      return Date.now() < exp;
    } catch {
      return false;
    }
  },
};