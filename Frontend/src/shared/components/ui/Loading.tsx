import React from 'react';
import { cn } from '../../utils/cn';

interface LoadingProps {
  size?: 'sm' | 'md' | 'lg' | 'xl';
  variant?: 'spinner' | 'dots' | 'pulse' | 'bars';
  text?: string;
  className?: string;
  fullScreen?: boolean;
  color?: 'blue' | 'gray' | 'green' | 'red';
}

export const Loading: React.FC<LoadingProps> = ({ 
  size = 'md', 
  variant = 'spinner',
  text = 'Loading...',
  className = '',
  fullScreen = false,
  color = 'blue'
}) => {
  const sizeClasses = {
    sm: 'h-4 w-4',
    md: 'h-8 w-8',
    lg: 'h-12 w-12',
    xl: 'h-16 w-16'
  };

  const colorClasses = {
    blue: 'text-blue-600',
    gray: 'text-gray-600',
    green: 'text-green-600',
    red: 'text-red-600',
  };

  const textSizeClasses = {
    sm: 'text-xs',
    md: 'text-sm',
    lg: 'text-base',
    xl: 'text-lg'
  };

  const containerClasses = cn(
    'flex flex-col items-center justify-center',
    fullScreen 
      ? 'fixed inset-0 bg-white/80 backdrop-blur-sm z-50' 
      : 'p-8',
    className
  );

  const renderSpinner = () => (
    <svg
      className={cn('animate-spin', sizeClasses[size], colorClasses[color])}
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
    >
      <circle
        className="opacity-25"
        cx="12"
        cy="12"
        r="10"
        stroke="currentColor"
        strokeWidth="4"
      />
      <path
        className="opacity-75"
        fill="currentColor"
        d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
      />
    </svg>
  );

  const renderDots = () => {
    const dotSize = size === 'sm' ? 'w-2 h-2' : size === 'md' ? 'w-3 h-3' : size === 'lg' ? 'w-4 h-4' : 'w-5 h-5';
    return (
      <div className="flex space-x-1">
        {[0, 1, 2].map((i) => (
          <div
            key={i}
            className={cn(
              dotSize,
              'rounded-full animate-pulse',
              colorClasses[color].replace('text-', 'bg-')
            )}
            style={{
              animationDelay: `${i * 0.2}s`,
              animationDuration: '1s'
            }}
          />
        ))}
      </div>
    );
  };

  const renderPulse = () => (
    <div
      className={cn(
        'rounded-full animate-pulse',
        sizeClasses[size],
        colorClasses[color].replace('text-', 'bg-'),
        'opacity-75'
      )}
    />
  );

  const renderBars = () => {
    const barHeight = size === 'sm' ? 'h-6' : size === 'md' ? 'h-8' : size === 'lg' ? 'h-12' : 'h-16';
    return (
      <div className="flex items-end space-x-1">
        {[0, 1, 2, 3].map((i) => (
          <div
            key={i}
            className={cn(
              'w-1',
              barHeight,
              'animate-pulse',
              colorClasses[color].replace('text-', 'bg-')
            )}
            style={{
              animationDelay: `${i * 0.15}s`,
              animationDuration: '1.2s'
            }}
          />
        ))}
      </div>
    );
  };

  const renderLoader = () => {
    switch (variant) {
      case 'dots':
        return renderDots();
      case 'pulse':
        return renderPulse();
      case 'bars':
        return renderBars();
      default:
        return renderSpinner();
    }
  };

  return (
    <div className={containerClasses}>
      <div className="animate-in fade-in duration-300">
        {renderLoader()}
      </div>
      {text && (
        <p className={cn(
          'mt-3 font-medium animate-in fade-in duration-500 delay-150',
          textSizeClasses[size],
          colorClasses[color]
        )}>
          {text}
        </p>
      )}
    </div>
  );
};