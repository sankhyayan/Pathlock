import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import type { Task } from '../types'
import { ProjectHeader } from './ProjectHeader'
import { TaskForm } from './TaskForm'
import { TaskItem } from './TaskItem'
import { ScheduleView } from './ScheduleView'
import { 
  calculateCompletionStats, 
  filterTasks, 
  sortTasks 
} from '../utils/taskUtils'
import {
  fetchProject,
  fetchProjectTasks,
  createTask,
  updateTask,
  deleteTask,
  generateSchedule
} from '../services/apiService'

export function ProjectDetails() {
  const { projectId } = useParams<{ projectId: string }>()
  const [project, setProject] = useState<any>(null)
  const [tasks, setTasks] = useState<Task[]>([])
  const [filter, setFilter] = useState<'all' | 'active' | 'completed'>('all')
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [showTaskForm, setShowTaskForm] = useState(false)
  const [isScheduleView, setIsScheduleView] = useState(false)
  const [scheduledOrder, setScheduledOrder] = useState<string[]>([])
  const [isScheduling, setIsScheduling] = useState(false)
  
  const { token } = useAuth()

  useEffect(() => {
    if (projectId) {
      loadProjectAndTasks()
    }
  }, [projectId])

  const loadProjectAndTasks = async () => {
    if (!projectId || !token) return
    
    setIsLoading(true)
    setError(null)
    
    try {
      const [projectData, tasksData] = await Promise.all([
        fetchProject(projectId, token),
        fetchProjectTasks(projectId, token)
      ])
      
      setProject(projectData)
      setTasks(tasksData)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load data')
    } finally {
      setIsLoading(false)
    }
  }

  const handleCreateTask = async (taskData: {
    title: string
    dueDate?: string
    estimatedHours?: number
    dependsOn?: string[]
  }) => {
    if (!projectId || !token) return

    try {
      const newTask = await createTask(projectId, token, taskData)
      setTasks([...tasks, newTask])
      setShowTaskForm(false)
      setError(null)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create task')
      throw err
    }
  }

  const handleToggleTask = async (taskId: string, completed: boolean) => {
    if (!token) return

    try {
      const current = tasks.find(t => t.id === taskId)
      if (!current) {
        setError('Task not found')
        return
      }

      const updatedTask = await updateTask(taskId, token, {
        title: current.title,
        completed,
        dueDate: current.dueDate || null,
        estimatedHours: current.estimatedHours || null,
        dependsOn: current.dependsOn || [],
      })

      setTasks(sortTasks(tasks.map(t => t.id === taskId ? updatedTask : t)))
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update task')
    }
  }

  const handleUpdateTask = async (
    taskId: string, 
    updates: {
      title: string
      dueDate?: string | null
      estimatedHours?: number | null
      dependsOn?: string[]
    }
  ) => {
    if (!token) return

    try {
      const current = tasks.find(t => t.id === taskId)
      if (!current) return

      const updatedTask = await updateTask(taskId, token, {
        ...updates,
        completed: current.completed,
      })

      setTasks(tasks.map(t => t.id === taskId ? updatedTask : t))
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update task')
      throw err
    }
  }

  const handleDeleteTask = async (taskId: string) => {
    if (!token) return

    try {
      await deleteTask(taskId, token)
      setTasks(tasks.filter(t => t.id !== taskId))
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to delete task')
    }
  }

  const handleSmartSchedule = async () => {
    if (!projectId || !token) return
    
    setIsScheduling(true)
    setError(null)
    
    try {
      const schedule = await generateSchedule(projectId, token)
      setScheduledOrder(schedule.recommendedOrder || [])
      setIsScheduleView(true)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to generate schedule')
    } finally {
      setIsScheduling(false)
    }
  }

  const exitScheduleView = () => {
    setIsScheduleView(false)
    setScheduledOrder([])
  }

  if (isLoading) return <div className="loading">Loading...</div>
  if (!project) return <div className="error-message">Project not found</div>

  const { activeTasks, completedTasks, completionPct } = calculateCompletionStats(tasks)
  const filteredAndSortedTasks = sortTasks(filterTasks(tasks, filter))

  // Render scheduled view
  if (isScheduleView) {
    return (
      <ScheduleView
        project={project}
        tasks={tasks}
        scheduledOrder={scheduledOrder}
        onBack={exitScheduleView}
        error={error}
      />
    )
  }

  return (
    <div className="project-details-container">
      <ProjectHeader
        project={project}
        activeTasks={activeTasks}
        completedTasks={completedTasks}
        totalTasks={tasks.length}
        completionPct={completionPct}
      />

      {error && <div className="error-message">{error}</div>}

      <div className="task-actions">
        <button 
          onClick={() => setShowTaskForm(!showTaskForm)} 
          className="btn-primary"
        >
          {showTaskForm ? 'Cancel' : '+ Add Task'}
        </button>
        <button 
          onClick={handleSmartSchedule}
          className="btn-secondary"
          disabled={isScheduling || tasks.length === 0}
          title="Generate smart schedule based on dependencies, due dates, and estimates"
        >
          {isScheduling ? '⏳ Scheduling...' : '🧠 Smart Schedule'}
        </button>
      </div>

      <div className="filter-buttons">
        <button
          className={filter === 'all' ? 'active' : ''}
          onClick={() => setFilter('all')}
        >
          All ({tasks.length})
        </button>
        <button
          className={filter === 'active' ? 'active' : ''}
          onClick={() => setFilter('active')}
        >
          Active ({activeTasks})
        </button>
        <button
          className={filter === 'completed' ? 'active' : ''}
          onClick={() => setFilter('completed')}
        >
          Completed ({completedTasks})
        </button>
      </div>

      {showTaskForm && (
        <TaskForm
          onSubmit={handleCreateTask}
          onCancel={() => setShowTaskForm(false)}
          availableTasks={tasks}
        />
      )}

      {tasks.length === 0 ? (
        <div className="empty-state">
          <p>No tasks yet. Add your first task to get started!</p>
        </div>
      ) : filteredAndSortedTasks.length === 0 ? (
        <div className="empty-state">
          <p>No {filter} tasks found.</p>
        </div>
      ) : (
        <div className="task-list">
          {filteredAndSortedTasks.map((task) => (
            <TaskItem
              key={task.id}
              task={task}
              availableTasks={tasks}
              onToggle={handleToggleTask}
              onUpdate={handleUpdateTask}
              onDelete={handleDeleteTask}
            />
          ))}
        </div>
      )}
    </div>
  )
}
