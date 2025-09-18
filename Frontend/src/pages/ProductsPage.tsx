import React from 'react';
import { Layout } from '../shared/components/layout';
import { ProductList } from '../features/products/components';

export const ProductsPage: React.FC = () => {
  return (
    <Layout>
      <ProductList />
    </Layout>
  );
};