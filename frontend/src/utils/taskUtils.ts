import type { Task } from '../types'

export type DueStatus = 'no-due' | 'overdue' | 'due-soon' | 'on-time'

export function getDueStatus(task: Task): DueStatus {
  if (!task.dueDate) return 'no-due'
  const now = new Date().getTime()
  const due = new Date(task.dueDate).getTime()
  if (due < now && !task.completed) return 'overdue'
  const threeDays = 3 * 24 * 60 * 60 * 1000
  if (due - now <= threeDays && !task.completed) return 'due-soon'
  return 'on-time'
}

export function sortTasks(tasks: Task[]): Task[] {
  return [...tasks].sort((a, b) => {
    // Completed tasks go to bottom
    if (a.completed !== b.completed) return a.completed ? 1 : -1
    // Sort by due date (earliest first)
    const ad = a.dueDate ? new Date(a.dueDate).getTime() : Infinity
    const bd = b.dueDate ? new Date(b.dueDate).getTime() : Infinity
    return ad - bd
  })
}

export function filterTasks(
  tasks: Task[],
  filter: 'all' | 'active' | 'completed'
): Task[] {
  if (filter === 'active') return tasks.filter(t => !t.completed)
  if (filter === 'completed') return tasks.filter(t => t.completed)
  return tasks
}

export function calculateCompletionStats(tasks: Task[]) {
  const activeTasks = tasks.filter(t => !t.completed).length
  const completedTasks = tasks.filter(t => t.completed).length
  const completionPct = tasks.length > 0 
    ? Math.round((completedTasks / tasks.length) * 100) 
    : 0

  return { activeTasks, completedTasks, completionPct }
}
