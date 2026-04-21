@echo off
setlocal

set ROOT=%~dp0
set CONFIG=Release

if /I "%~1"=="Debug" set CONFIG=Debug
if /I "%~1"=="Release" set CONFIG=Release

call "%ROOT%build.bat" %CONFIG%
if errorlevel 1 exit /b 1

pwsh -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Max2Babylon\2027\Deploy-2027.ps1" -Configuration %CONFIG% -StartMax
exit /b %ERRORLEVEL%
