// Stock Types
export interface Stock {
  id: string;
  productId: string;
  quantity: number;
  reservedQuantity: number;
  availableQuantity: number;
  lastUpdated: string;
}

export interface UpdateStockRequest {
  productId: string;
  quantity: number;
}

export interface ReserveStockRequest {
  productId: string;
  quantity: number;
}

export interface ReleaseStockRequest {
  productId: string;
  quantity: number;
}

export interface StockListResponse {
  stocks: Stock[];
  total: number;
}