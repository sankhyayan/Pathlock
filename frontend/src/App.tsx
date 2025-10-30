import React from 'react'
import TaskManager from './components/TaskManager'

export default function App() {
  return (
    <div className="app">
      <header>
        <h1>Task Manager</h1>
        <p>A small React + TypeScript SPA for managing tasks</p>
      </header>
      <main>
        <TaskManager />
      </main>
    </div>
  )
}
