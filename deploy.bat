@echo off
setlocal

set ROOT=%~dp0
set CONFIG=Release

if /I "%~1"=="Debug" set CONFIG=Debug
if /I "%~1"=="Release" set CONFIG=Release

pwsh -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Max2Babylon\2027\Deploy-2027.ps1" -Configuration %CONFIG%
exit /b %ERRORLEVEL%
