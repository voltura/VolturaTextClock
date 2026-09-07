# VolturaTextClock
A Windows application that shows time as text

The clock supports Swedish, English, or the Windows display language (English is used when the system language is not Swedish).

Open the clock's options button and choose settings to configure:

- Start with Windows, start minimized in the taskbar, and always on top.
- An optional update check at startup, disabled by default, or a manual check. Updates open the GitHub release page after confirmation; they are not installed automatically.
- Separate active and inactive fonts, sizes, styles and colors, with optional glow and flicker.
- The original, brushed-metal or rust background, a plain background, or a custom PNG/JPEG/bitmap.

Settings persist per user. Custom images are loaded without locking the source file; a missing image falls back to the original background.

The clock draws directly at its current control size and redraws after DPI changes. It does not depend on Internet Explorer rendering or temporary clock-image files. Normal updates occur at minute boundaries; optional flicker runs only while the clock is visible.

## Build and validate

Use Windows and the .NET 10 SDK:

```powershell
dotnet build VolturaTextClock.sln -c Release
dotnet run --project tests/ClockSmoke/ClockSmoke.csproj -c Release
dotnet publish VolturaTextClock/VolturaTextClock.csproj -c Release -r win-x64 --self-contained true -o artifacts/clock
```

The GitHub build workflow runs the build and smoke checks and uploads a portable Windows x64 build. It does not publish a release automatically.

The smoke checks cover all 1,440 minutes in both languages, exact five-minute phrases and rollover, system-language selection, rendering at 100–200% sizes, synthetic Windows DPI-change messages, taskbar/startup behavior, background loading, theme persistence, and update-version comparison. UI tests use an isolated settings file and do not enable startup or automatic update checks.

## Issue coverage

| Issue | Implementation |
|---|---|
| #6 | Custom background picker and embedded templates |
| #7 | Active/inactive font and color settings, glow and optional flicker |
| #8 | English/Swedish resources and persisted System/Swedish/English selector |
| #9 | Windows GitHub build, validation and portable artifact |
| #11 | Taskbar minimizing and restore |
| #12 | Persisted startup-minimized setting applied before showing the clock |
| #13 | Optional startup update check, off by default |
| #16 | Control-size rendering and redraw on per-monitor DPI changes |
