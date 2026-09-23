@echo off
cd /d "%~dp0"

echo ========================================
echo   INICIANDO DEPRECIACION APP
echo ========================================
echo.

echo [1/6] Iniciando SQL Server...
docker start depreciacion-sqlserver

echo.
echo [2/6] Iniciando Auth API...
start "Auth API" cmd /k dotnet run --project "backend-net\1. Auth\Auth.API\Auth.API.csproj" --launch-profile https

echo.
echo [3/6] Iniciando Depreciacion API...
start "Depreciacion API" cmd /k dotnet run --project "backend-net\2. Depreciacion\Depreciacion.API\Depreciacion.API.csproj" --launch-profile https

echo.
echo [4/6] Iniciando PDF API...
start "PDF API" cmd /k dotnet run --project "backend-net\3. PDF\Pdf.API\Pdf.API.csproj" --launch-profile https

echo.
echo [5/6] Iniciando API Gateway...
start "API Gateway" cmd /k dotnet run --project "backend-net\0. Gateway\ApiGateway\ApiGateway.csproj" --launch-profile https

echo.
echo [6/6] Iniciando Frontend React...
start "Frontend React" cmd /k "cd /d ""%~dp0frontend-react"" && if not exist node_modules call npm install && npm run dev"

echo.
echo ========================================
echo   SISTEMA INICIADO
echo ========================================
echo.
echo Frontend:      http://localhost:5173
echo Gateway:       https://localhost:7129
echo Auth:          https://localhost:7117
echo Depreciacion:  https://localhost:7292
echo PDF:           https://localhost:7126
echo.
echo Puedes cerrar esta ventana.
echo Las APIs continuaran ejecutandose.
echo ========================================


echo ========================================

echo Esperando que los servicios terminen de iniciar...
timeout /t 8 /nobreak >nul

echo Abriendo Depreciacion App...
start "" http://localhost:5173

pause