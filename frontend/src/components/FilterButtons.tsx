import React from 'react'
import type { FilterType } from '../types/index'
import { FILTERS } from '../constants'

interface FilterButtonsProps {
  currentFilter: FilterType
  onFilterChange: (filter: FilterType) => void
  counts: {
    all: number
    active: number
    completed: number
  }
}

export default function FilterButtons({
  currentFilter,
  onFilterChange,
  counts,
}: FilterButtonsProps) {
  return (
    <div className="filter-buttons">
      {FILTERS.map(({ value, label }) => (
        <button
          key={value}
          className={currentFilter === value ? 'active' : ''}
          onClick={() => onFilterChange(value)}
        >
          {label} ({counts[value as keyof typeof counts]})
        </button>
      ))}
    </div>
  )
}
