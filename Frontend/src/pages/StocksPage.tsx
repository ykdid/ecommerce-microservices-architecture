import React from 'react';
import { Layout } from '../shared/components/layout';
// Update the import path to the correct location of StockList
import { StockList } from '../features/stocks/components/StockList';

export const StocksPage: React.FC = () => {
  return (
    <Layout>
      <StockList />
    </Layout>
  );
};