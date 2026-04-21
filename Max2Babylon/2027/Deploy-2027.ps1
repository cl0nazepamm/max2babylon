param(
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",

    [string]$MaxRoot = $env:ADSK_3DSMAX_x64_2027,

    [switch]$IncludePdb,

    [switch]$StartMax
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$sourceDir = Join-Path $scriptDir "bin\$Configuration"

if ([string]::IsNullOrWhiteSpace($MaxRoot)) {
    $MaxRoot = "C:\Program Files\Autodesk\3ds Max 2027"
}

$destDir = Join-Path $MaxRoot "bin\assemblies"
$maxExe = Join-Path $MaxRoot "3dsmax.exe"

if (-not (Test-Path -LiteralPath $sourceDir)) {
    throw "Build output folder not found: $sourceDir`nBuild the 2027 target first."
}

if (-not (Test-Path -LiteralPath $destDir)) {
    throw "3ds Max 2027 assemblies folder not found: $destDir"
}

$runningMax = Get-Process 3dsmax -ErrorAction SilentlyContinue
if ($runningMax) {
    Write-Warning "3ds Max is currently running. Deploying while Max is open may leave the old assembly loaded until restart."
}

$filesToCopy = @(
    "Max2Babylon.dll",
    "Max2Babylon.deps.json",
    "GDImageLibrary.dll",
    "Newtonsoft.Json.dll",
    "TargaImage.dll",
    "TQ.Texture.dll"
)

if ($IncludePdb) {
    $filesToCopy += "Max2Babylon.pdb"
}

foreach ($fileName in $filesToCopy) {
    $sourcePath = Join-Path $sourceDir $fileName
    if (-not (Test-Path -LiteralPath $sourcePath)) {
        throw "Missing build artifact: $sourcePath"
    }
}

Write-Host "Deploying Max2Babylon 2027"
Write-Host "  Source: $sourceDir"
Write-Host "  Target: $destDir"

foreach ($fileName in $filesToCopy) {
    $sourcePath = Join-Path $sourceDir $fileName
    $destPath = Join-Path $destDir $fileName
    Copy-Item -LiteralPath $sourcePath -Destination $destPath -Force
    Write-Host "  Copied: $fileName"
}

if ($StartMax) {
    if (-not (Test-Path -LiteralPath $maxExe)) {
        throw "3ds Max executable not found: $maxExe"
    }

    Start-Process -FilePath $maxExe -ArgumentList "/Language=ENU" -WorkingDirectory $MaxRoot
    Write-Host "Started 3ds Max 2027."
}

Write-Host "Deploy complete."
