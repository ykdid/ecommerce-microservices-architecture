// Auth Types
export interface User {
  id: string;
  fullName: string;
  email: string;
}

export interface AuthRequest {
  email: string;
  password: string;
}

export interface RegisterRequest extends AuthRequest {
  fullName: string;
}

export interface AuthResponse {
  token: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}