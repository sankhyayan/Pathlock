import { useState } from 'react'
import type { Task } from '../types'
import { getDueStatus } from '../utils/taskUtils'

interface TaskItemProps {
  task: Task
  availableTasks: Task[]
  onToggle: (taskId: string, completed: boolean) => Promise<void>
  onUpdate: (taskId: string, updates: {
    title: string
    dueDate?: string | null
    estimatedHours?: number | null
    dependsOn?: string[]
  }) => Promise<void>
  onDelete: (taskId: string) => Promise<void>
}

export function TaskItem({ task, availableTasks, onToggle, onUpdate, onDelete }: TaskItemProps) {
  const [isEditing, setIsEditing] = useState(false)
  const [editTitle, setEditTitle] = useState(task.title)
  const [editDueDate, setEditDueDate] = useState(
    task.dueDate ? new Date(task.dueDate).toISOString().slice(0, 10) : ''
  )
  const [editEstimatedHours, setEditEstimatedHours] = useState(
    typeof task.estimatedHours === 'number' ? String(task.estimatedHours) : ''
  )
  const [editDependsOn, setEditDependsOn] = useState<string[]>(task.dependsOn || [])

  const handleSave = async () => {
    if (!editTitle.trim()) {
      alert('Task title is required')
      return
    }

    try {
      await onUpdate(task.id, {
        title: editTitle,
        dueDate: editDueDate ? new Date(editDueDate).toISOString() : null,
        estimatedHours: editEstimatedHours !== '' ? Number(editEstimatedHours) : null,
        dependsOn: editDependsOn.length > 0 ? editDependsOn : [],
      })
      setIsEditing(false)
    } catch (err) {
      console.error('Failed to update task:', err)
    }
  }

  const handleCancel = () => {
    setIsEditing(false)
    setEditTitle(task.title)
    setEditDueDate(task.dueDate ? new Date(task.dueDate).toISOString().slice(0, 10) : '')
    setEditEstimatedHours(typeof task.estimatedHours === 'number' ? String(task.estimatedHours) : '')
    setEditDependsOn(task.dependsOn || [])
  }

  const dueStatus = getDueStatus(task)

  return (
    <div className={`task-item ${task.completed ? 'completed' : ''} ${dueStatus}`}>
      <input
        type="checkbox"
        checked={task.completed}
        onChange={(e) => onToggle(task.id, e.target.checked)}
      />
      
      {isEditing ? (
        <div className="task-content">
          <div className="task-edit-row">
            <input
              type="text"
              value={editTitle}
              onChange={(e) => setEditTitle(e.target.value)}
              placeholder="Task title"
            />
          </div>
          <div className="task-edit-row">
            <input
              type="date"
              value={editDueDate}
              onChange={(e) => setEditDueDate(e.target.value)}
            />
            <input
              type="number"
              min="0"
              step="0.1"
              value={editEstimatedHours}
              onChange={(e) => setEditEstimatedHours(e.target.value)}
              placeholder="Est. hours"
            />
          </div>
          <div className="task-edit-row">
            <div style={{ display: 'flex', gap: '8px', alignItems: 'flex-start' }}>
              <select
                multiple
                value={editDependsOn}
                onChange={(e) => setEditDependsOn(Array.from(e.target.selectedOptions, option => option.value))}
                className="dependency-select-inline"
                title="Hold Ctrl/Cmd to select multiple"
              >
                {availableTasks
                  .filter(t => !t.completed && t.id !== task.id)
                  .map(t => (
                    <option key={t.id} value={t.id}>
                      {t.title}
                    </option>
                  ))}
              </select>
              {editDependsOn.length > 0 && (
                <button
                  type="button"
                  onClick={() => setEditDependsOn([])}
                  className="clear-deps-btn"
                  title="Clear all dependencies"
                >
                  ✕
                </button>
              )}
            </div>
          </div>
        </div>
      ) : (
        <div className="task-content">
          <div className="task-title-row">
            <span className="task-title">{task.title}</span>
            {typeof task.estimatedHours === 'number' && (
              <span className="task-estimate">{task.estimatedHours}h</span>
            )}
          </div>
          <div className="task-meta-row">
            {task.dueDate && (
              <span className={`task-due-chip ${dueStatus}`}>
                Due {new Date(task.dueDate).toLocaleDateString()}
              </span>
            )}
            {task.completed && <span className="task-completed-chip">Completed</span>}
          </div>
        </div>
      )}
      
      <div className="task-actions-col">
        {isEditing ? (
          <>
            <button className="btn-secondary" onClick={handleSave}>Save</button>
            <button className="btn-secondary" onClick={handleCancel}>Cancel</button>
          </>
        ) : (
          <>
            <button className="btn-secondary" onClick={() => setIsEditing(true)}>Edit</button>
            <button
              onClick={() => onDelete(task.id)}
              className="btn-delete"
              title="Delete task"
            >
              ×
            </button>
          </>
        )}
      </div>
    </div>
  )
}
