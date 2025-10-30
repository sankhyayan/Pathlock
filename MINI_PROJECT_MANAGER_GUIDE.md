# Mini Project Manager - Setup Guide

## Quick Start

### 1. Start the Backend

```bash
cd backend
dotnet run
```
Backend runs on: `http://localhost:5001`

### 2. Start the Frontend

```bash
cd frontend
npm install  # First time only
npm run dev
```
Frontend runs on: `http://localhost:5173`

### 3. Test the Application

**Option A: With Backend**
1. Set `DEV_MODE = false` in `frontend/src/App.tsx`
2. Register a new user
3. Login and start creating projects

**Option B: Dev Mode (UI Testing)**
1. Set `DEV_MODE = true` in `frontend/src/App.tsx`
2. Opens directly to dashboard with mock data
3. Test UI features without backend

## API Endpoints

### Authentication (No Auth Required)
```
POST /api/auth/register
POST /api/auth/login
```

### Projects (Auth Required)
```
GET    /api/projects
GET    /api/projects/{id}
POST   /api/projects
DELETE /api/projects/{id}
```

### Tasks (Auth Required)
```
GET    /api/projects/{projectId}/tasks
POST   /api/projects/{projectId}/tasks
PUT    /api/tasks/{id}
DELETE /api/tasks/{id}
```

## Key Files Modified

### Backend
- ✅ `Models/User.cs` - New user model with validation
- ✅ `Models/Project.cs` - New project model  
- ✅ `Models/TaskItem.cs` - Updated (added ProjectId, removed Description)
- ✅ `DTOs/*.cs` - All DTOs for API contracts
- ✅ `Services/AuthService.cs` - JWT & BCrypt authentication
- ✅ `Services/UserService.cs` - File-based user storage
- ✅ `Services/ProjectService.cs` - File-based project storage
- ✅ `Services/TaskService.cs` - Updated for projects (async methods)
- ✅ `Controllers/AuthController.cs` - Register/Login endpoints
- ✅ `Controllers/ProjectsController.cs` - Project & task CRUD
- ✅ `Controllers/TasksController.cs` - Task update/delete
- ✅ `Program.cs` - JWT configuration
- ✅ `appsettings.json` - JWT settings

### Frontend
- ✅ `contexts/AuthContext.tsx` - Authentication state management
- ✅ `components/Login.tsx` - Login form
- ✅ `components/Register.tsx` - Registration form
- ✅ `components/Dashboard.tsx` - Project list with CRUD
- ✅ `components/ProjectDetails.tsx` - Task management with filtering
- ✅ `components/ProtectedRoute.tsx` - Route protection
- ✅ `App.tsx` - Router setup with DEV_MODE
- ✅ `types/index.ts` - TypeScript types
- ✅ `index.css` - Complete CSS overhaul

## Storage Files (Auto-created)

The backend creates these JSON files in the backend directory:
- `users.json` - User accounts with hashed passwords
- `projects.json` - User projects
- `tasks.json` - Project tasks

## Validation Rules

**Users:**
- Username: min 3 chars
- Email: valid format
- Password: min 6 chars

**Projects:**
- Title: 3-100 chars
- Description: max 500 chars (optional)

**Tasks:**
- Title: 1-100 chars
- Due Date: optional
- Completed: boolean

## Security

- JWT Bearer tokens (24hr expiry)
- BCrypt password hashing
- User data isolation
- Project ownership validation
- CORS configured for localhost:5173

## Troubleshooting

**Backend won't start:**
```bash
cd backend
dotnet restore
dotnet clean
dotnet build
```

**Frontend won't start:**
```bash
cd frontend
rm -rf node_modules package-lock.json
npm install
```

**CORS errors:**
- Check backend is running on port 5001
- Check frontend is running on port 5173
- Verify CORS policy in `backend/Program.cs`

**Auth errors:**
- Check JWT settings in `appsettings.json`
- Verify token is being sent in Authorization header
- Check token hasn't expired (24hr default)

## Development vs Production

**Development (Current Setup):**
- HTTP only (HTTPS disabled)
- CORS allows localhost:5173
- JWT key in appsettings.json
- File-based JSON storage

**For Production:**
- Enable HTTPS
- Update CORS policy
- Use environment variables for JWT key
- Migrate to proper database (SQL Server, PostgreSQL, etc.)
- Add rate limiting
- Add input sanitization
- Add comprehensive error handling
