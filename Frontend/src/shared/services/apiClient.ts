import axios from 'axios';
import type { AxiosInstance, AxiosRequestConfig } from 'axios';

// Ocelot Gateway Configuration
const GATEWAY_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8000';

// Create axios instance for Ocelot Gateway
export const apiClient: AxiosInstance = axios.create({
  baseURL: GATEWAY_BASE_URL,
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for error handling
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Handle unauthorized - clear token and redirect to login
      localStorage.removeItem('authToken');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    
    // Log errors for debugging in development
    if (import.meta.env.VITE_DEV_MODE === 'true') {
      console.error('API Error:', error.response?.data || error.message);
    }
    
    return Promise.reject(error);
  }
);

// Helper function for API calls
export const apiCall = async <T>(config: AxiosRequestConfig): Promise<T> => {
  try {
    const response = await apiClient(config);
    return response.data;
  } catch (error: any) {
    throw {
      message: error.response?.data?.message || error.message || 'An error occurred',
      status: error.response?.status || 500,
      details: error.response?.data,
    };
  }
};