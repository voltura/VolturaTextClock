# Privacy

TextClock has no accounts, telemetry, advertising, or analytics. Reading the time and changing the clock's appearance work offline.

Settings remain in `%LOCALAPPDATA%\VolturaTextClockAppsettings.json`, preserving the path used by existing releases. A malformed file is copied beside it with a `.corrupt-…` suffix before defaults are restored. Settings include appearance, the chosen background image path, window position, language, and startup/update preferences. The app reads a selected custom image without uploading it or locking its source file.

Diagnostic errors are written locally in `%LOCALAPPDATA%\VolturaTextClock.log`. Verified pending installers and update metadata are cached in `%LOCALAPPDATA%\Voltura\TextClock\Updates`. No logs, settings, or images are uploaded automatically. Uninstall keeps settings by default.

Manual update checks, and optional startup checks when enabled, contact GitHub. Installed copies download release metadata, signed manifests, and selected installers from GitHub infrastructure; those requests disclose the usual IP address and HTTP request metadata to GitHub. Automatic startup checks are off by default. Installation is always user initiated.

The standard installer may download the .NET 10 Desktop Runtime from Microsoft and verifies its Microsoft Authenticode signature before running it. Choose the full offline installer to avoid this runtime download. Links to GitHub or Voltura open in your browser under those services' privacy policies.

The static product website uses no tracking scripts, cookies, or third-party font requests. GitHub Pages and the DNS/hosting providers still process ordinary web-request data according to their own policies.
