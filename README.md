# Task Manager - Full Stack Application

A full-stack task management application built with React (TypeScript) frontend and .NET 8 backend.

## Features

- ✅ Create, read, update, and delete tasks
- ✅ Toggle task completion status
- ✅ Filter tasks (All / Active / Completed)
- ✅ Automatic sorting (uncompleted tasks at top)
- ✅ Persistent file-based storage
- ✅ RESTful API with Swagger documentation
- ✅ Debug logging with emoji indicators

## Tech Stack

### Frontend
- **Framework**: React 18.2.0
- **Language**: TypeScript 5.0.4
- **Build Tool**: Vite 5.0.0
- **Styling**: CSS
- **State Management**: React Hooks (useState, useEffect, useMemo)

### Backend
- **Framework**: ASP.NET Core Web API (.NET 8)
- **Language**: C#
- **Storage**: JSON file-based persistence
- **API Documentation**: Swagger/OpenAPI

## Project Structure

```
Task_Manager/
├── frontend/
│   ├── src/
│   │   ├── api/           # API client
│   │   ├── components/    # React components
│   │   ├── constants/     # Constants
│   │   ├── hooks/         # Custom React hooks
│   │   ├── types/         # TypeScript types
│   │   ├── App.tsx
│   │   └── main.tsx
│   └── package.json
│
└── backend/
    └── TaskManager.API/
        ├── Controllers/   # API endpoints
        ├── Models/        # Data models
        ├── Services/      # Business logic
        └── Program.cs     # App configuration
```

## Prerequisites

### Frontend
- Node.js (v16 or higher)
- npm or yarn

### Backend
- .NET 8 SDK or higher

## Installation & Setup

### 1. Clone the Repository

```bash
cd "D:\PEC Material\Pathloack\Task_Manager"
```

### 2. Backend Setup

```powershell
# Navigate to backend directory
cd backend\TaskManager.API

# Restore dependencies
dotnet restore

# Build the project
dotnet build
```

### 3. Frontend Setup

```powershell
# Navigate to frontend directory (from root)
cd ..\..\frontend

# Install dependencies
npm install
```

## Running the Application

### Option 1: Run Both Servers Separately

#### Terminal 1 - Backend Server

```powershell
cd backend\TaskManager.API
dotnet run
```

The backend API will start at: **http://localhost:5000**
- Swagger UI: http://localhost:5000/swagger

#### Terminal 2 - Frontend Server

```powershell
cd frontend
npm run dev
```

The frontend will start at: **http://localhost:5173**

### Option 2: Quick Start (Two Commands)

Open two PowerShell terminals:

**Terminal 1:**
```powershell
cd "D:\PEC Material\Pathloack\Task_Manager\backend\TaskManager.API" ; dotnet run
```

**Terminal 2:**
```powershell
cd "D:\PEC Material\Pathloack\Task_Manager\frontend" ; npm run dev
```

## Using the Application

1. Open your browser and go to **http://localhost:5173**
2. Add tasks using the input field
3. Toggle task completion by clicking the checkbox
4. Delete tasks using the delete button
5. Filter tasks using the All/Active/Completed buttons
6. Tasks are automatically sorted (uncompleted first)

## API Endpoints

All endpoints are available at `http://localhost:5000/api/tasks`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks` | Get all tasks |
| POST | `/api/tasks` | Create a new task |
| PUT | `/api/tasks/{id}` | Update a task |
| DELETE | `/api/tasks/{id}` | Delete a task |

## Data Persistence

- Tasks are stored in `backend/TaskManager.API/tasks.json`
- Data persists even after stopping the backend server
- The file is automatically created on first use

## Development Features

### Debug Logging

The backend includes comprehensive logging with emoji indicators:
- 🔍 GET requests
- ➕ POST requests
- 📝 PUT requests
- 🗑️ DELETE requests
- ✅ Successful operations
- ❌ Failed operations
- 💾 Storage operations

### Hot Reload

- **Frontend**: Vite provides instant hot module replacement (HMR)
- **Backend**: Use `dotnet watch run` for automatic reload on code changes

```powershell
# Backend with hot reload
cd backend\TaskManager.API
dotnet watch run
```

## Building for Production

### Frontend

```powershell
cd frontend
npm run build
```

Output will be in `frontend/dist/`

### Backend

```powershell
cd backend\TaskManager.API
dotnet publish -c Release -o ./publish
```

Output will be in `backend/TaskManager.API/publish/`

## Troubleshooting

### Port Already in Use

**Backend (Port 5000):**
```powershell
# Find process using port 5000
netstat -ano | findstr :5000

# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F
```

**Frontend (Port 5173):**
```powershell
# Find process using port 5173
netstat -ano | findstr :5173

# Kill the process
taskkill /PID <PID> /F
```

### CORS Issues

If you encounter CORS errors:
1. Ensure backend is running on `http://localhost:5000`
2. Ensure frontend is running on `http://localhost:5173`
3. Check `Program.cs` for correct CORS configuration

### Tasks Not Persisting

- Check that `tasks.json` file is created in `backend/TaskManager.API/`
- Check console logs for any file I/O errors
- Ensure the application has write permissions to the directory

## API Testing

You can test the API using:
- **Swagger UI**: http://localhost:5000/swagger
- **Postman**: Import the endpoints
- **curl**: Command-line testing
- **TaskManager.API.http**: Use VS Code REST Client extension

## License

This project is created for educational purposes.

## Author

Created as part of PEC Material coursework.
