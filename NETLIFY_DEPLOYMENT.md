# Netlify Frontend Deployment Guide

## Prerequisites
- Backend deployed on Render (get the URL first)
- GitHub account connected to Netlify

---

## Step 1: Get Your Backend URL from Render

Once your Render backend is deployed, you'll get a URL like:
```
https://task-manager-api.onrender.com
```

Copy this URL (you'll need it for step 4).

---

## Step 2: Deploy to Netlify

1. Go to [Netlify](https://app.netlify.com/)
2. Click **"Add new site"** → **"Import an existing project"**
3. Choose **"Deploy with GitHub"**
4. Authorize Netlify to access your GitHub
5. Select repository: **`sankhyayan/Pathlock`**
6. Select branch: **`MiniProjectManager`**

---

## Step 3: Configure Build Settings

Netlify should auto-detect the settings from `netlify.toml`, but verify:

| Setting | Value |
|---------|-------|
| **Base directory** | `frontend` |
| **Build command** | `npm run build` |
| **Publish directory** | `frontend/dist` |

---

## Step 4: Add Environment Variables

Before deploying, add environment variables:

1. In Netlify, go to **Site settings** → **Environment variables**
2. Click **"Add a variable"**
3. Add this variable:

```
Key: VITE_API_URL
Value: https://task-manager-api.onrender.com/api
```

**Important:** Replace `task-manager-api.onrender.com` with your actual Render backend URL!

---

## Step 5: Deploy

1. Click **"Deploy site"**
2. Wait for build to complete (~2-3 minutes)
3. Your frontend will be live at: `https://random-name-123.netlify.app`

---

## Step 6: Update Backend CORS Settings

Now you need to allow your Netlify frontend URL in your backend.

### In Render (Backend):

1. Go to your Render dashboard
2. Select your backend service
3. Go to **Environment** tab
4. Update the CORS environment variable:

**Add this new variable:**
```
Key: AllowedOrigins__1
Value: https://your-frontend.netlify.app
```

Replace `your-frontend.netlify.app` with your actual Netlify URL.

5. Click **"Save Changes"**
6. Your backend will automatically redeploy with new CORS settings

---

## Step 7: Test Your Deployment

1. Visit your Netlify URL: `https://your-frontend.netlify.app`
2. Try to register a new user
3. Try to login
4. Create a project and tasks

---

## Troubleshooting

### CORS Errors
- **Problem:** "Access-Control-Allow-Origin" error in browser console
- **Solution:** Make sure you added your Netlify URL to `AllowedOrigins__1` in Render backend

### API Connection Failed
- **Problem:** "Failed to fetch" or network errors
- **Solution:** 
  - Check `VITE_API_URL` in Netlify environment variables
  - Ensure backend is running on Render
  - Check backend URL is correct (with `/api` at the end)

### Build Fails
- **Problem:** Build fails in Netlify
- **Solution:**
  - Check build logs for errors
  - Ensure `npm install` works locally
  - Try clearing Netlify cache and rebuilding

### Backend Sleeping (Free Tier)
- **Problem:** First request takes 30+ seconds
- **Solution:** This is normal on Render free tier - backend spins down after 15 minutes of inactivity

---

## Custom Domain (Optional)

### Change Netlify URL:
1. Go to **Site settings** → **Domain management**
2. Click **"Options"** → **"Edit site name"**
3. Change to: `task-manager-yourusername.netlify.app`

### Add Custom Domain:
1. Click **"Add custom domain"**
2. Enter your domain (e.g., `taskmanager.com`)
3. Follow DNS configuration instructions

**Remember:** After changing domain, update `AllowedOrigins__1` in Render backend!

---

## Environment Variables Summary

### Frontend (Netlify):
```
VITE_API_URL = https://task-manager-api.onrender.com/api
```

### Backend (Render):
```
Jwt__Key = deIai5cLjWyXbf9YM2xAh6zRqGUrPQZVmpNH3ECo
Jwt__Issuer = TaskManagerAPI
Jwt__Audience = TaskManagerClient
ASPNETCORE_ENVIRONMENT = Production
AllowedOrigins__0 = http://localhost:5173
AllowedOrigins__1 = https://your-frontend.netlify.app
```

---

## Production Checklist

- ✅ Backend deployed on Render
- ✅ Frontend deployed on Netlify
- ✅ `VITE_API_URL` set in Netlify
- ✅ Netlify URL added to backend CORS
- ✅ Test user registration
- ✅ Test login
- ✅ Test project/task creation
- ✅ Test smart scheduling

---

## Auto-Deploy

Both Netlify and Render are configured for auto-deploy:
- Push to `MiniProjectManager` branch → Both automatically redeploy
- Backend changes trigger Render rebuild
- Frontend changes trigger Netlify rebuild

To disable auto-deploy:
- **Netlify:** Site settings → Build & deploy → Stop builds
- **Render:** Settings → Build & Deploy → Auto-Deploy: Off
