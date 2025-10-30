import React, { useState } from 'react'

interface TaskFormProps {
  onAdd: (title: string) => Promise<boolean>
}

export default function TaskForm({ onAdd }: TaskFormProps) {
  const [title, setTitle] = useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    const success = await onAdd(title)
    if (success) {
      setTitle('')
    }
  }

  return (
    <form onSubmit={handleSubmit} className="task-form">
      <input
        aria-label="New task title"
        placeholder="Add a new task and press Enter"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />
      <button type="submit">Add</button>
    </form>
  )
}
