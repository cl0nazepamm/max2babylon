# Max2Babylon 2027

Small, 3ds Max 2027-only fork of the Babylon.js exporter.

This repo is trimmed for one job:

- build `Max2Babylon.dll` for 3ds Max 2027
- use the installed Autodesk SDK on your machine
- deploy the plugin without legacy multi-year project baggage

## Requirements

- .NET SDK 10
- 3ds Max 2027 installed at `C:\Program Files\Autodesk\3ds Max 2027`
  or `ADSK_3DSMAX_x64_2027` set to your Max install path

## Build

```bat
build.bat
```

Debug build:

```bat
build.bat Debug
```

## Deploy

```bat
deploy.bat
```

Debug deploy:

```bat
deploy.bat Debug
```

## Build + Deploy + Launch Max

```bat
dev.bat
```

## Layout

```text
Max2Babylon.sln                single-project solution
build.bat                      build Max2Babylon 2027
deploy.bat                     deploy build output into 3ds Max 2027
dev.bat                        build + deploy + launch Max
clean.bat                      remove local build artifacts
Max2Babylon/                   exporter source
Refs/                          non-Autodesk runtime DLLs
SharedProjects/                local shared exporter code
```

## Notes

- Autodesk assemblies are not vendored here. The project resolves them from your
  local 3ds Max 2027 install.
- Third-party runtime DLLs that the exporter still needs are kept in `Refs/`.
- This fork is intentionally private-fork friendly, not upstream-shaped.
