import React from 'react';
import { cn } from '../../utils/cn';

interface CardProps {
  children: React.ReactNode;
  className?: string;
  title?: string;
  subtitle?: string;
  variant?: 'default' | 'elevated' | 'outlined' | 'gradient';
  size?: 'sm' | 'md' | 'lg';
  hover?: boolean;
  clickable?: boolean;
  onClick?: () => void;
}

export const Card: React.FC<CardProps> = ({
  children,
  className = '',
  title,
  subtitle,
  variant = 'default',
  size = 'md',
  hover = false,
  clickable = false,
  onClick,
}) => {
  const baseClasses = 'bg-white rounded-xl overflow-hidden transition-all duration-300';
  
  const variantClasses = {
    default: 'shadow-sm border border-gray-200',
    elevated: 'shadow-lg hover:shadow-xl',
    outlined: 'border-2 border-gray-200 shadow-none',
    gradient: 'bg-gradient-to-br from-white to-gray-50 shadow-md border border-gray-100',
  };
  
  const sizeClasses = {
    sm: 'text-sm',
    md: 'text-base',
    lg: 'text-lg',
  };

  const headerPadding = {
    sm: 'px-4 py-3',
    md: 'px-6 py-4',
    lg: 'px-8 py-5',
  };

  const contentPadding = {
    sm: 'p-4',
    md: 'p-6',
    lg: 'p-8',
  };

  const hoverClasses = hover || clickable ? 'hover:shadow-lg hover:-translate-y-1' : '';
  const clickableClasses = clickable ? 'cursor-pointer' : '';

  const cardClasses = cn(
    baseClasses,
    variantClasses[variant],
    sizeClasses[size],
    hoverClasses,
    clickableClasses,
    className
  );

  const handleClick = () => {
    if (clickable && onClick) {
      onClick();
    }
  };

  return (
    <div className={cardClasses} onClick={handleClick}>
      {(title || subtitle) && (
        <div className={cn(
          'border-b border-gray-200 bg-gray-50/50',
          headerPadding[size]
        )}>
          {title && (
            <h3 className={cn(
              'font-semibold text-gray-900',
              size === 'sm' ? 'text-base' : size === 'md' ? 'text-lg' : 'text-xl'
            )}>
              {title}
            </h3>
          )}
          {subtitle && (
            <p className={cn(
              'text-gray-600',
              size === 'sm' ? 'text-xs mt-0.5' : 'text-sm mt-1'
            )}>
              {subtitle}
            </p>
          )}
        </div>
      )}
      <div className={contentPadding[size]}>
        {children}
      </div>
    </div>
  );
};