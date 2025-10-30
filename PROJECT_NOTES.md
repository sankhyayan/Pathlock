# Task Manager - Full-Stack Project Notes

**Project Type**: Full-Stack Application (React + TypeScript Frontend + .NET 8 Backend)  
**Frontend**: Vite + React + TypeScript  
**Backend**: ASP.NET Core Web API with file-based persistent storage  
**Created**: October 30, 2025  
**Purpose**: Complete task manager with REST API integration, CRUD operations, filtering, and data persistence

---

## 📁 Project Structure

```
Task_Manager/
├── frontend/
│   ├── index.html              # Entry HTML file
│   ├── package.json            # Dependencies and scripts
│   ├── tsconfig.json           # TypeScript configuration
│   ├── vite.config.ts          # Vite build configuration
│   ├── .gitignore              # Git ignore rules
│   ├── README.md               # Frontend documentation
│   └── src/
│       ├── main.tsx            # React entry point
│       ├── App.tsx             # Root component
│       ├── index.css           # Global styles
│       ├── types/
│       │   └── index.ts        # TypeScript type definitions (Task, FilterType)
│       ├── constants/
│       │   └── index.ts        # App constants (FILTERS, STORAGE_KEY)
│       ├── api/
│       │   └── taskAPI.ts      # API client for backend communication
│       ├── hooks/
│       │   ├── index.ts        # Hooks barrel export
│       │   ├── useTasks.ts     # Task CRUD operations + API integration
│       │   └── useTaskFilter.ts # Task filtering logic with memoization
│       └── components/
│           ├── TaskManager.tsx     # Main container component (orchestrator)
│           ├── TaskForm.tsx        # Add task input form
│           ├── FilterButtons.tsx   # Filter UI (All/Active/Completed)
│           ├── TaskItem.tsx        # Individual task item
│           └── TaskList.tsx        # Task list with empty states
│
└── backend/
    ├── README.md
    └── TaskManager.API.csproj         # Project file (directly in backend folder)
        ├── Controllers/
        │   └── TasksController.cs      # RESTful API endpoints with debug logging
        ├── Models/
        │   └── TaskItem.cs             # Task model (Id, Description, IsCompleted)
        ├── Services/
        │   └── TaskService.cs          # File-based persistent storage service
        ├── Properties/
        │   └── launchSettings.json     # Launch configuration
        ├── Program.cs                  # Application entry point & configuration
        ├── tasks.json                  # Data storage file (auto-generated)
        └── TaskManager.API.http        # HTTP test requests
```

### Module Responsibilities

#### **Frontend Modules**

##### `/types` - Type Definitions
- `Task`: Interface for task objects `{ id: string, title: string, completed: boolean }`
- `FilterType`: Union type for filter states

##### `/constants` - Configuration
- `FILTERS`: Filter button configuration array
- `STORAGE_KEY`: localStorage key constant (deprecated)

##### `/api` - Backend Communication
- `taskAPI`: RESTful API client with GET, POST, PUT, DELETE methods
- Maps between frontend Task and backend TaskDTO

##### `/hooks` - Custom React Hooks
- `useTasks`: Manages task state, CRUD operations, API integration
- `useTaskFilter`: Handles filtering logic with computed counts

##### `/components` - UI Components
- `TaskManager`: Container/orchestrator component
- `TaskForm`: Controlled form for adding tasks (async)
- `FilterButtons`: Filter selection UI with counts
- `TaskItem`: Single task display with toggle/delete
- `TaskList`: Renders filtered tasks with empty states

#### **Backend Modules**

##### `/Controllers` - API Endpoints
- `TasksController`: RESTful endpoints for CRUD operations

##### `/Models` - Data Models
- `TaskItem`: Backend model `{ Id: Guid, Description: string, IsCompleted: bool }`

##### `/services` - Business Logic
- `ITaskService`: Service interface
- `FileBasedTaskService`: Thread-safe file-based persistent storage with JSON serialization

---

## 🚀 Quick Commands

### **Frontend** (React + Vite)
```powershell
cd frontend
npm install
npm run dev  # Runs on http://localhost:5173
```

### **Backend** (.NET API)
```powershell
cd backend
dotnet run  # Runs on http://localhost:5000
```

### **Full Stack** (Both servers)
Open two terminals and run both commands above.

---

## 🎯 Features Implemented

