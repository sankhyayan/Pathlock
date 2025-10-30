import type { Project, Task } from '../types'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

export async function fetchProject(projectId: string, token: string): Promise<Project> {
  const response = await fetch(`${API_BASE_URL}/projects/${projectId}`, {
    headers: { 'Authorization': `Bearer ${token}` },
  })
  
  if (!response.ok) throw new Error('Failed to load project')
  return response.json()
}

export async function fetchProjectTasks(projectId: string, token: string): Promise<Task[]> {
  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/tasks`, {
    headers: { 'Authorization': `Bearer ${token}` },
  })
  
  if (!response.ok) throw new Error('Failed to load tasks')
  return response.json()
}

export async function createTask(
  projectId: string,
  token: string,
  taskData: {
    title: string
    dueDate?: string
    estimatedHours?: number
    dependsOn?: string[]
  }
): Promise<Task> {
  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/tasks`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`,
    },
    body: JSON.stringify({
      title: taskData.title,
      dueDate: taskData.dueDate || undefined,
      estimatedHours: taskData.estimatedHours || undefined,
      dependsOn: taskData.dependsOn && taskData.dependsOn.length > 0 ? taskData.dependsOn : undefined,
    }),
  })

  if (!response.ok) throw new Error('Failed to create task')
  return response.json()
}

export async function updateTask(
  taskId: string,
  token: string,
  taskData: {
    title: string
    completed: boolean
    dueDate?: string | null
    estimatedHours?: number | null
    dependsOn?: string[]
  }
): Promise<Task> {
  const response = await fetch(`${API_BASE_URL}/tasks/${taskId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`,
    },
    body: JSON.stringify({
      title: taskData.title,
      completed: taskData.completed,
      dueDate: taskData.dueDate || null,
      estimatedHours: taskData.estimatedHours || null,
      dependsOn: taskData.dependsOn || [],
    }),
  })

  if (!response.ok) throw new Error('Failed to update task')
  return response.json()
}

export async function deleteTask(taskId: string, token: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/tasks/${taskId}`, {
    method: 'DELETE',
    headers: { 'Authorization': `Bearer ${token}` },
  })

  if (!response.ok) throw new Error('Failed to delete task')
}

export async function generateSchedule(
  projectId: string,
  token: string
): Promise<{ recommendedOrder: string[] }> {
  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/schedule`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
    },
  })

  if (!response.ok) throw new Error('Failed to generate schedule')
  return response.json()
}
