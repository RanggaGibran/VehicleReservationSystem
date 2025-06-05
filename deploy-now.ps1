# Quick Deployment Script - .NET 8 to Heroku

Write-Host "🚀 Deploying Vehicle Reservation System - .NET 8 Configuration" -ForegroundColor Green
Write-Host "================================================================" -ForegroundColor Green

# Check if we're in the right directory
if (!(Test-Path "Program.cs")) {
    Write-Host "❌ Please run this script from the VehicleReservationSystem directory" -ForegroundColor Red
    exit 1
}

# Show current configuration
Write-Host "`n📋 Current Configuration:" -ForegroundColor Cyan
Write-Host "Runtime: $(Get-Content runtime.txt -Raw)" -ForegroundColor White
Write-Host "Procfile: $(Get-Content Procfile -Raw)" -ForegroundColor White

# Git operations
Write-Host "`n📦 Adding changes to git..." -ForegroundColor Yellow
git add .

Write-Host "💾 Committing changes..." -ForegroundColor Yellow
git commit -m "Deploy .NET 8 configuration to fix Heroku deployment issues"

# Deploy to Heroku
Write-Host "`n🚀 Deploying to Heroku..." -ForegroundColor Yellow
Write-Host "This may take several minutes..." -ForegroundColor Cyan
git push heroku main

Write-Host "`n📊 Checking deployment status..." -ForegroundColor Yellow
heroku ps

Write-Host "`n📝 Recent logs:" -ForegroundColor Yellow
heroku logs --tail --num 15

Write-Host "`n✅ Deployment completed!" -ForegroundColor Green
Write-Host "`n🧪 Test your application:" -ForegroundColor Cyan
Write-Host "Health Check: https://netdev-a401913bbb39.herokuapp.com/health" -ForegroundColor White
Write-Host "Login Page: https://netdev-a401913bbb39.herokuapp.com/Account/Login" -ForegroundColor White
Write-Host "Main App: https://netdev-a401913bbb39.herokuapp.com/" -ForegroundColor White

Write-Host "`n🎉 The .NET 8 configuration should resolve all previous deployment issues!" -ForegroundColor Green
