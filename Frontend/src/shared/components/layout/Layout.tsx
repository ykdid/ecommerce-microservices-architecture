import React from 'react';
import { Navigation } from './Navigation';
import { cn } from '../../utils/cn';

interface LayoutProps {
  children: React.ReactNode;
  className?: string;
  variant?: 'default' | 'centered' | 'wide' | 'narrow';
  background?: 'default' | 'gradient' | 'pattern';
}

export const Layout: React.FC<LayoutProps> = ({ 
  children, 
  className = '',
  variant = 'default',
  background = 'default'
}) => {
  const backgroundClasses = {
    default: 'bg-gray-50',
    gradient: 'bg-gradient-to-br from-gray-50 via-blue-50/30 to-indigo-50/50',
    pattern: 'bg-gray-50 bg-[radial-gradient(ellipse_at_top,_var(--tw-gradient-stops))] from-blue-50/20 via-gray-50 to-gray-50',
  };

  const containerClasses = {
    default: 'container mx-auto px-4 sm:px-6 lg:px-8',
    centered: 'max-w-4xl mx-auto px-4 sm:px-6 lg:px-8',
    wide: 'max-w-7xl mx-auto px-4 sm:px-6 lg:px-8',
    narrow: 'max-w-2xl mx-auto px-4 sm:px-6 lg:px-8',
  };

  return (
    <div className={cn(
      'min-h-screen transition-all duration-300',
      backgroundClasses[background],
      className
    )}>
      <Navigation />
      <main className={cn(
        'py-6 sm:py-8 lg:py-12',
        containerClasses[variant]
      )}>
        <div className="animate-in fade-in duration-500">
          {children}
        </div>
      </main>
      
      {/* Subtle background decorations */}
      {background === 'pattern' && (
        <>
          <div className="fixed top-0 left-0 w-72 h-72 bg-gradient-to-br from-blue-400/10 to-purple-400/10 rounded-full blur-3xl -z-10" />
          <div className="fixed bottom-0 right-0 w-96 h-96 bg-gradient-to-br from-indigo-400/10 to-pink-400/10 rounded-full blur-3xl -z-10" />
        </>
      )}
    </div>
  );
};