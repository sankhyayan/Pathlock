import { useState, useMemo } from 'react'
import type { Task, FilterType } from '../types/index'

export function useTaskFilter(tasks: Task[]) {
  const [filter, setFilter] = useState<FilterType>('all')

  const filteredTasks = useMemo(() => {
    switch (filter) {
      case 'active':
        return tasks.filter((task) => !task.completed)
      case 'completed':
        return tasks.filter((task) => task.completed)
      default:
        return tasks
    }
  }, [tasks, filter])

  const counts = useMemo(
    () => ({
      all: tasks.length,
      active: tasks.filter((task) => !task.completed).length,
      completed: tasks.filter((task) => task.completed).length,
    }),
    [tasks]
  )

  return {
    filter,
    setFilter,
    filteredTasks,
    counts,
  }
}
