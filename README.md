# MiniProjectManager

A full-stack task management application with smart scheduling capabilities.

## 🚀 Live Demo

**Frontend:** [https://miniprojectmanagerfrontend.netlify.app](https://miniprojectmanagerfrontend.netlify.app)

**Backend API:** [https://miniprojectmanagerbackend.onrender.com](https://miniprojectmanagerbackend.onrender.com)

---

## ✨ Features

- **User Authentication**: Secure JWT-based registration and login
- **Project Management**: Create, view, and delete projects
- **Task Management**: Add tasks with dependencies, due dates, and estimated hours
- **Smart Scheduling**: Automatic task ordering based on dependencies and priorities
- **Real-time Updates**: Instant UI updates for all CRUD operations
- **Responsive Design**: Works seamlessly on desktop and mobile devices

---

## 🛠️ Tech Stack

### Frontend
- **React 18** with TypeScript
- **Vite** for fast build and development
- **React Router** for navigation
- **Modular Architecture**: Components, services, and utilities separated

### Backend
- **.NET 9** (ASP.NET Core)
- **Clean Architecture**: Controllers, Application, Core, Infrastructure layers
- **JWT Authentication** with BCrypt password hashing
- **File-based JSON storage** (users, projects, tasks)
- **Swagger Documentation** for API endpoints

---

## 📦 Project Structure

```
Task_Manager/
├── frontend/               # React + TypeScript frontend
│   ├── src/
│   │   ├── components/    # React components
│   │   ├── contexts/      # Auth context
│   │   ├── services/      # API service layer
│   │   ├── utils/         # Utility functions
│   │   └── types.ts       # TypeScript interfaces
│   └── ...
├── backend/               # .NET 9 backend
│   ├── Controllers/       # API controllers
│   ├── Application/       # DTOs, Services, Mappers
│   ├── Core/             # Domain entities
│   ├── Infrastructure/    # Data persistence
│   ├── Configuration/     # Service configuration
│   └── Middleware/        # Exception handling
└── ...
```

---

## 🚀 Deployment

### Frontend (Netlify)
- **Platform**: Netlify
- **Branch**: `MiniProjectManager`
- **Build Command**: `npm run build`
- **Publish Directory**: `frontend/dist`

### Backend (Render)
- **Platform**: Render
- **Runtime**: Shell (with .NET 9 SDK installed)
- **Build Command**: `bash build.sh`
- **Start Command**: `bash start.sh`

---

## 🔧 Local Development

### Prerequisites
- Node.js 18+
- .NET 9 SDK
- Git

### Frontend Setup
```bash
cd frontend
npm install
npm run dev
```

### Backend Setup
```bash
cd backend
dotnet restore
dotnet run
```

The frontend will run on `http://localhost:5173` and backend on `http://localhost:5000`.

---

## 📝 API Documentation

Once the backend is running, visit:
- **Swagger UI**: `http://localhost:5000/swagger`

### Main Endpoints
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `GET /api/projects` - Get all projects
- `POST /api/projects` - Create project
- `GET /api/projects/{id}/tasks` - Get project tasks
- `POST /api/projects/{id}/tasks` - Create task
- `POST /api/projects/{id}/schedule` - Generate smart schedule

---

## 🌟 Key Features Explained

### Smart Scheduling
The application analyzes task dependencies, due dates, and estimated hours to generate an optimal execution order. Tasks with dependencies are automatically ordered so prerequisites are completed first.

### Modular Architecture
- **Backend**: Clean Architecture with separation of concerns (Controllers → Application → Core → Infrastructure)
- **Frontend**: Component-based with centralized API service layer and reusable utilities

### Security
- JWT-based authentication
- BCrypt password hashing
- CORS protection
- Environment variable configuration

---

## 📄 License

This project is part of an academic assignment.

---

## 👤 Author

**Sankhyayan Chaudhuri**

- GitHub: [@sankhyayan](https://github.com/sankhyayan)
- Repository: [Pathlock](https://github.com/sankhyayan/Pathlock)

---

## 🙏 Acknowledgments

Built as part of the Mini Project Manager assignment, demonstrating full-stack development skills with modern technologies and clean architecture principles.
