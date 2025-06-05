# Deploy to Heroku - Vehicle Reservation System
# Deployment script to fix HTTP 500 error

Write-Host "🚀 Deploying Vehicle Reservation System to Heroku" -ForegroundColor Green
Write-Host "===================================================" -ForegroundColor Green

# Step 1: Check if we're in the right directory
if (Test-Path "Program.cs") {
    Write-Host "✅ Found Program.cs - in correct directory" -ForegroundColor Green
} else {
    Write-Host "❌ Program.cs not found. Please run from project root directory." -ForegroundColor Red
    exit 1
}

# Step 2: Verify critical files exist
$criticalFiles = @("Program.cs", "Procfile", "global.json", "runtime.txt")
foreach ($file in $criticalFiles) {
    if (Test-Path $file) {
        Write-Host "✅ $file exists" -ForegroundColor Green
    } else {
        Write-Host "❌ $file missing" -ForegroundColor Red
        exit 1
    }
}

# Step 3: Display the fixes that will be deployed
Write-Host "`n🔧 Fixes included in this deployment:" -ForegroundColor Yellow
Write-Host "- Port binding to Heroku's dynamic PORT" -ForegroundColor White
Write-Host "- Removed HTTPS redirection in production" -ForegroundColor White
Write-Host "- Fixed database initialization (EnsureCreated vs Migrate)" -ForegroundColor White
Write-Host "- Enhanced error handling and logging" -ForegroundColor White
Write-Host "- Added health check endpoints (/health and /)" -ForegroundColor White
Write-Host "- Fixed null reference in AccountController" -ForegroundColor White
Write-Host "- Optimized Procfile for .NET deployment" -ForegroundColor White

# Step 4: Show git status
Write-Host "`n📋 Git Status:" -ForegroundColor Yellow
git status

# Step 5: Add all changes
Write-Host "`n📦 Adding changes to git..." -ForegroundColor Yellow
git add .

# Step 6: Commit changes
Write-Host "💾 Committing changes..." -ForegroundColor Yellow
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
Write-Host "`n🚀 Deploying to Heroku..." -ForegroundColor Yellow
Write-Host "This may take a few minutes..." -ForegroundColor Cyan
git push heroku main

# Step 8: Check deployment status
Write-Host "`n📊 Checking deployment status..." -ForegroundColor Yellow
heroku ps

# Step 9: Show logs
Write-Host "`n📋 Recent logs (last 50 lines):" -ForegroundColor Yellow
heroku logs --tail --num 50

Write-Host "`n✅ Deployment commands completed!" -ForegroundColor Green
Write-Host "`n🧪 Test your application:" -ForegroundColor Cyan
Write-Host "- Health Check: https://netdev-a401913bbb39.herokuapp.com/health" -ForegroundColor White
Write-Host "- Root Status: https://netdev-a401913bbb39.herokuapp.com/" -ForegroundColor White
Write-Host "- Login Page: https://netdev-a401913bbb39.herokuapp.com/Account/Login" -ForegroundColor White

Write-Host "`n🔍 If issues persist:" -ForegroundColor Yellow
Write-Host "- Run: 'heroku logs --tail' to see live logs" -ForegroundColor White
Write-Host "- Run: 'heroku restart' to restart the application" -ForegroundColor White
Write-Host "- Check the health endpoint first to verify basic functionality" -ForegroundColor White

Write-Host "`n🎉 The HTTP 500 error should now be resolved!" -ForegroundColor Green
