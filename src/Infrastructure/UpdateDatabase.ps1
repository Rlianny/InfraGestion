# Script para actualizar la base de datos con Entity Framework Core
# Ejecutar desde la raíz del proyecto Infrastructure

Write-Host "===================================================" -ForegroundColor Cyan
Write-Host "  Entity Framework Core - Actualizar Base de Datos" -ForegroundColor Cyan
Write-Host "===================================================" -ForegroundColor Cyan
Write-Host ""

# Navegar al directorio Infrastructure
$infrastructurePath = "c:\Users\JL\Desktop\3er Año\IS\Proyecto\Prject Mirror\InfraGestion\src\Infrastructure"

if (Test-Path $infrastructurePath) {
    Set-Location $infrastructurePath
    Write-Host "✓ Directorio Infrastructure encontrado" -ForegroundColor Green
} else {
    Write-Host "✗ Error: No se encontró el directorio Infrastructure" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Actualizando base de datos..." -ForegroundColor Yellow
Write-Host ""

# Actualizar la base de datos
try {
    dotnet ef database update --verbose
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "===================================================" -ForegroundColor Green
        Write-Host "  ✓ Base de datos actualizada exitosamente!" -ForegroundColor Green
        Write-Host "===================================================" -ForegroundColor Green
        Write-Host ""
        Write-Host "La base de datos 'InfraGestionDb' está lista para usarse" -ForegroundColor White
        Write-Host ""
        Write-Host "Conexión utilizada:" -ForegroundColor Cyan
        Write-Host "Server=(localdb)\mssqllocaldb;Database=InfraGestionDb" -ForegroundColor White
        Write-Host ""
    } else {
        Write-Host ""
        Write-Host "✗ Error al actualizar la base de datos" -ForegroundColor Red
        Write-Host "Revisa los mensajes de error anteriores" -ForegroundColor Yellow
    }
} catch {
    Write-Host ""
    Write-Host "✗ Excepción al ejecutar dotnet ef database update" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
