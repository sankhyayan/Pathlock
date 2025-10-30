import type { Task } from '../types'

interface TaskItemProps {
  task: Task
  onToggle: (id: string) => void
  onDelete: (id: string) => void
}

export default function TaskItem({ task, onToggle, onDelete }: TaskItemProps) {
  return (
    <li className={task.completed ? 'completed' : ''}>
      <label>
        <input
          type="checkbox"
          checked={task.completed}
          onChange={() => onToggle(task.id)}
        />
        <span className="title">{task.title}</span>
      </label>
      <button
        className="delete"
        onClick={() => onDelete(task.id)}
        aria-label={`Delete ${task.title}`}
      >
        ×
      </button>
    </li>
  )
}
