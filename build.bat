@echo off
setlocal

set ROOT=%~dp0
set PROJECT=%ROOT%Max2Babylon\2027\Max2Babylon2027.csproj
set CONFIG=Release
set DOTNET_CLI_HOME=%ROOT%.dotnet_cli
set DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
set DOTNET_CLI_TELEMETRY_OPTOUT=1
set DOTNET_NOLOGO=1

if /I "%~1"=="Debug" set CONFIG=Debug
if /I "%~1"=="Release" set CONFIG=Release

echo [1/1] Building Max2Babylon 2027 (%CONFIG%)...
if not exist "%DOTNET_CLI_HOME%" mkdir "%DOTNET_CLI_HOME%"
dotnet build "%PROJECT%" -c %CONFIG% -p:Platform=x64 /clp:ErrorsOnly
if errorlevel 1 goto :fail

echo.
echo Build output:
echo   %ROOT%Max2Babylon\2027\bin\%CONFIG%\Max2Babylon.dll
exit /b 0

:fail
echo.
echo Build failed.
exit /b 1
