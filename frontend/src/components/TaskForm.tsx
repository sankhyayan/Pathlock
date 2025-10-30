import { useState } from 'react'
import type { Task } from '../types'

interface TaskFormProps {
  onSubmit: (taskData: {
    title: string
    dueDate?: string
    estimatedHours?: number
    dependsOn?: string[]
  }) => Promise<void>
  onCancel: () => void
  availableTasks: Task[]
}

export function TaskForm({ onSubmit, onCancel, availableTasks }: TaskFormProps) {
  const [taskTitle, setTaskTitle] = useState('')
  const [taskDueDate, setTaskDueDate] = useState('')
  const [taskEstimatedHours, setTaskEstimatedHours] = useState('')
  const [taskDependsOn, setTaskDependsOn] = useState<string[]>([])
  const [error, setError] = useState<string | null>(null)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    
    if (!taskTitle.trim()) {
      setError('Task title is required')
      return
    }

    try {
      await onSubmit({
        title: taskTitle,
        dueDate: taskDueDate || undefined,
        estimatedHours: taskEstimatedHours ? Number(taskEstimatedHours) : undefined,
        dependsOn: taskDependsOn.length > 0 ? taskDependsOn : undefined,
      })
      
      // Reset form
      setTaskTitle('')
      setTaskDueDate('')
      setTaskEstimatedHours('')
      setTaskDependsOn([])
      setError(null)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create task')
    }
  }

  return (
    <form onSubmit={handleSubmit} className="task-form">
      {error && <div className="error-message">{error}</div>}
      
      <div className="form-group">
        <label htmlFor="taskTitle">Task Title *</label>
        <input
          id="taskTitle"
          type="text"
          value={taskTitle}
          onChange={(e) => setTaskTitle(e.target.value)}
          placeholder="Enter task title"
        />
      </div>

      <div className="form-group">
        <label htmlFor="taskDueDate">Due Date (optional)</label>
        <input
          id="taskDueDate"
          type="date"
          value={taskDueDate}
          onChange={(e) => setTaskDueDate(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="taskEstimatedHours">Estimated Hours (optional)</label>
        <input
          id="taskEstimatedHours"
          type="number"
          min="0"
          step="0.1"
          value={taskEstimatedHours}
          onChange={(e) => setTaskEstimatedHours(e.target.value)}
          placeholder="e.g., 2 or 1.5"
        />
      </div>

      <div className="form-group">
        <label htmlFor="taskDependencies">Dependencies (optional)</label>
        <div style={{ display: 'flex', gap: '8px', alignItems: 'flex-start' }}>
          <div style={{ flex: 1 }}>
            <select
              id="taskDependencies"
              multiple
              value={taskDependsOn}
              onChange={(e) => setTaskDependsOn(Array.from(e.target.selectedOptions, option => option.value))}
              className="dependency-select"
            >
              {availableTasks
                .filter(t => !t.completed)
                .map(t => (
                  <option key={t.id} value={t.id}>
                    {t.title}
                  </option>
                ))}
            </select>
            <small className="form-help">Hold Ctrl/Cmd to select multiple tasks</small>
          </div>
          {taskDependsOn.length > 0 && (
            <button
              type="button"
              onClick={() => setTaskDependsOn([])}
              className="clear-deps-btn"
              title="Clear all dependencies"
            >
              ✕
            </button>
          )}
        </div>
      </div>

      <div style={{ display: 'flex', gap: '8px' }}>
        <button type="submit" className="btn-primary">Add Task</button>
        <button type="button" onClick={onCancel} className="btn-secondary">Cancel</button>
      </div>
    </form>
  )
}
