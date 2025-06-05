#!/bin/bash
# Build and publish for Heroku deployment

echo "Starting build process..."

# Restore packages
dotnet restore

# Clean previous builds
dotnet clean

# Build the application
dotnet build --configuration Release

# Publish the application
dotnet publish --configuration Release --output ./bin/Release/publish

echo "Build process completed."
echo "Files in publish directory:"
ls -la ./bin/Release/publish/

# Verify DLL exists
if [ -f "./bin/Release/publish/VehicleReservationSystem.dll" ]; then
    echo "✅ VehicleReservationSystem.dll found"
else
    echo "❌ VehicleReservationSystem.dll NOT found"
    exit 1
fi