### Core Functionality
- ✅ **Add Task**: Text input + form submission (Enter key or button)
- ✅ **Toggle Completion**: Checkbox to mark tasks complete/incomplete
- ✅ **Delete Task**: × button to remove tasks
- ✅ **Filter Tasks**: All / Active / Completed filter buttons with counts
- ✅ **API Integration**: Full REST API integration with backend
- ✅ **Loading States**: UI feedback during API operations
- ✅ **Error Handling**: Display error messages from API failures
- ✅ **Task Sorting**: Uncompleted tasks always at top, completed at bottom
- ✅ **Data Persistence**: Tasks saved to file, survive backend restarts

### Technical Features - Frontend
- ✅ React Hooks (useState, useEffect, useMemo)
- ✅ Custom hooks for separation of concerns
- ✅ TypeScript for type safety
- ✅ Modular component architecture
- ✅ Async/await for API calls
- ✅ Controlled form inputs
- ✅ Immutable state updates
- ✅ Memoized computed values for performance
- ✅ Responsive CSS styling
- ✅ Empty state handling with context-aware messages
- ✅ Input validation (trim whitespace, reject empty tasks)
- ✅ Dynamic task counts in filter buttons
- ✅ Loading and error UI states

### Technical Features - Backend
- ✅ RESTful API design with 4 endpoints
- ✅ .NET 8 (running on .NET 9) Web API
- ✅ File-based persistent storage with JSON serialization
- ✅ Thread-safe operations with locking
- ✅ CORS enabled for frontend
- ✅ Swagger/OpenAPI documentation
- ✅ Dependency injection
- ✅ Input validation
- ✅ Proper HTTP status codes (200, 201, 204, 400, 404)
- ✅ Comprehensive debug logging with emoji indicators
- ✅ Auto-load on startup, auto-save on changes

---

## 🔧 Technical Details

### API Endpoints
**Base URL**: `http://localhost:5000/api/tasks`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks` | Get all tasks |
| POST | `/api/tasks` | Create new task |
| PUT | `/api/tasks/{id}` | Update task |
| DELETE | `/api/tasks/{id}` | Delete task |

### Data Mapping
**Frontend Task Model**:
```typescript
{ id: string, title: string, completed: boolean }
```

**Backend TaskItem Model**:
```csharp
{ Id: Guid, Description: string, IsCompleted: bool }
```

**API Client** maps between these formats automatically.

### State Management
- **tasks**: Array of Task objects
- **isLoading**: Boolean for API operation status
- **error**: String for error messages
- **title**: Current input field value
- **filter**: Current filter view ('all' | 'active' | 'completed')

### Data Flow
1. User types → `onChange` → `setTitle` → re-render
2. User submits → `handleAdd` → API POST → `setTasks` → re-render
3. User toggles → `toggle` → API PUT → `setTasks` → re-render
4. User deletes → `remove` → API DELETE → `setTasks` → re-render
5. Component mounts → API GET → `setTasks` → render tasks

### Communication Flow
```
Frontend (React) → API Client → HTTP Request → Backend (.NET)
                                              ↓
                                    FileBasedTaskService
                                              ↓
                                    tasks.json (persistent)
                                              ↓
Frontend (React) ← JSON Response ←────────────┘
```

---

## 📝 Development Log

### October 30, 2025
#### **Phase 1: Frontend Setup**
- Created Vite + React + TypeScript project structure
- Implemented TaskManager component with CRUD operations
- Added basic responsive CSS with clean UI
- Verified dev server runs successfully

#### **Phase 2: Feature Enhancement**
- Added task filtering (All/Active/Completed) with dynamic counts
- Implemented styled filter buttons

#### **Phase 3: Code Refactoring**
- Modularized entire frontend codebase:
  - Custom hooks (`useTasks`, `useTaskFilter`)
  - Separate components (`TaskForm`, `FilterButtons`, `TaskItem`, `TaskList`)
  - Type definitions and constants modules
- Improved code organization and maintainability

#### **Phase 4: Backend Development**
- Created .NET 8 Web API project
- Implemented TaskItem model (Guid Id, string Description, bool IsCompleted)
- Built in-memory storage service with thread-safe operations
- Created RESTful API controller with 4 endpoints
- Configured CORS for frontend communication
- Added Swagger/OpenAPI documentation

#### **Phase 5: Full-Stack Integration**
- Created API client module in frontend
- Replaced localStorage with API calls in `useTasks` hook
- Updated Task type to use string ID (GUID from backend)
- Made TaskForm async to handle API calls
- Added loading and error states to UI
- Tested full integration: ✅ Working!

#### **Phase 6: Data Persistence & UX Improvements**
- Replaced in-memory storage with file-based storage
- Implemented FileBasedTaskService with JSON serialization
- Added automatic task sorting (uncompleted first)
- Added comprehensive debug logging with emoji indicators
- Removed localStorage from frontend entirely
- Updated project structure (backend files directly in backend folder)
- Fixed solution file path
- Created comprehensive README.md with setup instructions
- Updated .gitignore for both frontend and backend

---

## 🔮 Backend Integration Status

### ✅ Completed
- ✅ RESTful API endpoints implemented
- ✅ Frontend connected to backend
- ✅ API client module created
- ✅ Data mapping between frontend/backend
- ✅ Loading states implemented
- ✅ Error handling implemented
- ✅ CORS configured
- ✅ Full CRUD operations working

### Current Architecture
```
Frontend (localhost:5173)
    ↓ HTTP Requests
