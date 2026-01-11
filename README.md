# Overview

UUIDBR is a Windows context menu tool that bulk renames files and directories to UUIDs.

## Modes

- **Random**: Each item gets a unique UUID
- **Sequential**: All items share one UUID with incrementing numbers

## Installation

1. Download "UUIDBR.7z" from the [releases](../../releases/latest) page
1. Place `UUIDBR.exe` in `%LOCALAPPDATA%\Programs\UUIDBR\`
1. In `UUIDBR.reg`, replace `<USERNAME>` with your actual username before merging

## Building

```powershell
dotnet publish -c Release
```

Output: `bin\Release\net8.0\win-x64\publish\UUIDBR.exe`