import { API_BASE_URL } from "../constants/index"

export interface TaskDTO {
  id?: string
  description: string
  isCompleted: boolean
}

export const taskAPI = {
  // GET /api/tasks - Get all tasks
  async getAll(): Promise<TaskDTO[]> {
    const response = await fetch(API_BASE_URL)
    if (!response.ok) {
      throw new Error(`Failed to fetch tasks: ${response.statusText}`)
    }
    return response.json()
  },

  // POST /api/tasks - Create a new task
  async create(task: Omit<TaskDTO, 'id'>): Promise<TaskDTO> {
    const response = await fetch(API_BASE_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(task),
    })
    if (!response.ok) {
      throw new Error(`Failed to create task: ${response.statusText}`)
    }
    return response.json()
  },

  // PUT /api/tasks/{id} - Update a task
  async update(id: string, task: Omit<TaskDTO, 'id'>): Promise<TaskDTO> {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(task),
    })
    if (!response.ok) {
      throw new Error(`Failed to update task: ${response.statusText}`)
    }
    return response.json()
  },

  // DELETE /api/tasks/{id} - Delete a task
  async delete(id: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
      method: 'DELETE',
    })
    if (!response.ok) {
      throw new Error(`Failed to delete task: ${response.statusText}`)
    }
  },
}
