# Security policy

The latest stable release is supported. Older versions should be upgraded before reporting a problem. The v1.1.0 revival replaces the obsolete Codacy baseline with current CodeQL and dependency monitoring.

Report suspected vulnerabilities privately through GitHub's **Report a vulnerability** action in this repository's Security tab. Include the affected version, configuration, steps, and expected impact. If private reporting is unavailable, contact Voltura through https://voltura.se/ without posting exploit details publicly.

## Supported surface and trust boundaries

TextClock is a per-user Windows desktop app. It runs without administrator privileges. Clock themes and settings are local user data. It is not a service, server, HTML renderer, or browser extension.

- Network update metadata and downloads are untrusted until verified. Only HTTPS GitHub release origins are accepted, signed manifests must match the embedded TextClock public key and release version, and installer size/hash must match before execution. Verification must also run immediately before installation.
- Update checks are optional and off by default. Download and installation require user action. Portable copies use manual downloads.
- Installer maintenance must remain within the application-owned installation and staging directories, reject linked paths and escaping payload names, preserve the previous installation on a failed upgrade, and preserve user settings by default.
- App startup registration belongs only to the current user. Uninstall must remove only an entry referring to the installed TextClock executable.
- Local image files are decoded as images, never executed. Broken configuration must not silently erase the only copy of a user's settings.

Report signature bypasses, arbitrary file writes/deletions, installation escape, unwanted privilege elevation, or comparable violations. Coding-style preferences and 2020 Codacy metadata findings are tracked separately from vulnerabilities. A same-user attacker who can replace the running application or embedded key is outside the updater's integrity guarantee; this does not excuse unsafe handling of remote update inputs.

Authenticode packages currently use an existing self-signed Voltura certificate with timestamps. This is distinct from the independently verified RSA-PSS update signatures and does not establish public Windows publisher trust.
