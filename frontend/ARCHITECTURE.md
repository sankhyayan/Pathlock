# Modular Architecture Guide

## Overview
The Task Manager application has been refactored into a clean, modular architecture following React best practices and separation of concerns.

## Architecture Principles

### 1. **Separation of Concerns**
Each module has a single, well-defined responsibility:
- **Types**: Define data structures
- **Constants**: Centralize configuration
- **Hooks**: Encapsulate business logic and state management
- **Components**: Handle UI rendering and user interactions

### 2. **Component Hierarchy**

```
App
 └─ TaskManager (Container/Orchestrator)
     ├─ TaskForm (Add tasks)
     ├─ FilterButtons (Filter selection)
     └─ TaskList (Display tasks)
         └─ TaskItem (Individual task) [repeated]
```

### 3. **Data Flow**

```
useTasks Hook
  ↓ (provides tasks, addTask, toggleTask, deleteTask)
TaskManager
  ↓ (passes callbacks down)
Child Components
  ↓ (user interactions)
Callbacks trigger state updates
  ↓
React re-renders affected components
```

## Module Details

### `/src/types/index.ts`
**Purpose**: Centralized type definitions

```typescript
export interface Task {
  id: number
  title: string
  completed: boolean
}

export type FilterType = 'all' | 'active' | 'completed'
```

**Benefits**:
- Single source of truth for types
- Easy to update across the app
- Better TypeScript IntelliSense

---

### `/src/constants/index.ts`
**Purpose**: Application constants and configuration

```typescript
export const FILTERS: { value: FilterType; label: string }[] = [
  { value: 'all', label: 'All' },
  { value: 'active', label: 'Active' },
  { value: 'completed', label: 'Completed' },
]

export const STORAGE_KEY = 'tasks'
```

**Benefits**:
- Easy to modify filter options
- Centralized storage key management
- Prevents magic strings in code

---

### `/src/hooks/useTasks.ts`
**Purpose**: Manage task state and localStorage persistence

**Exports**:
- `tasks`: Array of all tasks
- `addTask(title)`: Add new task, returns boolean success
- `toggleTask(id)`: Toggle task completion status
- `deleteTask(id)`: Remove task by ID

**Key Features**:
- Automatic localStorage sync on mount and updates
- Error handling for storage operations
- Immutable state updates
- Input validation (trim, reject empty)

**Usage**:
```typescript
const { tasks, addTask, toggleTask, deleteTask } = useTasks()
```

---

### `/src/hooks/useTaskFilter.ts`
**Purpose**: Handle task filtering logic with performance optimization

**Exports**:
- `filter`: Current filter type
- `setFilter`: Update filter
- `filteredTasks`: Filtered task array
- `counts`: Object with task counts `{ all, active, completed }`

**Key Features**:
- `useMemo` for performance (avoids re-filtering on unrelated renders)
- Computed counts for filter buttons
- Clean separation of filtering logic

**Usage**:
```typescript
const { filter, setFilter, filteredTasks, counts } = useTaskFilter(tasks)
```

---

### `/src/components/TaskManager.tsx`
**Purpose**: Container component that orchestrates the app

**Role**: "Smart Component" / "Container"
- Manages hooks (state sources)
- Coordinates data flow
- Passes callbacks to children
- Minimal UI logic

**Benefits**:
- Single point of coordination
- Easy to test business logic
- Clear component boundaries

---

### `/src/components/TaskForm.tsx`
**Purpose**: Controlled form for adding tasks

**Props**:
- `onAdd(title: string): boolean` - Callback to add task

**Features**:
- Local state for input value
- Form submission handling
- Clears input on success
- Accessible form labels

**Role**: "Dumb Component" / "Presentational"

---

### `/src/components/FilterButtons.tsx`
**Purpose**: Display filter options with task counts

**Props**:
- `currentFilter`: Active filter
- `onFilterChange(filter)`: Callback when filter changes
- `counts`: Task counts object

**Features**:
- Maps over FILTERS constant
- Highlights active filter
- Shows dynamic counts

---

### `/src/components/TaskItem.tsx`
**Purpose**: Render individual task with controls

**Props**:
- `task`: Task object
- `onToggle(id)`: Toggle callback
- `onDelete(id)`: Delete callback

**Features**:
- Checkbox for completion toggle
- Delete button with accessibility label
- Conditional styling for completed state

**Role**: Pure presentational component

---

### `/src/components/TaskList.tsx`
**Purpose**: Render list of filtered tasks

**Props**:
- `tasks`: All tasks (for empty state logic)
- `filteredTasks`: Tasks to display
- `currentFilter`: For empty state message
- `onToggle`, `onDelete`: Pass-through callbacks

**Features**:
- Smart empty state rendering
- Maps over filtered tasks
- Renders TaskItem components

---

## Benefits of This Architecture

### 1. **Maintainability**
- Each file has < 100 lines
- Clear responsibilities
- Easy to locate code

### 2. **Testability**
- Hooks can be tested in isolation
- Components have clear props
- Pure functions are easy to test

### 3. **Reusability**
- `useTasks` could be used in other views
- `TaskItem` could be reused elsewhere
- Components are self-contained

### 4. **Scalability**
- Easy to add new features
- Can add more filters without touching task logic
- Can add new task properties by updating types

### 5. **Performance**
- `useMemo` prevents unnecessary recalculations
- Component splits enable fine-grained re-renders
- React can optimize component updates

### 6. **Developer Experience**
- TypeScript IntelliSense works great
- Clear imports show dependencies
- Easy to onboard new developers

---

## Future Backend Integration

When adding API calls:

1. **Create `/src/api/tasks.ts`**:
   ```typescript
   export const taskAPI = {
     getAll: () => fetch('/api/tasks').then(r => r.json()),
     create: (task) => fetch('/api/tasks', { method: 'POST', body: JSON.stringify(task) }),
     update: (id, updates) => fetch(`/api/tasks/${id}`, { method: 'PATCH', body: JSON.stringify(updates) }),
     delete: (id) => fetch(`/api/tasks/${id}`, { method: 'DELETE' }),
   }
   ```

2. **Update `useTasks.ts`**:
   - Replace localStorage with API calls
   - Add loading states
   - Add error handling
   - Keep the same interface (no component changes needed!)

3. **Add `/src/hooks/useAPI.ts`** (optional):
   - Generic hook for API calls with loading/error states
   - Can be used by other features

---

## Code Organization Best Practices

✅ **DO**:
- One component per file
- Co-locate related code (hooks with related logic)
- Use barrel exports (`index.ts`) for cleaner imports
- Keep components under 100 lines
- Extract complex logic to hooks
- Use TypeScript for all files

❌ **DON'T**:
- Mix business logic with UI in components
- Create deeply nested component hierarchies
- Put unrelated code in same file
- Use any type (leverage TypeScript)
- Mutate state directly

---

*This architecture is ready for production and scales well to larger applications.*
