#!/bin/bash
# Test script for local deployment verification

echo "=== Vehicle Reservation System - Local Test ==="

# Set environment variables for testing
export ASPNETCORE_ENVIRONMENT=Production
export PORT=5001

echo "Building application..."
dotnet build --configuration Release

if [ $? -eq 0 ]; then
    echo "✅ Build successful"
    
    echo "Starting application on port $PORT..."
    echo "Test endpoints:"
    echo "- Health: http://localhost:$PORT/health"  
    echo "- Root: http://localhost:$PORT/"
    echo "- Login: http://localhost:$PORT/Account/Login"
    echo ""
    echo "Press Ctrl+C to stop"
    
    dotnet run --configuration Release --no-build
else
    echo "❌ Build failed"
    exit 1
fi
