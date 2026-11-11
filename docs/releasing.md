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

Signed packaging uses the installer signing configuration in `scripts/signing.json`. Pass `-SigningConfig` to select another configuration or `-Unsigned` to build local development packages. The installer certificate is self-signed and Windows may show a publisher warning.

Supported in-app updates require a signed update manifest. Previously released builds verify it using the embedded public key, so use the established update signing identity.

Scripts sign the app host before hashing payloads, sign and timestamp both setup variants, then create the portable ZIP and RSA-PSS signed schema-1 manifest. The manifest contains the version and exact installer name, byte size, and SHA-256. Verify the key matches the embedded public key before publication. Use `scripts/sign-update.ps1` only after final installer signing.

## Publish

Commit reviewed source and release notes, push master normally, and confirm required build/CodeQL results. Run `scripts/release.ps1 -Publish` to package the clean synchronized source and publish vVERSION with all five assets. `-PrepareOnly` validates and packages without publishing. After testing already prepared packages, use `-Publish -UsePrepared` to publish those exact bytes; this checks the recorded clean source commit and every asset hash. Publication requires no existing tag/release and matching clean local/remote master. Never replace an existing stable release's assets or move its tag.

Use `scripts/test-installer.ps1` for isolated maintenance acceptance. Where Windows Sandbox is available, `scripts/test-sandbox.ps1 -Variant full` starts a clean sandbox with networking disabled; `-Variant standard` uses a fresh sandbox with networking enabled for the Microsoft runtime prerequisite. Results are written to artifacts/sandbox-VARIANT/output/result.json. These scripts never run sandbox acceptance on the host.

The product site is built from site/ with version placeholders resolved into artifacts/site. GitHub Pages Actions publishes it from master. Configure the repository's Pages source as Actions, the custom domain as textclock.voltura.se, and DNS as a CNAME to voltura.github.io. Enable HTTPS once the certificate is provisioned. The website's Open Graph image does not configure GitHub's repository social preview: upload site/assets/social-preview.png separately in repository Settings → General → Social preview.

After publication verify the GitHub release points to the source commit, all five assets exist, app/installer metadata agrees, the site links reach the same installers, CodeQL is current, and HTTPS serves the new product page and preview image. Record any unavailable clean-VM or device checks in docs/verification.md instead of claiming they passed.
