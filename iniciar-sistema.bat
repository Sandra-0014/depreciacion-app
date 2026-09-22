@echo off
cd /d "%~dp0"

echo Iniciando SQL Server...
docker start depreciacion-sqlserver

echo Iniciando Auth...
start "Auth API" cmd /k dotnet run --project "backend-net\1. Auth\Auth.API\Auth.API.csproj" --launch-profile http

echo Iniciando Depreciacion...
start "Depreciacion API" cmd /k dotnet run --project "backend-net\2. Depreciacion\Depreciacion.API\Depreciacion.API.csproj" --launch-profile http

echo Iniciando Gateway...
start "API Gateway" cmd /k dotnet run --project "backend-net\0. Gateway\ApiGateway\ApiGateway.csproj" --launch-profile http

echo Iniciando Frontend...
start "Frontend React" cmd /k "cd /d "%~dp0frontend-react" && if not exist node_modules call npm install && npm run dev"

echo.
echo Sistema iniciado.
echo Frontend: http://localhost:5173
echo Gateway:  http://localhost:5085
echo Auth:     http://localhost:5001
echo Depreciacion: http://localhost:5013
pause