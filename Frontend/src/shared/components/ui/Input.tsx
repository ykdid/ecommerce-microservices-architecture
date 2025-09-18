import React, { forwardRef } from 'react';
import { cn } from '../../utils/cn';

interface InputProps extends Omit<React.InputHTMLAttributes<HTMLInputElement>, 'size'> {
  label?: string;
  error?: string;
  helperText?: string;
  variant?: 'default' | 'filled' | 'underlined';
  size?: 'sm' | 'md' | 'lg';
  icon?: React.ReactNode;
  iconPosition?: 'left' | 'right';
}

export const Input = forwardRef<HTMLInputElement, InputProps>(({
  label,
  error,
  helperText,
  variant = 'default',
  size = 'md',
  className = '',
  icon,
  iconPosition = 'left',
  id,
  ...props
}, ref) => {
  const inputId = id || `input-${Math.random().toString(36).substr(2, 9)}`;
  
  const baseClasses = 'w-full transition-all duration-200 focus:outline-none disabled:opacity-50 disabled:cursor-not-allowed placeholder-gray-400';
  
  const variantClasses = {
    default: 'border border-gray-300 rounded-lg bg-white shadow-sm focus:ring-4 focus:ring-blue-500/20 focus:border-blue-500 hover:border-gray-400',
    filled: 'border-0 rounded-lg bg-gray-100 shadow-sm focus:ring-4 focus:ring-blue-500/20 focus:bg-white hover:bg-gray-50',
    underlined: 'border-0 border-b-2 border-gray-300 rounded-none bg-transparent focus:border-blue-500 focus:ring-0 hover:border-gray-400 px-0',
  };
  
  const sizeClasses = {
    sm: 'px-3 py-2 text-sm',
    md: 'px-4 py-2.5 text-sm',
    lg: 'px-6 py-3 text-base',
  };

  const inputClasses = cn(
    baseClasses,
    variantClasses[variant],
    sizeClasses[size],
    icon && iconPosition === 'left' && 'pl-10',
    icon && iconPosition === 'right' && 'pr-10',
    error && 'border-red-500 focus:border-red-500 focus:ring-red-500/20',
    className
  );

  const containerClasses = cn(
    'w-full',
    size === 'sm' ? 'mb-3' : 'mb-4'
  );

  return (
    <div className={containerClasses}>
      {label && (
        <label 
          htmlFor={inputId} 
          className={cn(
            'block font-medium text-gray-700 transition-colors duration-200',
            size === 'sm' ? 'text-xs mb-1' : 'text-sm mb-2',
            error && 'text-red-600'
          )}
        >
          {label}
        </label>
      )}
      <div className="relative">
        {icon && iconPosition === 'left' && (
          <div className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 pointer-events-none">
            {icon}
          </div>
        )}
        <input
          ref={ref}
          id={inputId}
          className={inputClasses}
          {...props}
        />
        {icon && iconPosition === 'right' && (
          <div className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 pointer-events-none">
            {icon}
          </div>
        )}
      </div>
      {error && (
        <p className={cn(
          'text-red-600 transition-all duration-200',
          size === 'sm' ? 'text-xs mt-1' : 'text-sm mt-1'
        )}>
          {error}
        </p>
      )}
      {helperText && !error && (
        <p className={cn(
          'text-gray-500 transition-all duration-200',
          size === 'sm' ? 'text-xs mt-1' : 'text-sm mt-1'
        )}>
          {helperText}
        </p>
      )}
    </div>
  );
});

Input.displayName = 'Input';