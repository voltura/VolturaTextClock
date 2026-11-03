# Verification

Run scripts/verify.ps1 for a warning-free Release build, the original clock smoke harness, isolated reliability checks, and a dependency vulnerability query. The query reports known vulnerabilities from configured package sources; it is not a security audit.

ClockSmoke covers all 1,440 minutes in Swedish and English, known five-minute and rollover phrases, system-language fallback, four render sizes, synthetic DPI changes, minimizing/restoring, background loading without source-file locks, settings persistence, and version comparison. Its settings file is isolated and startup integration is disabled for test entry assemblies.

Reliability exercises old-settings migration, unknown-key retention, malformed-file recovery, invalid types, atomic-write cleanup, independent UI translations, signed manifests, wrong signatures/assets, oversized installers, check-versus-download separation, altered cached installers, retry, older releases, unexpected origins, cancellation, and network interruption. Simulated installers are never executed.

Manual acceptance should cover the actual English/Swedish settings tabs, keyboard controls, 100–200% scaling, a small desktop, switching monitors, pinning, and missing custom backgrounds. Renderer exports and synthetic DPI messages do not establish physical-monitor behavior.

Installer acceptance includes a fresh per-user installation, same-version reinstall, future-version upgrade simulation, rollback after staging/promotion, required runtime handling, offline setup, metadata, shortcuts, uninstall, startup-entry ownership, and retained settings. Test isolated maintenance roots use an explicit `.voltura-installer-test` marker and never represent a clean Windows VM.

Website acceptance covers desktop and phone layouts, no horizontal overflow, image alt text and loading, keyboard navigation, package URLs, HTTPS/custom domain, canonical and social metadata, and the separately configured GitHub repository preview.

Clean-machine runtime installation, offline installation on a machine without .NET, and physical multi-monitor DPI changes require their actual environments. Do not infer them from a successful developer-machine build or installer run.
