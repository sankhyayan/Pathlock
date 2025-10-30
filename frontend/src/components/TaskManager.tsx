import React from 'react'
import { useTasks } from '../hooks/useTasks'
import { useTaskFilter } from '../hooks/useTaskFilter'
import TaskForm from './TaskForm'
import FilterButtons from './FilterButtons'
import TaskList from './TaskList'

export default function TaskManager() {
  const { tasks, isLoading, error, addTask, toggleTask, deleteTask } = useTasks()
  const { filter, setFilter, filteredTasks, counts } = useTaskFilter(tasks)

  return (
    <section className="task-manager">
      {error && <div className="error-message">{error}</div>}
      <TaskForm onAdd={addTask} />
      <FilterButtons
        currentFilter={filter}
        onFilterChange={setFilter}
        counts={counts}
      />
      {isLoading && <div className="loading">Loading...</div>}
      <TaskList
        tasks={tasks}
        filteredTasks={filteredTasks}
        currentFilter={filter}
        onToggle={toggleTask}
        onDelete={deleteTask}
      />
    </section>
  )
}
