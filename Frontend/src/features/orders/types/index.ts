// Order Types
export interface Address {
  street: string;
  city: string;
  state: string;
  country: string;
  zipCode: string;
}

export interface OrderItem {
  productId: string;
  productName: string;
  unitPrice: number;
  currency: string;
  quantity: number;
}

export interface Order {
  id: string;
  userId: string;
  shippingAddress: Address;
  billingAddress: Address;
  orderItems: OrderItem[];
  totalAmount: number;
  status: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateOrderRequest {
  userId: string;
  shippingAddress: Address;
  billingAddress: Address;
  orderItems: OrderItem[];
}

export interface OrderListResponse {
  orders: Order[];
  total: number;
}