@echo off
setlocal

set ROOT=%~dp0

pwsh -NoProfile -ExecutionPolicy Bypass -Command ^
  "$targets = @(" ^
  "  (Join-Path '%ROOT%' 'Max2Babylon\2027\bin')," ^
  "  (Join-Path '%ROOT%' 'Max2Babylon\2027\obj')," ^
  "  (Join-Path '%ROOT%' '.dotnet_cli')" ^
  ");" ^
  "foreach ($target in $targets) {" ^
  "  if (Test-Path -LiteralPath $target) {" ^
  "    Remove-Item -LiteralPath $target -Recurse -Force;" ^
  "    Write-Host ('Removed: ' + $target)" ^
  "  }" ^
  "}"

exit /b %ERRORLEVEL%
