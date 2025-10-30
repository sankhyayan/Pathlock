#!/usr/bin/env bash
# Build script for Render

set -o errexit

cd backend
dotnet restore
dotnet publish -c Release -o out
