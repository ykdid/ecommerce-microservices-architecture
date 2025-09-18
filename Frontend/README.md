# E-Commerce Microservices Frontend

A modern React + TypeScript + Vite frontend application for the e-commerce microservices architecture.

## 🚀 Features

- **Feature-based Architecture**: Organized by business domains (auth, products, orders, stocks)
- **TypeScript**: Full type safety and developer experience
- **Tailwind CSS**: Modern utility-first styling
- **React Router**: Client-side routing and navigation
- **Responsive Design**: Mobile-first responsive UI
- **API Integration**: Ready-to-use services for all microservices

## 📁 Project Structure

```
src/
├── features/                 # Feature-based modules
│   ├── auth/                # Authentication feature
│   ├── products/            # Product management
│   ├── orders/              # Order processing
│   └── stocks/              # Stock management
├── shared/                  # Shared/reusable code
│   ├── components/          # UI components
│   ├── services/            # API client
│   ├── types/               # Global types
│   └── utils/               # Utilities
├── pages/                   # Route components
└── App.tsx                  # Main application
```

## 🛠️ Tech Stack

- **React 18** - UI library
- **TypeScript** - Type safety
- **Vite** - Build tool and dev server
- **React Router** - Client-side routing
- **Tailwind CSS** - Styling
- **Axios** - HTTP client

## 🚀 Getting Started

### Prerequisites

- Node.js 18+ 
- npm or yarn
- Running microservices (see main project README)

### Installation

1. Install dependencies:
   ```bash
   npm install
   ```

2. Start the development server:
   ```bash
   npm run dev
   ```

3. Open http://localhost:3000 in your browser

### Environment Variables

Create a `.env` file in the root directory:

```env
VITE_API_URL=http://localhost:8000
VITE_DEV_MODE=true
```

## 🔗 API Integration

The frontend connects to microservices through the API Gateway:

| Service | Gateway Endpoint | Direct Port |
|---------|------------------|-------------|
| API Gateway | http://localhost:8000 | 8000 |
| Auth Service | /auth/* | 7001 |
| Product Service | /products/* | 7002 |
| Order Service | /orders/* | 7003 |
| Stock Service | /stocks/* | 7004 |

## 📋 Available Features

### Authentication
- ✅ User registration
- ✅ User login
- ✅ JWT token management
- ✅ Protected routes
- ✅ Auto logout on token expiry

### Products
- ✅ Product listing
- ✅ Product creation
- ✅ Product editing
- ✅ Product deletion
- ✅ Real-time validation

### Orders (Ready for implementation)
- 🚧 Order creation
- 🚧 Order history
- 🚧 Order tracking

### Stock Management (Ready for implementation)
- 🚧 Stock monitoring
- 🚧 Stock updates
- 🚧 Stock reservations

## 🧩 Component Usage

### Basic Components

```tsx
import { Button, Input, Card } from '@/shared/components/ui';

// Button variations
<Button variant="primary" size="md" loading={false}>
  Click me
</Button>

// Input with validation
<Input
  label="Email"
  type="email"
  error="Invalid email"
  placeholder="Enter email"
/>

// Card container
<Card title="Product Details">
  <p>Content here</p>
</Card>
```

### Feature Components

```tsx
import { LoginForm } from '@/features/auth/components';
import { ProductList } from '@/features/products/components';

// Use in pages
<LoginForm />
<ProductList />
```

## 🔧 Development

### Available Scripts

```bash
# Development
npm run dev          # Start dev server
npm run build        # Build for production
npm run preview      # Preview production build

# Code Quality
npm run lint         # Run ESLint
npm run type-check   # TypeScript checking
```

### Adding New Features

1. Create feature directory: `src/features/[feature-name]/`
2. Add components, services, types, and hooks
3. Export through `index.ts`
4. Create page component in `src/pages/`
5. Add route to `App.tsx`

### API Service Pattern

```tsx
// features/[feature]/services/index.ts
import { apiCall } from '@/shared/services/apiClient';

export const featureService = {
  getAll: () => apiCall({ method: 'GET', url: '/endpoint' }),
  create: (data) => apiCall({ method: 'POST', url: '/endpoint', data }),
  // ... more methods
};
```

## 🎨 Styling Guidelines

Using Tailwind CSS utilities:

```tsx
// Layout
<div className="container mx-auto px-4 py-8">

// Responsive
<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">

// Interactive
<button className="bg-blue-600 hover:bg-blue-700 transition-colors">
```

## 🔒 Authentication Flow

1. User logs in → JWT token stored in localStorage
2. API client automatically adds token to requests
3. Protected routes check authentication status
4. Auto-redirect to login on 401 responses
5. Token validation on app startup

## 📱 Responsive Design

- **Mobile-first**: Designed for mobile, enhanced for desktop
- **Breakpoints**: sm (640px), md (768px), lg (1024px), xl (1280px)
- **Navigation**: Responsive header with mobile menu
- **Forms**: Touch-friendly inputs and buttons

## 🚀 Deployment

### Build for Production

```bash
npm run build
```

### Docker Deployment

```dockerfile
FROM node:18-alpine
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
RUN npm run build
EXPOSE 3000
CMD ["npm", "run", "preview"]
```

---

This frontend provides a clean, scalable foundation for the e-commerce microservices application with room for future enhancements and features.
