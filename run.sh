#!/bin/bash

# Script para ejecutar InfraGestion con Swagger
# Uso: ./run.sh [--https]

PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
WEB_API_DIR="$PROJECT_DIR/src/Web.API"

# Colores para output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}       InfraGestion - API Server        ${NC}"
echo -e "${BLUE}========================================${NC}"

# Verificar si dotnet está instalado
if ! command -v dotnet &> /dev/null; then
    echo -e "${YELLOW}Error: dotnet SDK no está instalado${NC}"
    exit 1
fi

# Determinar si usar HTTPS
USE_HTTPS=false
if [[ "$1" == "--https" ]]; then
    USE_HTTPS=true
fi

# Restaurar dependencias
echo -e "${YELLOW}Restaurando dependencias...${NC}"
dotnet restore "$PROJECT_DIR/InfraGestion.sln"

# Compilar el proyecto
echo -e "${YELLOW}Compilando el proyecto...${NC}"
dotnet build "$PROJECT_DIR/InfraGestion.sln" --no-restore

if [ $? -ne 0 ]; then
    echo -e "${YELLOW}Error en la compilación${NC}"
    exit 1
fi

# URLs según el modo
if [ "$USE_HTTPS" = true ]; then
    APP_URL="https://localhost:7257"
    echo -e "${GREEN}Iniciando servidor en modo HTTPS...${NC}"
else
    APP_URL="http://localhost:5147"
    echo -e "${GREEN}Iniciando servidor en modo HTTP...${NC}"
fi

SWAGGER_URL="$APP_URL/swagger"

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}API URL:     $APP_URL${NC}"
echo -e "${GREEN}Swagger UI:  $SWAGGER_URL${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Función para esperar a que el servidor esté listo y abrir el navegador
wait_and_open_browser() {
    local url="$1"
    local max_attempts=30
    local attempt=0
    
    echo -e "${YELLOW}Esperando a que el servidor esté listo...${NC}"
    
    while [ $attempt -lt $max_attempts ]; do
        if curl -s -o /dev/null -w "%{http_code}" "$url" 2>/dev/null | grep -q "200\|301\|302"; then
            echo -e "${GREEN}Servidor listo! Abriendo navegador...${NC}"
            xdg-open "$url" 2>/dev/null || firefox "$url" 2>/dev/null || google-chrome "$url" 2>/dev/null &
            return 0
        fi
        sleep 1
        attempt=$((attempt + 1))
    done
    
    echo -e "${YELLOW}No se pudo verificar el servidor, intenta abrir manualmente: $url${NC}"
}

# Ejecutar la espera en segundo plano
(wait_and_open_browser "$SWAGGER_URL") &

# Ejecutar el proyecto
cd "$WEB_API_DIR"
if [ "$USE_HTTPS" = true ]; then
    dotnet run --launch-profile https
else
    dotnet run --launch-profile http
fi
