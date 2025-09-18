import React, { useState } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { authService } from '../../../features/auth/services';
import { Button } from '../ui/Button';
import { cn } from '../../utils/cn';

export const Navigation: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const user = authService.getCurrentUser();
  const isAuthenticated = authService.isAuthenticated();

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
    setIsMenuOpen(false);
  };

  const isActivePath = (path: string) => location.pathname === path;

  const NavLink: React.FC<{ to: string; children: React.ReactNode; onClick?: () => void }> = ({ 
    to, 
    children, 
    onClick 
  }) => (
    <Link
      to={to}
      onClick={onClick}
      className={cn(
        'relative px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 hover:bg-gray-100',
        isActivePath(to) 
          ? 'text-blue-600 bg-blue-50' 
          : 'text-gray-700 hover:text-gray-900'
      )}
    >
      {children}
      {isActivePath(to) && (
        <div className="absolute bottom-0 left-1/2 transform -translate-x-1/2 w-1 h-1 bg-blue-600 rounded-full" />
      )}
    </Link>
  );

  return (
    <nav className="sticky top-0 z-50 bg-white/95 backdrop-blur-sm shadow-sm border-b border-gray-200">
      <div className="container mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          {/* Logo */}
          <div className="flex items-center">
            <Link 
              to="/" 
              className="flex items-center space-x-2 text-xl font-bold text-gray-900 hover:text-blue-600 transition-colors duration-200"
            >
              <div className="w-8 h-8 bg-gradient-to-br from-blue-500 to-blue-600 rounded-lg flex items-center justify-center">
                <span className="text-white text-sm font-bold">EC</span>
              </div>
              <span className="hidden sm:block">E-Commerce</span>
            </Link>
          </div>

          {/* Desktop Navigation Links */}
          <div className="hidden md:flex items-center space-x-2">
            <NavLink to="/">Home</NavLink>
            {isAuthenticated && (
              <>
                <NavLink to="/products">Products</NavLink>
                <NavLink to="/orders">Orders</NavLink>
                <NavLink to="/stocks">Stocks</NavLink>
              </>
            )}
          </div>

          {/* Desktop User Actions */}
          <div className="hidden md:flex items-center space-x-4">
            {isAuthenticated ? (
              <div className="flex items-center space-x-4">
                <div className="flex items-center space-x-2">
                  <div className="w-8 h-8 bg-gradient-to-br from-gray-400 to-gray-500 rounded-full flex items-center justify-center">
                    <span className="text-white text-xs font-medium">
                      {(user?.fullName || user?.email || 'U').charAt(0).toUpperCase()}
                    </span>
                  </div>
                  <span className="text-sm text-gray-700 max-w-32 truncate">
                    {user?.fullName || user?.email}
                  </span>
                </div>
                <Button
                  variant="danger"
                  size="sm"
                  onClick={handleLogout}
                >
                  Logout
                </Button>
              </div>
            ) : (
              <div className="flex items-center space-x-3">
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={() => navigate('/login')}
                >
                  Login
                </Button>
                <Button
                  variant="primary"
                  size="sm"
                  onClick={() => navigate('/register')}
                >
                  Register
                </Button>
              </div>
            )}
          </div>

          {/* Mobile menu button */}
          <div className="md:hidden">
            <button
              onClick={() => setIsMenuOpen(!isMenuOpen)}
              className="p-2 rounded-lg text-gray-700 hover:bg-gray-100 transition-colors duration-200"
            >
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                {isMenuOpen ? (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                ) : (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
                )}
              </svg>
            </button>
          </div>
        </div>

        {/* Mobile Navigation Menu */}
        {isMenuOpen && (
          <div className="md:hidden py-4 border-t border-gray-200 bg-white/95 backdrop-blur-sm">
            <div className="flex flex-col space-y-2">
              <NavLink to="/" onClick={() => setIsMenuOpen(false)}>Home</NavLink>
              {isAuthenticated && (
                <>
                  <NavLink to="/products" onClick={() => setIsMenuOpen(false)}>Products</NavLink>
                  <NavLink to="/orders" onClick={() => setIsMenuOpen(false)}>Orders</NavLink>
                  <NavLink to="/stocks" onClick={() => setIsMenuOpen(false)}>Stocks</NavLink>
                </>
              )}
              
              <div className="pt-4 border-t border-gray-200 mt-4">
                {isAuthenticated ? (
                  <div className="space-y-3">
                    <div className="flex items-center space-x-2 px-3 py-2">
                      <div className="w-8 h-8 bg-gradient-to-br from-gray-400 to-gray-500 rounded-full flex items-center justify-center">
                        <span className="text-white text-xs font-medium">
                          {(user?.fullName || user?.email || 'U').charAt(0).toUpperCase()}
                        </span>
                      </div>
                      <span className="text-sm text-gray-700 truncate">
                        {user?.fullName || user?.email}
                      </span>
                    </div>
                    <Button
                      variant="danger"
                      size="sm"
                      onClick={handleLogout}
                      className="w-full"
                    >
                      Logout
                    </Button>
                  </div>
                ) : (
                  <div className="space-y-2">
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => {
                        navigate('/login');
                        setIsMenuOpen(false);
                      }}
                      className="w-full"
                    >
                      Login
                    </Button>
                    <Button
                      variant="primary"
                      size="sm"
                      onClick={() => {
                        navigate('/register');
                        setIsMenuOpen(false);
                      }}
                      className="w-full"
                    >
                      Register
                    </Button>
                  </div>
                )}
              </div>
            </div>
          </div>
        )}
      </div>
    </nav>
  );
};