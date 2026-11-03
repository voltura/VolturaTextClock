# Contributing

Use Windows x64, the .NET SDK pinned in global.json, PowerShell 7, and NSIS for installers. Keep changes focused on this small WinForms app; preserve existing clock settings and the public rendering library.

```powershell
./scripts/verify.ps1
./scripts/build-branding.ps1
dotnet run --project VolturaTextClock -c Release
```

The existing ClockSmoke harness covers all 1,440 minutes in both languages and relevant settings/rendering behavior. Reliability covers settings recovery and update trust boundaries with isolated files and simulated HTTP; it never executes downloaded test bytes. Check actual UI changes at normal and high DPI, in English and Swedish, and on a small screen.

The single release-version source is Directory.Build.props. All assemblies, installer filenames, update manifests, and website downloads derive from it. See docs/releasing.md for packaging, signing, and publication. The repository keeps its existing master branch.

The product website source is site/. Run scripts/build-site.ps1 and serve artifacts/site to preview the generated versioned links. Branding is generated from editable System.Drawing code with scripts/build-branding.ps1; marketing clock images come from the real renderer, not invented clock artwork.

By contributing you agree to license your changes under the repository's GPL-3.0 terms. Retain third-party notices. Report security issues through SECURITY.md instead of public issue details.
