# Packaging and release

Directory.Build.props is the only release-version source. Keep the app, rendering library, NSIS metadata, package names, signed update manifests, and generated site in sync. Keep master as the default branch.

## Prepare

```powershell
./scripts/verify.ps1
./scripts/build-branding.ps1
dotnet run --project tests/ClockSmoke -c Release -- --export-media site/assets
./scripts/build-social-preview.ps1
./scripts/build-site.ps1
./scripts/package.ps1
```

Review the running app and icon before capturing release media. Packaging requires NSIS and Windows SDK SignTool. Packages go to artifacts/publish; artifacts is disposable build output. Standard and offline setup use the same transactional installer maintenance, payload hashes, health check, and rollback logic adapted from Earner. Desktop shortcuts are optional. Setup runs as the current user; the Microsoft runtime prerequisite may require elevation.

The default Authenticode configuration reuses the existing local Voltura configuration at `%LOCALAPPDATA%\Voltura\Markdown\Signing\signing.json`. Override `-SigningConfig` to select another configuration with `certificateThumbprint`, `timestampUrl`, and `allowSelfSigned`. The existing certificate is self-signed; public Windows trust is not claimed. Do not acquire a certificate or replace the selected one silently. `-Unsigned` produces development-only packages without signed update manifests; never publish them as a supported in-app update.

The TextClock-specific RSA private key is DPAPI-protected at `%USERPROFILE%\.voltura\signing\textclock-update.dpapi`; the public key is embedded from VolturaTextClock/Updates/update-signing-public.pem. Keep the private key outside Git. DPAPI binds recovery to the Windows user profile: back it up using an appropriately protected profile/key-backup process before moving machines. Do not regenerate it for an existing release; previous app versions trust the current public key.

Scripts sign the app host before hashing payloads, sign and timestamp both setup variants, then create the portable ZIP and RSA-PSS signed schema-1 manifest. The manifest contains the version and exact installer name, byte size, and SHA-256. Verify the key matches the embedded public key before publication. Use `scripts/sign-update.ps1` only after final installer signing.

## Publish

Commit reviewed source and release notes, push master normally, and confirm required build/CodeQL results. Run `scripts/release.ps1 -Publish` to package the clean synchronized source and publish vVERSION with all five assets. `-PrepareOnly` validates and packages without publishing. After testing already prepared packages, use `-Publish -UsePrepared` to publish those exact bytes; this checks the recorded clean source commit and every asset hash. Publication requires no existing tag/release and matching clean local/remote master. Never replace an existing stable release's assets or move its tag.

Use `scripts/test-installer.ps1` for isolated maintenance acceptance. Where Windows Sandbox is available, `scripts/test-sandbox.ps1 -Variant full` starts a clean sandbox with networking disabled; `-Variant standard` uses a fresh sandbox with networking enabled for the Microsoft runtime prerequisite. Results are written to artifacts/sandbox-VARIANT/output/result.json. These scripts never run sandbox acceptance on the host.

The product site is built from site/ with version placeholders resolved into artifacts/site. GitHub Pages Actions publishes it from master. Configure the repository's Pages source as Actions, the custom domain as textclock.voltura.se, and DNS as a CNAME to voltura.github.io. Enable HTTPS once the certificate is provisioned. The website's Open Graph image does not configure GitHub's repository social preview: upload site/assets/social-preview.png separately in repository Settings → General → Social preview.

After publication verify the GitHub release points to the source commit, all five assets exist, app/installer metadata agrees, the site links reach the same installers, CodeQL is current, and HTTPS serves the new product page and preview image. Record any unavailable clean-VM or device checks in docs/verification.md instead of claiming they passed.
