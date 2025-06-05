# Test script for local deployment verification

Write-Host "=== Vehicle Reservation System - Local Test ===" -ForegroundColor Green

# Set environment variables for testing
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:PORT = "5001"

Write-Host "Building application..." -ForegroundColor Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful" -ForegroundColor Green
    
    Write-Host "Starting application on port $($env:PORT)..." -ForegroundColor Yellow
    Write-Host "Test endpoints:" -ForegroundColor Cyan
    Write-Host "- Health: http://localhost:$($env:PORT)/health" -ForegroundColor White
    Write-Host "- Root: http://localhost:$($env:PORT)/" -ForegroundColor White
    Write-Host "- Login: http://localhost:$($env:PORT)/Account/Login" -ForegroundColor White
    Write-Host ""
    Write-Host "Press Ctrl+C to stop" -ForegroundColor Yellow
    
    dotnet run --configuration Release --no-build
} else {
    Write-Host "❌ Build failed" -ForegroundColor Red
    exit 1
}
