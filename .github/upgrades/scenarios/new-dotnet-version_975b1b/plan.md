# Upgrade Plan: .NET 9 → .NET 10

**Solution:** AionOverdose58.slnx  
**Source Branch:** main  
**Target Branch:** upgrade-to-NET10  
**Target Framework:** net10.0  

---

## Assessment Summary

| Metric | Value |
|--------|-------|
| Projects | 4 |
| Total Issues | 11 |
| Mandatory Issues | 4 |
| Potential Issues | 7 |
| Affected Files | 5 |
| Incompatible Packages | 0 |
| Packages to Update | 5 |

---

## Project Dependency Order

Projects must be upgraded in the following topological order to ensure dependencies are always up to date before the projects that consume them:

```
Level 0 (no dependencies):
  1. AionOverdose58.Shared
  2. AionOverdose58.Web.Client

Level 1 (depends on Level 0):
  3. AionOverdose58.Data  →  depends on: Shared

Level 2 (depends on Levels 0–1):
  4. AionOverdose58.Web   →  depends on: Shared, Data, Web.Client
```

---

## Issues to Resolve

### 1. Target Framework Update (Mandatory — all 4 projects)

**Rule:** `Project.0002`  
**Severity:** Mandatory  

All four projects currently target `net9.0`. Each project file's `<TargetFramework>` element must be changed to `net10.0`.

| Project | File |
|---------|------|
| AionOverdose58.Shared | `AionOverdose58.Shared\AionOverdose58.Shared.csproj` |
| AionOverdose58.Web.Client | `AionOverdose58.Web\AionOverdose58.Web.Client\AionOverdose58.Web.Client.csproj` |
| AionOverdose58.Data | `AionOverdose58.Data\AionOverdose58.Data.csproj` |
| AionOverdose58.Web | `AionOverdose58.Web\AionOverdose58.Web\AionOverdose58.Web.csproj` |

**Action:** In each `.csproj`, replace:
```xml
<TargetFramework>net9.0</TargetFramework>
```
with:
```xml
<TargetFramework>net10.0</TargetFramework>
```

---

### 2. NuGet Package Updates (Potential — 3 projects)

**Rule:** `NuGet.0002`  
**Severity:** Potential  

All packages should be updated to versions aligned with .NET 10. No packages are incompatible — only version bumps are required.

| Package | Current Version | Target Version | Project(s) |
|---------|----------------|----------------|------------|
| `Microsoft.AspNetCore.Components.WebAssembly` | 9.0.14 | **10.0.5** | Web.Client |
| `Microsoft.AspNetCore.Components.WebAssembly.Server` | 9.0.14 | **10.0.5** | Web |
| `Microsoft.EntityFrameworkCore.Design` | 9.0.4 | **10.0.5** | Data, Web |
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.0.4 | **10.0.5** | Data |
| `Microsoft.EntityFrameworkCore.Tools` | 9.0.4 | **10.0.5** | Data |

**Action:** In each affected `.csproj`, update the `Version` attribute of each `<PackageReference>` to the target version shown above.

---

### 3. Behavioral Change — UseExceptionHandler (Potential — Web project)

**Rule:** `Api.0003`  
**Severity:** Potential  
**File:** `AionOverdose58.Web\AionOverdose58.Web\Program.cs`, line 34  

```csharp
app.UseExceptionHandler("/Error", createScopeForErrors: true);
```

In .NET 10, `UseExceptionHandler(string path, bool createScopeForErrors)` has a behavioral change. Code and binaries may behave differently at runtime without requiring recompilation.

**Action:** Review and verify the exception handler behavior post-upgrade by:
1. Confirming the `/Error` route resolves correctly in the Blazor app after the upgrade.
2. Running the application in a non-development environment to exercise the exception handler path.
3. If the behavior is undesirable, consider switching to the explicit options-based overload:

```csharp
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandlingPath = "/Error",
    CreateScopeForErrors = true
});
```

---

## Upgrade Steps (Execution Order)

### Step 1 — Validate .NET 10 SDK
Verify the .NET 10 SDK is installed on the machine before making any project changes.

### Step 2 — Upgrade AionOverdose58.Shared
- Update `<TargetFramework>` from `net9.0` → `net10.0`
- Build to verify

### Step 3 — Upgrade AionOverdose58.Web.Client
- Update `<TargetFramework>` from `net9.0` → `net10.0`
- Update `Microsoft.AspNetCore.Components.WebAssembly` → `10.0.5`
- Build to verify

### Step 4 — Upgrade AionOverdose58.Data
- Update `<TargetFramework>` from `net9.0` → `net10.0`
- Update `Microsoft.EntityFrameworkCore.Design` → `10.0.5`
- Update `Microsoft.EntityFrameworkCore.SqlServer` → `10.0.5`
- Update `Microsoft.EntityFrameworkCore.Tools` → `10.0.5`
- Build to verify

### Step 5 — Upgrade AionOverdose58.Web
- Update `<TargetFramework>` from `net9.0` → `net10.0`
- Update `Microsoft.AspNetCore.Components.WebAssembly.Server` → `10.0.5`
- Update `Microsoft.EntityFrameworkCore.Design` → `10.0.5`
- Review `UseExceptionHandler` behavioral change in `Program.cs` (line 34)
- Build to verify

### Step 6 — Full Solution Build & Test
- Build the entire solution and verify zero errors
- Run all tests (if test projects are present)
- Commit all changes to branch `upgrade-to-NET10`

---

## Risk Summary

| Risk | Severity | Mitigation |
|------|----------|-----------|
| `UseExceptionHandler` behavioral change | Low | Review in staging; consider explicit options overload if issues arise |
| EF Core major version bump (9→10) | Low | No breaking changes identified; verify DB migrations still apply |
| Blazor WebAssembly package update | Low | No incompatibility flagged; re-test client-side rendering post-upgrade |
