import type { Project, Task } from '../types'
import { getDueStatus } from '../utils/taskUtils'

interface ScheduleViewProps {
  project: Project
  tasks: Task[]
  scheduledOrder: string[]
  onBack: () => void
  error?: string | null
}

export function ScheduleView({ project, tasks, scheduledOrder, onBack, error }: ScheduleViewProps) {
  const taskMap = tasks.reduce((acc, task) => {
    acc[task.id] = task
    return acc
  }, {} as Record<string, Task>)

  const scheduledTasks = scheduledOrder
    .map(id => taskMap[id])
    .filter(Boolean)

  return (
    <div className="project-details-container">
      <header className="project-details-header">
        <div>
          <button onClick={onBack} className="back-link">← Back to Project</button>
          <h1>🧠 Smart Schedule: {project.title}</h1>
          {project.description && <p className="project-description">{project.description}</p>}
          <div className="schedule-info">
            <span>📋 {scheduledTasks.length} tasks in recommended order</span>
          </div>
        </div>
      </header>

      {error && <div className="error-message">{error}</div>}

      {scheduledTasks.length === 0 ? (
        <div className="empty-state">
          <p>No tasks to schedule. Add tasks to your project first.</p>
        </div>
      ) : (
        <div className="scheduled-list">
          {scheduledTasks.map((task, index) => {
            const dueStatus = getDueStatus(task)
            return (
              <div key={task.id} className={`scheduled-item ${task.completed ? 'completed' : ''} ${dueStatus}`}>
                <div className="schedule-order">{index + 1}</div>
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
              </div>
            )
          })}
        </div>
      )}
    </div>
  )
}
