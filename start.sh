#!/usr/bin/env bash
# Start script for Render

cd backend/out
dotnet TaskManager.API.dll --urls "http://0.0.0.0:$PORT"
