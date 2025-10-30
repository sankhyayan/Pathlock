import { useState, useEffect } from 'react'
import type { Task } from '../types'
import { taskAPI } from '../api/taskAPI'

export function useTasks() {
  const [tasks, setTasks] = useState<Task[]>([])
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  // Load tasks from API on mount
  useEffect(() => {
    loadTasks()
  }, [])

  const loadTasks = async () => {
    setIsLoading(true)
    setError(null)
    try {
      const data = await taskAPI.getAll()
      // Map backend DTO to frontend Task type
      const mappedTasks: Task[] = data.map((dto) => ({
        id: dto.id!,
        title: dto.description,
        completed: dto.isCompleted,
      }))
      // Sort: uncompleted tasks first, then completed tasks
      const sortedTasks = mappedTasks.sort((a, b) => {
        if (a.completed === b.completed) return 0
        return a.completed ? 1 : -1
      })
      setTasks(sortedTasks)
    } catch (err) {
      console.error('Failed to load tasks:', err)
      setError('Failed to load tasks')
    } finally {
      setIsLoading(false)
    }
  }

  const addTask = async (title: string) => {
    const trimmed = title.trim()
    if (!trimmed) return false

    setIsLoading(true)
    setError(null)
    try {
      const newTaskDTO = await taskAPI.create({
        description: trimmed,
        isCompleted: false,
      })
      
      const newTask: Task = {
        id: newTaskDTO.id!,
        title: newTaskDTO.description,
        completed: newTaskDTO.isCompleted,
      }
      
      setTasks((prev) => [newTask, ...prev])
      return true
    } catch (err) {
      console.error('Failed to add task:', err)
      setError('Failed to add task')
      return false
    } finally {
      setIsLoading(false)
    }
  }

  const toggleTask = async (id: string) => {
    const task = tasks.find((t) => t.id === id)
    if (!task) return

    setIsLoading(true)
    setError(null)
    try {
      const updatedDTO = await taskAPI.update(id, {
        description: task.title,
        isCompleted: !task.completed,
      })

      setTasks((prev) => {
        const updated = prev.map((t) =>
          t.id === id
            ? { ...t, completed: updatedDTO.isCompleted }
            : t
        )
        // Sort: uncompleted tasks first, then completed tasks
        return updated.sort((a, b) => {
          if (a.completed === b.completed) return 0
          return a.completed ? 1 : -1
        })
      })
    } catch (err) {
      console.error('Failed to toggle task:', err)
      setError('Failed to update task')
    } finally {
      setIsLoading(false)
    }
  }

  const deleteTask = async (id: string) => {
    setIsLoading(true)
    setError(null)
    try {
      await taskAPI.delete(id)
      setTasks((prev) => prev.filter((task) => task.id !== id))
    } catch (err) {
      console.error('Failed to delete task:', err)
      setError('Failed to delete task')
    } finally {
      setIsLoading(false)
    }
  }

  return {
    tasks,
    isLoading,
    error,
    addTask,
    toggleTask,
    deleteTask,
  }
}
