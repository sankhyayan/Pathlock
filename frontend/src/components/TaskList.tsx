import type { Task, FilterType } from '../types/index'
import TaskItem from './TaskItem'

interface TaskListProps {
  tasks: Task[]
  filteredTasks: Task[]
  currentFilter: FilterType
  onToggle: (id: string) => void
  onDelete: (id: string) => void
}

export default function TaskList({
  tasks,
  filteredTasks,
  currentFilter,
  onToggle,
  onDelete,
}: TaskListProps) {
  const renderEmptyState = () => {
    if (tasks.length === 0) {
      return <li className="empty">No tasks yet — add one above.</li>
    }
    if (filteredTasks.length === 0) {
      return <li className="empty">No {currentFilter} tasks.</li>
    }
    return null
  }

  return (
    <ul className="task-list">
      {renderEmptyState()}
      {filteredTasks.map((task) => (
        <TaskItem
          key={task.id}
          task={task}
          onToggle={onToggle}
          onDelete={onDelete}
        />
      ))}
    </ul>
  )
}
