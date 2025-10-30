import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider } from './contexts/AuthContext'
import { Login } from './components/Login'
import { Register } from './components/Register'
import { Dashboard } from './components/Dashboard'
import { ProjectDetails } from './components/ProjectDetails'
import { ProtectedRoute } from './components/ProtectedRoute'

// Dev mode - set to false to connect to backend
const DEV_MODE = false

export default function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="app">
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route
              path="/dashboard"
              element={
                DEV_MODE ? <Dashboard /> : (
                  <ProtectedRoute>
                    <Dashboard />
                  </ProtectedRoute>
                )
              }
            />
            <Route
              path="/projects/:projectId"
              element={
                DEV_MODE ? <ProjectDetails /> : (
                  <ProtectedRoute>
                    <ProjectDetails />
                  </ProtectedRoute>
                )
              }
            />
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
          </Routes>
        </div>
      </Router>
    </AuthProvider>
  )
}
