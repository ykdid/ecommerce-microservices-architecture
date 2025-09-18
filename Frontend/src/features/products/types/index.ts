// Product Types
export interface Product {
  id: string;
  name: string;
  price: number;
  stock: number;
  createdAt?: string;
  updatedAt?: string;
}

export interface CreateProductRequest {
  name: string;
  price: number;
  stock: number;
}

export interface UpdateProductRequest {
  id: string;
  name: string;
  price: number;
  stock: number;
}

export interface ProductListResponse {
  products: Product[];
  total: number;
}