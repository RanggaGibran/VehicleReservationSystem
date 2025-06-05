# Final Heroku Deployment Script - .NET 8 with Correct Configuration

Write-Host "🚀 Final Heroku Deployment - .NET 8 Configuration" -ForegroundColor Green
Write-Host "========================================================" -ForegroundColor Green

# Check if in correct directory
if (Test-Path "Program.cs") {
    Write-Host "✅ Found Program.cs - in correct directory" -ForegroundColor Green
} else {
    Write-Host "❌ Program.cs not found. Please run from project root." -ForegroundColor Red
    exit 1
}

Write-Host "`n🔧 Updated Configuration:" -ForegroundColor Yellow
Write-Host "- Downgraded to .NET 8.0 (more stable on Heroku)" -ForegroundColor White
Write-Host "- Updated all packages to 8.0.0 versions" -ForegroundColor White
Write-Host "- Fixed Procfile for published output" -ForegroundColor White
Write-Host "- Removed problematic project.json" -ForegroundColor White

# Clean and test build locally first
Write-Host "`n🧹 Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean

Write-Host "📦 Restoring packages..." -ForegroundColor Yellow
dotnet restore

Write-Host "🔨 Building application..." -ForegroundColor Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Local build successful" -ForegroundColor Green
} else {
    Write-Host "❌ Local build failed" -ForegroundColor Red
    exit 1
}

# Test publish locally
Write-Host "`n📋 Testing publish output..." -ForegroundColor Yellow
dotnet publish --configuration Release --output ./temp-publish

if (Test-Path "./temp-publish/VehicleReservationSystem.dll") {
    Write-Host "✅ DLL created successfully in publish output" -ForegroundColor Green
    Remove-Item -Recurse -Force ./temp-publish
} else {
    Write-Host "❌ DLL not found in publish output" -ForegroundColor Red
    exit 1
}

# Show current configuration
Write-Host "`n📄 Current Heroku Configuration:" -ForegroundColor Cyan
Write-Host "Runtime: $(Get-Content runtime.txt)" -ForegroundColor White
Write-Host "Procfile: $(Get-Content Procfile)" -ForegroundColor White

# Git operations
Write-Host "`n📦 Adding changes to git..." -ForegroundColor Yellow
git add .

Write-Host "💾 Committing changes..." -ForegroundColor Yellow
git commit -m "Fix Heroku deployment - Switch to .NET 8 with correct configuration

- Downgraded to .NET 8.0 for better Heroku compatibility
- Updated all packages to 8.0.0 versions  
- Fixed Procfile to use published DLL location
- Removed unnecessary project.json file
- All deployment issues should now be resolved"

# Deploy to Heroku
Write-Host "`n🚀 Deploying to Heroku..." -ForegroundColor Yellow
Write-Host "This will take a few minutes..." -ForegroundColor Cyan
git push heroku main

Write-Host "`n📊 Checking deployment..." -ForegroundColor Yellow
heroku ps
heroku logs --tail --num 20

Write-Host "`n✅ Deployment completed!" -ForegroundColor Green
Write-Host "`n🧪 Test your application:" -ForegroundColor Cyan
Write-Host "- Health: https://netdev-a401913bbb39.herokuapp.com/health" -ForegroundColor White
Write-Host "- Login: https://netdev-a401913bbb39.herokuapp.com/Account/Login" -ForegroundColor White

Write-Host "`n🎉 The .NET 8 configuration should resolve all deployment issues!" -ForegroundColor Green
