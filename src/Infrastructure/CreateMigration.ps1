# Script para crear la migración inicial de Entity Framework Core
# Ejecutar desde la raíz del proyecto Infrastructure

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  Entity Framework Core - Crear Migración Inicial" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
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
Write-Host "Creando migración inicial..." -ForegroundColor Yellow
Write-Host ""

# Crear la migración
try {
    dotnet ef migrations add InitialCreate --verbose
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "==================================================" -ForegroundColor Green
        Write-Host "  ✓ Migración creada exitosamente!" -ForegroundColor Green
        Write-Host "==================================================" -ForegroundColor Green
        Write-Host ""
        Write-Host "Próximos pasos:" -ForegroundColor Cyan
        Write-Host "1. Revisar los archivos de migración en /Migrations" -ForegroundColor White
        Write-Host "2. Aplicar la migración: dotnet ef database update" -ForegroundColor White
        Write-Host "3. Verificar la base de datos creada" -ForegroundColor White
        Write-Host ""
    } else {
        Write-Host ""
        Write-Host "✗ Error al crear la migración" -ForegroundColor Red
        Write-Host "Revisa los mensajes de error anteriores" -ForegroundColor Yellow
    }
} catch {
    Write-Host ""
    Write-Host "✗ Excepción al ejecutar dotnet ef migrations" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
