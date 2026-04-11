# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [AionOverdose58.Data\AionOverdose58.Data.csproj](#aionoverdose58dataaionoverdose58datacsproj)
  - [AionOverdose58.Shared\AionOverdose58.Shared.csproj](#aionoverdose58sharedaionoverdose58sharedcsproj)
  - [AionOverdose58.Web\AionOverdose58.Web.Client\AionOverdose58.Web.Client.csproj](#aionoverdose58webaionoverdose58webclientaionoverdose58webclientcsproj)
  - [AionOverdose58.Web\AionOverdose58.Web\AionOverdose58.Web.csproj](#aionoverdose58webaionoverdose58webaionoverdose58webcsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 4 | All require upgrade |
| Total NuGet Packages | 5 | All packages need upgrade |
| Total Code Files | 16 |  |
| Total Code Files with Incidents | 5 |  |
| Total Lines of Code | 1236 |  |
| Total Number of Issues | 11 |  |
| Estimated LOC to modify | 1+ | at least 0.1% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [AionOverdose58.Data\AionOverdose58.Data.csproj](#aionoverdose58dataaionoverdose58datacsproj) | net9.0 | 🟢 Low | 3 | 0 |  | ClassLibrary, Sdk Style = True |
| [AionOverdose58.Shared\AionOverdose58.Shared.csproj](#aionoverdose58sharedaionoverdose58sharedcsproj) | net9.0 | 🟢 Low | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [AionOverdose58.Web\AionOverdose58.Web.Client\AionOverdose58.Web.Client.csproj](#aionoverdose58webaionoverdose58webclientaionoverdose58webclientcsproj) | net9.0 | 🟢 Low | 1 | 0 |  | AspNetCore, Sdk Style = True |
| [AionOverdose58.Web\AionOverdose58.Web\AionOverdose58.Web.csproj](#aionoverdose58webaionoverdose58webaionoverdose58webcsproj) | net9.0 | 🟢 Low | 2 | 1 | 1+ | AspNetCore, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 0 | 0.0% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 5 | 100.0% |
| ***Total NuGet Packages*** | ***5*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 1 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 3987 |  |
| ***Total APIs Analyzed*** | ***3988*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| Microsoft.AspNetCore.Components.WebAssembly | 9.0.14 | 10.0.5 | [AionOverdose58.Web.Client.csproj](#aionoverdose58webaionoverdose58webclientaionoverdose58webclientcsproj) | NuGet package upgrade is recommended |
| Microsoft.AspNetCore.Components.WebAssembly.Server | 9.0.14 | 10.0.5 | [AionOverdose58.Web.csproj](#aionoverdose58webaionoverdose58webaionoverdose58webcsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Design | 9.0.4 | 10.0.5 | [AionOverdose58.Data.csproj](#aionoverdose58dataaionoverdose58datacsproj)<br/>[AionOverdose58.Web.csproj](#aionoverdose58webaionoverdose58webaionoverdose58webcsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.4 | 10.0.5 | [AionOverdose58.Data.csproj](#aionoverdose58dataaionoverdose58datacsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Tools | 9.0.4 | 10.0.5 | [AionOverdose58.Data.csproj](#aionoverdose58dataaionoverdose58datacsproj) | NuGet package upgrade is recommended |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| M:Microsoft.AspNetCore.Builder.ExceptionHandlerExtensions.UseExceptionHandler(Microsoft.AspNetCore.Builder.IApplicationBuilder,System.String,System.Boolean) | 1 | 100.0% | Behavioral Change |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;AionOverdose58.Data.csproj</b><br/><small>net9.0</small>"]
    P2["<b>📦&nbsp;AionOverdose58.Shared.csproj</b><br/><small>net9.0</small>"]
    P3["<b>📦&nbsp;AionOverdose58.Web.Client.csproj</b><br/><small>net9.0</small>"]
    P4["<b>📦&nbsp;AionOverdose58.Web.csproj</b><br/><small>net9.0</small>"]
    P1 --> P2
    P4 --> P3
    P4 --> P1
    P4 --> P2
    click P1 "#aionoverdose58dataaionoverdose58datacsproj"
    click P2 "#aionoverdose58sharedaionoverdose58sharedcsproj"
    click P3 "#aionoverdose58webaionoverdose58webclientaionoverdose58webclientcsproj"
    click P4 "#aionoverdose58webaionoverdose58webaionoverdose58webcsproj"

```

## Project Details

<a id="aionoverdose58dataaionoverdose58datacsproj"></a>
### AionOverdose58.Data\AionOverdose58.Data.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 1
- **Number of Files**: 4
- **Number of Files with Incidents**: 1
- **Lines of Code**: 901
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P4["<b>📦&nbsp;AionOverdose58.Web.csproj</b><br/><small>net9.0</small>"]
        click P4 "#aionoverdose58webaionoverdose58webaionoverdose58webcsproj"
    end
    subgraph current["AionOverdose58.Data.csproj"]
        MAIN["<b>📦&nbsp;AionOverdose58.Data.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#aionoverdose58dataaionoverdose58datacsproj"
    end
    subgraph downstream["Dependencies (1"]
        P2["<b>📦&nbsp;AionOverdose58.Shared.csproj</b><br/><small>net9.0</small>"]
        click P2 "#aionoverdose58sharedaionoverdose58sharedcsproj"
    end
    P4 --> MAIN
    MAIN --> P2

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1332 |  |
| ***Total APIs Analyzed*** | ***1332*** |  |

<a id="aionoverdose58sharedaionoverdose58sharedcsproj"></a>
### AionOverdose58.Shared\AionOverdose58.Shared.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 2
- **Number of Files**: 8
- **Number of Files with Incidents**: 1
- **Lines of Code**: 114
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>📦&nbsp;AionOverdose58.Data.csproj</b><br/><small>net9.0</small>"]
        P4["<b>📦&nbsp;AionOverdose58.Web.csproj</b><br/><small>net9.0</small>"]
        click P1 "#aionoverdose58dataaionoverdose58datacsproj"
        click P4 "#aionoverdose58webaionoverdose58webaionoverdose58webcsproj"
    end
    subgraph current["AionOverdose58.Shared.csproj"]
        MAIN["<b>📦&nbsp;AionOverdose58.Shared.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#aionoverdose58sharedaionoverdose58sharedcsproj"
    end
    P1 --> MAIN
    P4 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 248 |  |
| ***Total APIs Analyzed*** | ***248*** |  |

<a id="aionoverdose58webaionoverdose58webclientaionoverdose58webclientcsproj"></a>
### AionOverdose58.Web\AionOverdose58.Web.Client\AionOverdose58.Web.Client.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 5
- **Number of Files with Incidents**: 1
- **Lines of Code**: 5
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P4["<b>📦&nbsp;AionOverdose58.Web.csproj</b><br/><small>net9.0</small>"]
        click P4 "#aionoverdose58webaionoverdose58webaionoverdose58webcsproj"
    end
    subgraph current["AionOverdose58.Web.Client.csproj"]
        MAIN["<b>📦&nbsp;AionOverdose58.Web.Client.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#aionoverdose58webaionoverdose58webclientaionoverdose58webclientcsproj"
    end
    P4 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 56 |  |
| ***Total APIs Analyzed*** | ***56*** |  |

<a id="aionoverdose58webaionoverdose58webaionoverdose58webcsproj"></a>
### AionOverdose58.Web\AionOverdose58.Web\AionOverdose58.Web.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 3
- **Dependants**: 0
- **Number of Files**: 23
- **Number of Files with Incidents**: 2
- **Lines of Code**: 216
- **Estimated LOC to modify**: 1+ (at least 0.5% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["AionOverdose58.Web.csproj"]
        MAIN["<b>📦&nbsp;AionOverdose58.Web.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#aionoverdose58webaionoverdose58webaionoverdose58webcsproj"
    end
    subgraph downstream["Dependencies (3"]
        P3["<b>📦&nbsp;AionOverdose58.Web.Client.csproj</b><br/><small>net9.0</small>"]
        P1["<b>📦&nbsp;AionOverdose58.Data.csproj</b><br/><small>net9.0</small>"]
        P2["<b>📦&nbsp;AionOverdose58.Shared.csproj</b><br/><small>net9.0</small>"]
        click P3 "#aionoverdose58webaionoverdose58webclientaionoverdose58webclientcsproj"
        click P1 "#aionoverdose58dataaionoverdose58datacsproj"
        click P2 "#aionoverdose58sharedaionoverdose58sharedcsproj"
    end
    MAIN --> P3
    MAIN --> P1
    MAIN --> P2

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 1 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 2351 |  |
| ***Total APIs Analyzed*** | ***2352*** |  |

