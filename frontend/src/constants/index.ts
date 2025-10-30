import type { FilterType } from '../types/index'

export const FILTERS: { value: FilterType; label: string }[] = [
  { value: 'all', label: 'All' },
  { value: 'active', label: 'Active' },
  { value: 'completed', label: 'Completed' },
]

export const STORAGE_KEY = 'tasks'
export const API_BASE_URL = 'http://localhost:5000/api/tasks'