import React from 'react';
import { Layout } from '../shared/components/layout';
import { OrderList } from '../features/orders/components';

export const OrdersPage: React.FC = () => {
  return (
    <Layout>
      <OrderList />
    </Layout>
  );
};