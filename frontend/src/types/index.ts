// User types
export interface User {
  id: string
  username: string
  email: string
}

export interface AuthResponse {
  token: string
  user: User
}

export interface LoginCredentials {
  email: string
  password: string
}

export interface RegisterCredentials {
  username: string
  email: string
  password: string
}

// Project types
export interface Project {
  id: string
  title: string
  description?: string
  createdAt: string
}

export interface ProjectFormData {
  title: string
  description?: string
}

// Task types
export interface Task {
  id: string
  title: string
  completed: boolean
  dueDate?: string
  estimatedHours?: number
  dependsOn?: string[]
  projectId: string
}

export interface TaskFormData {
  title: string
  dueDate?: string
  estimatedHours?: number
  dependsOn?: string[]
}

export type FilterType = 'all' | 'active' | 'completed'
