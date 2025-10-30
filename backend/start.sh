#!/usr/bin/env bash
# Start script for Render

# Ensure .NET is in PATH
export PATH="/opt/render/.dotnet:$PATH"
export DOTNET_ROOT="/opt/render/.dotnet"

cd out
dotnet TaskManager.API.dll --urls "http://0.0.0.0:$PORT"
