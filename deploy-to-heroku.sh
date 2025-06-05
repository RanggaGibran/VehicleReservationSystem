#!/bin/bash
# Deploy to Heroku - Vehicle Reservation System
# Deployment script to fix HTTP 500 error

echo "🚀 Deploying Vehicle Reservation System to Heroku"
echo "==================================================="

# Step 1: Check if we're in the right directory
if [ -f "Program.cs" ]; then
    echo "✅ Found Program.cs - in correct directory"
else
    echo "❌ Program.cs not found. Please run from project root directory."
    exit 1
fi

# Step 2: Verify critical files exist
critical_files=("Program.cs" "Procfile" "global.json" "runtime.txt")
for file in "${critical_files[@]}"; do
    if [ -f "$file" ]; then
        echo "✅ $file exists"
    else
        echo "❌ $file missing"
        exit 1
    fi
done

# Step 3: Display the fixes that will be deployed
echo ""
echo "🔧 Fixes included in this deployment:"
echo "- Port binding to Heroku's dynamic PORT"
echo "- Removed HTTPS redirection in production"
echo "- Fixed database initialization (EnsureCreated vs Migrate)"
echo "- Enhanced error handling and logging"
echo "- Added health check endpoints (/health and /)"
echo "- Fixed null reference in AccountController"
echo "- Optimized Procfile for .NET deployment"

# Step 4: Show git status
echo ""
echo "📋 Git Status:"
git status

# Step 5: Add all changes
echo ""
echo "📦 Adding changes to git..."
git add .

# Step 6: Commit changes
echo "💾 Committing changes..."
git commit -m "FINAL FIX: Resolve HTTP 500 deployment issues

- Added Heroku PORT environment variable handling
- Removed HTTPS redirection in production (Heroku handles SSL)
- Fixed database initialization with EnsureCreated() instead of Migrate()
- Enhanced logging and error handling for production
- Added health monitoring endpoints (/health, /)
- Fixed null reference warnings in AccountController
- Updated Procfile with correct .NET command
- All deployment issues should now be resolved"

# Step 7: Deploy to Heroku
echo ""
echo "🚀 Deploying to Heroku..."
echo "This may take a few minutes..."
git push heroku main

# Step 8: Check deployment status
echo ""
echo "📊 Checking deployment status..."
heroku ps

# Step 9: Show logs
echo ""
echo "📋 Recent logs (last 50 lines):"
heroku logs --tail --num 50

echo ""
echo "✅ Deployment commands completed!"
echo ""
echo "🧪 Test your application:"
echo "- Health Check: https://netdev-a401913bbb39.herokuapp.com/health"
echo "- Root Status: https://netdev-a401913bbb39.herokuapp.com/"
echo "- Login Page: https://netdev-a401913bbb39.herokuapp.com/Account/Login"

echo ""
echo "🔍 If issues persist:"
echo "- Run: 'heroku logs --tail' to see live logs"
echo "- Run: 'heroku restart' to restart the application"
echo "- Check the health endpoint first to verify basic functionality"

echo ""
echo "🎉 The HTTP 500 error should now be resolved!"
