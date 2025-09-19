import React, { useState, useEffect } from 'react';
import { Button, Card, Loading } from '../../../shared/components/ui';
import { stockService } from '../services';
import type { Stock } from '../types';

export const StockList: React.FC = () => {
  const [stocks, setStocks] = useState<Stock[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadStocks();
  }, []);

  const loadStocks = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await stockService.getAll();
      setStocks(data);
    } catch (err: any) {
      setError(err.message || 'Failed to load stocks');
    } finally {
      setLoading(false);
    }
  };

  const getStockStatus = (stock: Stock) => {
    const availablePercentage = (stock.availableQuantity / stock.quantity) * 100;
    
    if (availablePercentage <= 10) {
      return { color: 'bg-red-100 text-red-800', status: 'Critical' };
    } else if (availablePercentage <= 25) {
      return { color: 'bg-yellow-100 text-yellow-800', status: 'Low' };
    } else if (availablePercentage <= 50) {
      return { color: 'bg-orange-100 text-orange-800', status: 'Medium' };
    } else {
      return { color: 'bg-green-100 text-green-800', status: 'Good' };
    }
  };

  if (loading) return <Loading text="Loading stock information..." />;

  if (error) {
    return (
      <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
        {error}
        <Button onClick={loadStocks} className="ml-4" size="sm">
          Retry
        </Button>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900">Stock Management</h1>
        <Button onClick={loadStocks} variant="outline">
          Refresh
        </Button>
      </div>

      {stocks.length === 0 ? (
        <Card>
          <div className="text-center py-8">
            <p className="text-gray-500">No stock information found</p>
          </div>
        </Card>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {stocks.map(stock => {
            const stockStatus = getStockStatus(stock);
            return (
              <Card key={stock.id} className="hover:shadow-lg transition-shadow">
                <div className="space-y-4">
                  <div className="flex justify-between items-start">
                    <div>
                      <h3 className="text-lg font-semibold text-gray-900">
                        Product ID: {stock.productId}
                      </h3>
                      <p className="text-sm text-gray-600">
                        Stock ID: {stock.id}
                      </p>
                    </div>
                    <span className={`px-3 py-1 rounded-full text-sm font-medium ${stockStatus.color}`}>
                      {stockStatus.status}
                    </span>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="text-center">
                      <p className="text-2xl font-bold text-blue-600">{stock.quantity}</p>
                      <p className="text-xs text-gray-500">Total Stock</p>
                    </div>
                    <div className="text-center">
                      <p className="text-2xl font-bold text-green-600">{stock.availableQuantity}</p>
                      <p className="text-xs text-gray-500">Available</p>
                    </div>
                  </div>

                  <div className="text-center">
                    <p className="text-xl font-bold text-orange-600">{stock.reservedQuantity}</p>
                    <p className="text-xs text-gray-500">Reserved</p>
                  </div>

                  <div className="w-full bg-gray-200 rounded-full h-3">
                    <div
                      className="bg-blue-600 h-3 rounded-full transition-all duration-300"
                      style={{
                        width: `${(stock.availableQuantity / stock.quantity) * 100}%`
                      }}
                    ></div>
                  </div>

                  <div className="text-center">
                    <p className="text-xs text-gray-400">
                      Last updated: {new Date(stock.lastUpdated).toLocaleDateString()}
                    </p>
                  </div>

                  <div className="flex space-x-2">
                    <Button
                      size="sm"
                      variant="secondary"
                      className="flex-1"
                      onClick={() => alert('Update stock functionality would be implemented here')}
                    >
                      Update Stock
                    </Button>
                  </div>
                </div>
              </Card>
            );
          })}
        </div>
      )}
    </div>
  );
};