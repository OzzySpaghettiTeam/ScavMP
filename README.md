# ScavMP

ScavMP is a lightweight multiplayer mod which utilizes LiteEntitySystem.

## Compilation guide

Requirements:
- .NET SDK 6.0 or later (required to build netstandard2.1 libraries)
- (Optional) Unity if you intend to use the compiled DLL as a plugin in a Unity project

### Build (dotnet CLI):

Set `vars.targets`'s BaseGamePath variable to your game's path

```bash
cd ScavMP
dotnet restore
dotnet build
```

In `Debug` it compiles to `BepInEx/scripts` to support hot reloading with F6.
In `Release` it compiles to `BepInEx/plugins` like normal.

## Goals

- ✅ Get to compile
- ☑️ Get to connect
- ☑️ UI with UXML and USS
- ☑️ Sync World
- ☑️ Sync Players
- ☑️ Sync everything else

## Features

- Live reloading
- Heavy utiliztion of LiteEntitySystem to skip packet(or rather ease) packet handling.

## Development notes

- Live Reload code with F6
