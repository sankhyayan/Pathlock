import { Link } from 'react-router-dom'
import type { Project } from '../types'

interface ProjectHeaderProps {
  project: Project
  activeTasks: number
  completedTasks: number
  totalTasks: number
  completionPct: number
}

export function ProjectHeader({
  project,
  activeTasks,
  completedTasks,
  totalTasks,
  completionPct
}: ProjectHeaderProps) {
  return (
    <>
      <header className="project-details-header">
        <div>
          <Link to="/dashboard" className="back-link">← Back to Projects</Link>
          <h1>{project.title}</h1>
          {project.description && <p className="project-description">{project.description}</p>}
          <div className="task-stats">
            <span>{activeTasks} active</span>
            <span>{completedTasks} completed</span>
            <span>{totalTasks} total tasks</span>
          </div>
        </div>
      </header>

      <div className="task-progress">
        <div className="progress-bar">
          <div className="progress-fill" style={{ width: `${completionPct}%` }} />
        </div>
        <span className="progress-label">{completionPct}% complete</span>
      </div>
    </>
  )
}
