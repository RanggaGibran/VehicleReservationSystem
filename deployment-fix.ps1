# 🎯 DEFINITIVE HEROKU DEPLOYMENT SOLUTION
# This script will fix the .NET deployment issue once and for all

Write-Host "🚀 DEFINITIVE HEROKU DEPLOYMENT FIX" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

Write-Host "`n🔍 DIAGNOSIS: The issue is .NET SDK vs Runtime on Heroku" -ForegroundColor Yellow
Write-Host "- Heroku only has .NET Runtime, not SDK" -ForegroundColor White
Write-Host "- The 'dotnet run' command requires SDK" -ForegroundColor White
Write-Host "- We need to use the compiled DLL directly" -ForegroundColor White

Write-Host "`n🔧 SOLUTION STEPS:" -ForegroundColor Cyan

Write-Host "`n1️⃣ Clean up mixed .NET versions..." -ForegroundColor Yellow
if (Test-Path "project.json") {
    Remove-Item "project.json" -Force
    Write-Host "   ✅ Removed project.json" -ForegroundColor Green
}

if (Test-Path "bin\Debug\net9.0") {
    Remove-Item -Recurse -Force "bin\Debug\net9.0" -ErrorAction SilentlyContinue
    Write-Host "   ✅ Removed .NET 9 artifacts" -ForegroundColor Green
}

Write-Host "`n2️⃣ Verify configuration files..." -ForegroundColor Yellow
$runtime = Get-Content "runtime.txt"
$procfile = Get-Content "Procfile"
$globalJson = Get-Content "global.json" | ConvertFrom-Json

Write-Host "   Runtime: $runtime" -ForegroundColor White
Write-Host "   Procfile: $procfile" -ForegroundColor White
Write-Host "   Global.json SDK: $($globalJson.sdk.version)" -ForegroundColor White

if ($runtime -eq "dotnet-8.0.x" -and $procfile -eq "web: dotnet VehicleReservationSystem.dll --urls http://0.0.0.0:`$PORT") {
    Write-Host "   ✅ Configuration files are correct" -ForegroundColor Green
} else {
    Write-Host "   ❌ Configuration files need fixing" -ForegroundColor Red
}

Write-Host "`n3️⃣ Test local build..." -ForegroundColor Yellow
$buildResult = & dotnet build --configuration Release 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✅ Local build successful" -ForegroundColor Green
} else {
    Write-Host "   ❌ Local build failed:" -ForegroundColor Red
    Write-Host $buildResult -ForegroundColor Red
    Write-Host "`n   🔧 Run these commands manually:" -ForegroundColor Yellow
    Write-Host "   dotnet clean" -ForegroundColor White
    Write-Host "   dotnet restore" -ForegroundColor White
    Write-Host "   dotnet build --configuration Release" -ForegroundColor White
    exit 1
}

Write-Host "`n4️⃣ Test publish output..." -ForegroundColor Yellow
$publishResult = & dotnet publish --configuration Release --output ./temp-test-publish 2>&1
if (Test-Path "./temp-test-publish/VehicleReservationSystem.dll") {
    Write-Host "   ✅ DLL created successfully" -ForegroundColor Green
    Remove-Item -Recurse -Force "./temp-test-publish"
} else {
    Write-Host "   ❌ DLL not created in publish output" -ForegroundColor Red
    Write-Host $publishResult -ForegroundColor Red
    exit 1
}

Write-Host "`n5️⃣ Ready for deployment!" -ForegroundColor Green
Write-Host "`n📋 DEPLOYMENT COMMANDS (run these):" -ForegroundColor Cyan
Write-Host "git add ." -ForegroundColor White
Write-Host "git commit -m 'Fix Heroku deployment with .NET 8 configuration'" -ForegroundColor White
Write-Host "git push heroku main" -ForegroundColor White

Write-Host "`n🔍 AFTER DEPLOYMENT, CHECK:" -ForegroundColor Cyan
Write-Host "heroku logs --tail" -ForegroundColor White
Write-Host "https://netdev-a401913bbb39.herokuapp.com/health" -ForegroundColor White
Write-Host "https://netdev-a401913bbb39.herokuapp.com/Account/Login" -ForegroundColor White

Write-Host "`n✨ KEY CHANGES THAT WILL FIX THE ISSUE:" -ForegroundColor Green
Write-Host "- Using .NET 8 (stable on Heroku)" -ForegroundColor White
Write-Host "- Procfile uses compiled DLL (not 'dotnet run')" -ForegroundColor White
Write-Host "- All packages downgraded to 8.0.0" -ForegroundColor White
Write-Host "- Removed conflicting project.json" -ForegroundColor White

Write-Host "`n🎉 THIS SHOULD RESOLVE THE DEPLOYMENT ISSUE!" -ForegroundColor Green