Backend API (localhost:5000)
    ↓
FileBasedTaskService (Thread-Safe)
    ↓
tasks.json (Persistent Storage)
```

### Storage Details
- **Location**: `backend/tasks.json`
- **Format**: JSON with indented formatting
- **Persistence**: Data survives backend restarts
- **Operations**: Auto-save on Create/Update/Delete, Auto-load on startup

### Future Enhancements
1. **Database Integration**
   - Replace file storage with SQL Server/PostgreSQL
   - Add Entity Framework Core
   - Implement proper data migrations

2. **Authentication & Authorization**
   - Add JWT authentication
   - User registration/login
   - Protected API endpoints
   - User-specific tasks

3. **Advanced Features**
   - Task categories/tags
   - Due dates and reminders
   - Task priority levels
   - Real-time sync with SignalR
   - Task search and advanced filtering

---

## 🐛 Known Issues / Technical Debt

- ✅ ~~In-memory storage: Data is lost when backend restarts~~ FIXED: Now using file-based storage
- ⚠️ No authentication: API is open to all requests
- ⚠️ Single file storage: Not suitable for high concurrency (use database for production)
- ⚠️ No data backup mechanism

---

## 💡 Improvement Ideas

### Short Term
- [x] Add task filtering (All/Active/Completed)
- [x] Add data persistence (file-based storage)
- [x] Add automatic task sorting
- [x] Add debug logging
- [ ] Add task editing (inline edit mode)
- [ ] Add "Clear completed" button
- [ ] Add task count display
- [ ] Add keyboard shortcuts (e.g., Ctrl+Enter to add)

### Medium Term
- [ ] Add categories/tags for tasks
- [ ] Add due dates
- [ ] Add priority levels
- [ ] Add drag-and-drop reordering
- [ ] Add dark mode toggle

### Long Term
- [ ] Multi-user support (requires backend)
- [ ] Real-time sync (WebSockets)
- [ ] Offline-first architecture (service workers)
- [ ] Mobile app (React Native)

---

## 📚 Resources & References

### Frontend Documentation
- [React Docs](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Vite Guide](https://vitejs.dev/guide/)

### Backend Documentation
- [ASP.NET Core Docs](https://learn.microsoft.com/en-us/aspnet/core/)
- [.NET 8 API Tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api)

### Key Dependencies - Frontend
- `react` ^18.2.0
- `react-dom` ^18.2.0
- `typescript` ^5.0.4
- `vite` ^5.0.0
- `@vitejs/plugin-react` ^4.0.0

### Key Dependencies - Backend
- `Microsoft.AspNetCore.App` (framework)
- `Swashbuckle.AspNetCore` ^9.0.6
- `Microsoft.AspNetCore.OpenApi` ^9.0.10

---

## 🔐 Security Notes

### Frontend
- XSS protection: React automatically escapes content in JSX
- API calls use fetch with proper headers
- Error messages sanitized before display

### Backend
- CORS configured for specific origin (localhost:5173)
- Input validation on API endpoints
- Thread-safe file operations with locking
- ⚠️ No authentication yet (add JWT for production)
- ⚠️ No rate limiting (add for production)
- ⚠️ HTTPS disabled for development (enable for production)
- ⚠️ File access not encrypted (consider encryption for sensitive data)

---

## 📊 Performance Notes

### Frontend
- Vite dev server: Fast HMR (Hot Module Replacement)
- React re-renders optimized via immutable state updates
- useMemo for filtering and counts computation
- Async operations don't block UI

### Backend
- File-based storage: Fast read/write for small datasets
- JSON serialization with indented formatting
- Thread-safe with locking mechanism
- Auto-save after each operation
- CORS preflight caching enabled
- Emoji-based debug logging for easy visual scanning

---

*Last Updated: October 30, 2025*
