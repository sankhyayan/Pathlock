import { useState, useEffect } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import type { Project } from '../types'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

// Dev mode - mock projects for UI testing
const DEV_MODE = false
const MOCK_PROJECTS: Project[] = [
  {
    id: '1',
    title: 'Website Redesign',
    description: 'Complete overhaul of company website with modern design',
    createdAt: new Date().toISOString(),
  },
  {
    id: '2',
    title: 'Mobile App Development',
    description: 'Build native iOS and Android applications',
    createdAt: new Date(Date.now() - 86400000).toISOString(),
  },
  {
    id: '3',
    title: 'Marketing Campaign',
    createdAt: new Date(Date.now() - 172800000).toISOString(),
  },
]

export function Dashboard() {
  const [projects, setProjects] = useState<Project[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [showCreateForm, setShowCreateForm] = useState(false)
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  
  const { token, user, logout } = useAuth()
  const navigate = useNavigate()

  useEffect(() => {
    loadProjects()
  }, [])

  const loadProjects = async () => {
    setIsLoading(true)
    setError(null)
    
    // Dev mode - use mock data
    if (DEV_MODE) {
      setTimeout(() => {
        setProjects(MOCK_PROJECTS)
        setIsLoading(false)
      }, 500)
      return
    }
    
    try {
      const response = await fetch(`${API_BASE_URL}/projects`, {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      })

      if (!response.ok) throw new Error('Failed to load projects')
      
      const data = await response.json()
      setProjects(data)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load projects')
    } finally {
      setIsLoading(false)
    }
  }

  const handleCreateProject = async (e: React.FormEvent) => {
    e.preventDefault()
    
    if (!title.trim() || title.length < 3 || title.length > 100) {
      setError('Title must be between 3 and 100 characters')
      return
    }

    if (description && description.length > 500) {
      setError('Description must be at most 500 characters')
      return
    }

    try {
      const response = await fetch(`${API_BASE_URL}/projects`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify({ title, description: description || undefined }),
      })

      if (!response.ok) throw new Error('Failed to create project')
      
      const newProject = await response.json()
      setProjects([newProject, ...projects])
      setTitle('')
      setDescription('')
      setShowCreateForm(false)
      setError(null)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create project')
    }
  }

  const handleDeleteProject = async (id: string) => {
    if (!confirm('Are you sure you want to delete this project? All tasks will be deleted.')) {
      return
    }

    try {
      const response = await fetch(`${API_BASE_URL}/projects/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      })

      if (!response.ok) throw new Error('Failed to delete project')
      
      setProjects(projects.filter(p => p.id !== id))
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to delete project')
    }
  }

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <div className="dashboard-container">
      <header className="dashboard-header">
        <div>
          <h1>My Projects</h1>
          <p className="user-info">Welcome, {DEV_MODE ? 'Demo User' : user?.username}!</p>
        </div>
        <button onClick={handleLogout} className="btn-secondary">Logout</button>
      </header>

      {error && <div className="error-message">{error}</div>}

      <div className="dashboard-actions">
        <button 
          onClick={() => setShowCreateForm(!showCreateForm)} 
          className="btn-primary"
        >
          {showCreateForm ? 'Cancel' : '+ New Project'}
        </button>
      </div>

      {showCreateForm && (
        <form onSubmit={handleCreateProject} className="project-form">
          <div className="form-group">
            <label htmlFor="title">Project Title *</label>
            <input
              id="title"
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="Enter project title (3-100 characters)"
              maxLength={100}
            />
            <div className="char-count">{title.length}/100 characters</div>
          </div>

          <div className="form-group">
            <label htmlFor="description">Description (optional)</label>
            <textarea
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Enter project description (max 500 characters)"
              maxLength={500}
              rows={3}
            />
            <div className="char-count">{description.length}/500 characters</div>
          </div>

          <button type="submit" className="btn-primary">Create Project</button>
        </form>
      )}

      {isLoading ? (
        <div className="loading">Loading projects...</div>
      ) : projects.length === 0 ? (
        <div className="empty-state">
          <p>No projects yet. Create your first project to get started!</p>
        </div>
      ) : (
        <div className="projects-grid">
          {projects.map((project) => (
            <div key={project.id} className="project-card">
              <div className="project-header">
                <h3>{project.title}</h3>
                <button
                  onClick={() => handleDeleteProject(project.id)}
                  className="btn-delete"
                  title="Delete project"
                >
                  ×
                </button>
              </div>
              
              {project.description && (
                <p className="project-description">{project.description}</p>
              )}
              
              <div className="project-footer">
                <span className="project-date">
                  Created: {new Date(project.createdAt).toLocaleDateString()}
                </span>
                <Link to={`/projects/${project.id}`} className="btn-link">
                  View Tasks →
                </Link>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}
