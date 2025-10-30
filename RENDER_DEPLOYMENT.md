# Render Deployment Guide (No Docker)

## Prerequisites
- GitHub account connected to Render
- Push your code to the `MiniProjectManager` branch

---

## Step 1: Create Web Service on Render

1. Go to [Render Dashboard](https://dashboard.render.com/)
2. Click **"New +"** → **"Web Service"**
3. Connect GitHub: Authorize Render to access `sankhyayan/Pathlock` repository
4. Select branch: **MiniProjectManager**

---

## Step 2: Configure Web Service

### Basic Configuration
| Setting | Value |
|---------|-------|
| **Name** | `task-manager-api` (or any name you prefer) |
| **Region** | Select closest to your users |
| **Branch** | `MiniProjectManager` |
| **Root Directory** | `Task_Manager` |
| **Runtime** | `.NET` |

### Build & Deploy
| Setting | Command |
|---------|---------|
| **Build Command** | `chmod +x build.sh && ./build.sh` |
| **Start Command** | `chmod +x start.sh && ./start.sh` |

---

## Step 3: Environment Variables

In Render Dashboard → Settings → Environment, add these variables:

```bash
# JWT Configuration (REQUIRED)
Jwt__Key=Generate-A-Random-String-At-Least-32-Characters-Long-ABC123XYZ

Jwt__Issuer=TaskManagerAPI

Jwt__Audience=TaskManagerClient

# ASP.NET Core Configuration
ASPNETCORE_ENVIRONMENT=Production

ASPNETCORE_URLS=http://0.0.0.0:$PORT

# CORS - Allow localhost for testing (update after frontend deployment)
AllowedOrigins__0=http://localhost:5173
```

### After Frontend Deployment:
When you deploy your frontend (e.g., to Vercel), add its URL:
```bash
AllowedOrigins__1=https://your-frontend-app.vercel.app
```

---

## Step 4: Deploy

1. Click **"Create Web Service"**
2. Render will automatically:
   - Clone your repository
   - Run `build.sh` (restore & publish)
   - Run `start.sh` (start the API)
3. Wait for deployment to complete (~2-5 minutes)
4. Your API will be live at: `https://task-manager-api.onrender.com`

---

## Step 5: Test Your Deployment

### Health Check
```bash
curl https://task-manager-api.onrender.com/api/auth/health
```

### Register a User
```bash
curl -X POST https://task-manager-api.onrender.com/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test123!"
  }'
```

### Login
```bash
curl -X POST https://task-manager-api.onrender.com/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test123!"
  }'
```

---

## Step 6: Update Frontend

Update `frontend/src/services/apiService.ts`:

```typescript
// Change from local development URL
const API_BASE_URL = 'http://localhost:5000/api';

// To production URL
const API_BASE_URL = 'https://task-manager-api.onrender.com/api';

// Or use environment variable (recommended)
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';
```

Then create `frontend/.env.production`:
```bash
VITE_API_URL=https://task-manager-api.onrender.com/api
```

---

## Troubleshooting

### Build Fails
- Check Render logs for errors
- Ensure `build.sh` and `start.sh` have correct line endings (LF, not CRLF)
- Run locally: `dotnet publish -c Release -o out`

### App Crashes on Start
- Check environment variables are set correctly
- Look at Render logs for error messages
- Ensure `$PORT` is being used in start command

### CORS Errors
- Add your frontend URL to `AllowedOrigins__0` environment variable
- Check frontend is using correct backend URL
- Ensure credentials are allowed in CORS policy

### JWT Errors
- Verify `Jwt__Key` is at least 32 characters
- Check `Jwt__Issuer` and `Jwt__Audience` match configuration
- Ensure token is being sent in Authorization header

---

## Important Notes

1. **Free Tier Limitations:**
   - Render free tier spins down after 15 minutes of inactivity
   - First request after spin down takes ~30 seconds
   - Consider upgrading for production use

2. **Data Persistence:**
   - Your app uses file-based storage (JSON files)
   - On Render free tier, these files are **ephemeral** (lost on redeploy)
   - Consider migrating to a database for production (PostgreSQL, MongoDB)

3. **Security:**
   - Generate a strong random `Jwt__Key` for production
   - Never commit secrets to GitHub
   - Use Render's environment variables for all sensitive data

4. **Auto-Deploy:**
   - Render automatically redeploys when you push to `MiniProjectManager` branch
   - You can disable this in Settings → Build & Deploy

---

## Next Steps

1. ✅ Deploy backend to Render
2. ⏳ Test all API endpoints
3. ⏳ Deploy frontend to Vercel/Netlify
4. ⏳ Update CORS settings with frontend URL
5. ⏳ Consider database migration for data persistence
