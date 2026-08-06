# MAUI

Boilerplate .NET MAUI solution: a layered app head over a platform-agnostic core, a design-system
library (in-house components plus a Material Design 3 implementation), and a JSX/Markdown-to-XAML
extension layer.

Targets `net9.0-android`, `net9.0-ios`, `net9.0-maccatalyst` (and `net*-windows` on Windows).
The SDK is pinned in [global.json](global.json) to `10.0.300` with `rollForward: latestFeature`.

---

## Architecture

The solution is a **layered / clean-ish architecture** around an **MVVM** presentation layer. The
governing rule is that the *core is platform-free*: `Core`, `Api`, and `Model` are plain `net9.0`
libraries with no MAUI workload dependency, so they compile and unit-test without a device or an
emulator. Everything that touches MAUI, the Shell, or a platform SDK lives in the app head or the
MAUI-targeted libraries.

### Projects

| Project | TFM | Role |
| --- | --- | --- |
| `CompanyName.AppName.MAUI` | net9.0-android/ios/maccatalyst | **App head.** Composition root, Shell, routes, views, platform code. The only project that knows about `Shell`. |
| `CompanyName.AppName.Core` | net9.0 | **Presentation logic.** View models and the navigation abstraction. No MAUI reference. |
| `CompanyName.AppName.Api` | net9.0 | **Service clients.** Deliberately not a MAUI target so clients can be tested against a fake `HttpMessageHandler`. Currently a scaffold. |
| `CompanyName.AppName.Model` | net9.0 | **Contracts / DTOs.** Currently a scaffold. |
| `CompanyName.AppName.Design.MAUI` | net9.0-android/ios/maccatalyst | **Design system.** Components, Material 3 tokens, themes, fonts, catalog pages. |
| `CompanyName.AppName.Extensions.MAUI` | net9.0-android/ios/maccatalyst | **Markup extensions.** JSX and Markdown parsers that project onto MAUI controls via wrappers. |
| `CompanyName.AppName.Core.Tests` | net10.0 | xUnit tests over `Core`. |
| `CompanyName.AppName.MAUI.Design` | net7.0 | Legacy stub. |

### Dependency flow

```
                       MAUI (app head)
                     /        |        \
        Extensions.MAUI   Design.MAUI   Core
                \_____________/           |
                                         Api
                                          |
                                        Model
```

Dependencies point inward only. `Core` never references a MAUI project; the app head references
everything and wires it together.

### Data / persistence

**There is no database or persistence layer in this solution.** No ORM, no local store, no
connection strings, no caching layer. When one is added, it belongs behind an interface in `Core`
(alongside `INavigationService`) with the implementation in `Api` or a new infrastructure project,
so view models stay testable and the core stays platform-free.

Remote data is intended to flow `Model` (DTOs) → `Api` (typed `HttpClient` clients) → `Core`
(view models), using `HttpClient` and `System.Text.Json` from the shared framework.

---

## Dependency injection

DI is the built-in `Microsoft.Extensions.DependencyInjection` container that ships with
`MauiAppBuilder`. There is **no service locator and no `Shell.Current` in view models** — the
static Shell dependency is isolated at the platform boundary.

### Composition root

[MauiProgram.cs](CompanyName.AppName.MAUI/MauiProgram.cs) is the single composition root, split
into three intention-revealing registration methods:

```csharp
RegisterNavigation(builder.Services);   // INavigationService, AppShell, route→page mapping
RegisterViewModels(builder.Services);   // view models
RegisterPages(builder.Services);        // pages, so the container injects their view models
```

### Lifetimes

| Lifetime | Used for | Why |
| --- | --- | --- |
| **Singleton** | `INavigationService`, `AppShell` | Stateless service; one shell per app. |
| **Transient** | View models, pages | Each page gets a fresh view model — no state leaking between visits. |

### Patterns in use

**Constructor injection everywhere.** `App` takes `AppShell`; `SystemDesignViewModel` takes
`INavigationService`. No property injection, no static access.

**Abstraction at the platform boundary.** `INavigationService` lives in `Core`;
`ShellNavigationService` in the app head is the only code that touches `Shell.Current`, and it
marshals every call to the UI thread. Tests substitute
[FakeNavigationService](CompanyName.AppName.Core.Tests/FakeNavigationService.cs), which records
routes.

**Factory `DataTemplate` for container-resolved pages.** [AppShell.xaml.cs](CompanyName.AppName.MAUI/AppShell.xaml.cs)
builds tabs with `new DataTemplate(() => _services.GetRequiredService<MainPage>())` — the factory
overload is what routes page construction through the container while keeping creation lazy.

**Single source of truth for routes.** [AppRoutes.cs](CompanyName.AppName.MAUI/Navigation/AppRoutes.cs)
holds the `(Route, Type Page)` list consumed by both Shell route registration and container page
registration, so the two cannot drift. Adding a page is one line.

**Builder extension for the design system.** `UseApplicationDesignSystem()` in
[AppBuilderExtensions.cs](CompanyName.AppName.Design.MAUI/Extensions/AppBuilderExtensions.cs)
encapsulates font registration, keeping the library's setup out of `MauiProgram`.

**Injected root window.** `App` receives the shell and returns it from `CreateWindow` rather than
assigning the obsolete `MainPage` property (the supported MAUI 9 approach).

---

## Folder structure

```
CompanyName.AppName.MAUI/            # app head
├── MauiProgram.cs                   # composition root
├── App.xaml(.cs)  AppShell.xaml(.cs)
├── Navigation/                      # AppRoutes, ShellNavigationService
├── Views/                           # pages (XAML + code-behind)
├── Platforms/                       # Android, iOS, MacCatalyst, Windows, Tizen
├── Properties/                      # launchSettings.json
└── Resources/                       # AppIcon, Fonts, Images, Raw, Splash, Styles

CompanyName.AppName.Core/            # platform-free presentation logic
├── Navigation/                      # INavigationService
└── ViewModels/                      # BaseViewModel, feature view models

CompanyName.AppName.Design.MAUI/     # design system
├── Components/                      # Buttons/{StandardButtons,IconsButtons}, Inputs, Notifications
├── Material/                        # Material Design 3
│   ├── Components/                  # MaterialButton, MaterialCard, MaterialTextField, …
│   ├── Enumerations/                # MaterialVariants
│   └── Tokens/                      # MaterialRole, MaterialColorScheme, Elevation, Shape, Typography
├── Enumerations/                    # ButtonSize, InputType, LoaderSize
├── Extensions/                      # AppBuilderExtensions, VisualElementExtensions, …
├── Platforms/                       # per-platform component handlers
├── Resources/                       # Fonts (Montserrat, Roboto), Styles, Styles/Themes
├── SystemDesigns/                   # catalog / showcase pages
└── Typography/

CompanyName.AppName.Extensions.MAUI/ # markup → MAUI controls
├── Contracts/                       # IComponentWrapper
├── Enumerations/                    # ExtensionType, XamlType
├── Jsx/                             # JsxParser + Enumerations, Extensions
├── Markdown/                        # MarkdownParser + Contracts, Models, Enumerations, Extensions
├── Wrappers/                        # ComponentWrapperBase + one wrapper per control
└── Platforms/

CompanyName.AppName.Api/             # service clients (scaffold)
CompanyName.AppName.Model/           # DTOs / contracts (scaffold)
CompanyName.AppName.Core.Tests/      # xUnit tests
```

### Conventions

- **Folder = namespace.** `Design.MAUI/Material/Tokens/` → `…Design.MAUI.Material.Tokens`.
- **Group by feature, then by kind.** `Material/` owns its own Components / Enumerations / Tokens
  rather than scattering them into solution-wide buckets.
- **`Contracts/` and `Enumerations/`** are the standard names for interfaces and enums.
- **`Platforms/`** mirrors the MAUI multi-targeting convention; platform-specific partials sit
  under the platform folder with the same relative path as the shared file.
- Catalog pages for a component live in `SystemDesigns/`, next to nothing else.

---

## Coding standards

Conventions observed consistently across the codebase. There is currently **no `.editorconfig` or
`Directory.Build.props`** — adding one to enforce the below is a reasonable next step.

### Language and project settings

- `Nullable` **enabled** — annotate with `?`, don't suppress with `!` without cause.
- `ImplicitUsings` **enabled** — no boilerplate `using System;`.
- `net9.0` for libraries; MAUI TFMs only where a MAUI type is genuinely needed.

### Naming and layout

| Element | Convention | Example |
| --- | --- | --- |
| Types, methods, public members | `PascalCase` | `ShellNavigationService`, `GoToAsync` |
| Private fields (injected deps) | `_camelCase` | `_navigation`, `_services` |
| Private fields (local state) | `_camelCase` | `_sizeAllocated` |
| `[ObservableProperty]` backing fields | `camelCase`, **no** underscore | `isLoading` → generates `IsLoading` |
| Enum members | `PascalCase`, spec-faithful | `OnPrimaryContainer` |
| Async methods | `…Async` suffix | `GoBackAsync` |
| Files | one type per file, named after the type | |

- **File-scoped namespaces** (`namespace X;`) throughout.
- Four-space indentation in the newer projects; the app-head files use tabs — match the file you
  are editing.
- Default (implicit) `private` access — the code writes `readonly INavigationService _navigation;`
  and `void BuildTabs()`, not `private …`.
- `sealed` on implementation classes not designed for inheritance (`ShellNavigationService`,
  `FakeNavigationService`).
- Expression-bodied members for one-liners: `public Task GoBackAsync() => …`.

### MVVM

- Use **CommunityToolkit.Mvvm** source generators — never hand-write `INotifyPropertyChanged`.
  - `[ObservableProperty]` on a field generates the PascalCase property.
  - `[RelayCommand]` on `Foo()` generates `FooCommand` for XAML to bind.
- View models derive from `BaseViewModel` (loading/status state) → `ObservableObject`.
- View models are `partial` (required by the generators) and live in `Core`.
- **View models never reference MAUI types**, `Shell.Current`, or any static app state. Take an
  abstraction in the constructor instead.
- Code-behind stays thin: `InitializeComponent()` and wiring only; logic belongs in the view model.

### Documentation

This is the most distinctive standard in the codebase and worth preserving:

- **XML doc comments on every public type and member** — `<summary>`, `<param>`, `<returns>`.
- **`<remarks>` explains *why*, not what.** Existing examples state the rationale for a design
  decision — why `INavigationService` exists instead of `Shell.Current`, why the factory
  `DataTemplate` overload is used, why a Jint engine needs a gate. Follow this: if a choice would
  puzzle the next reader, put the reason in `<remarks>`.
- Reference other members with `<see cref="…"/>` so links survive renames.
- Inline `//` comments are reserved for non-obvious mechanics; mark deferred work with
  `// TODO:` plus the issue reference.

### Testing

- **xUnit**, in `CompanyName.AppName.Core.Tests`.
- Test through abstractions with hand-written fakes (`FakeNavigationService`) rather than a mocking
  framework — none is referenced.
- Fakes are `internal sealed` and record what they were asked to do, so tests assert on observable
  behaviour.
- Because `Core` is platform-free, tests run on plain .NET — no device, emulator, or MAUI workload.

```bash
dotnet test CompanyName.AppName.Core.Tests
```

### Design-system rules

- Components ask for a **`MaterialRole`**, never a literal colour — the theme resolves the value.
  This is what keeps light/dark theming working.
- Tokens (`MaterialElevation`, `MaterialShape`, `MaterialTypography`) are the vocabulary; don't
  introduce magic numbers in a component.
- Every new component gets a page under `SystemDesigns/` demonstrating its variants.
- Shared visual behaviour goes in an extension (`VisualElementExtensions`) or a helper
  (`ShadowElevation`), not copy-pasted between components.

---

## CI/CD

Two GitHub Actions workflows, both supporting the **UAT**, **STAGE** and **RELEASE** environments.

| Workflow | Trigger | Does |
| --- | --- | --- |
| [ci.yml](.github/workflows/ci.yml) | push / PR to `main`, manual | Unit tests, then an Android compile for all three environments in a matrix. Linux only — no macOS minutes. |
| [cd.yml](.github/workflows/cd.yml) | manual (pick environment), or tag push | Tests, then signed Android AAB/APK and iOS IPA for one environment, uploaded as artifacts. |

### Running a deployment build

From the Actions tab → **CD** → *Run workflow*, choose the environment, optionally a display
version, and which platforms to build. Or push a tag:

| Tag | Environment |
| --- | --- |
| `v1.4.0-uat` | UAT |
| `v1.4.0-stage` | STAGE |
| `v1.4.0` | RELEASE |

`ApplicationVersion` (the build number) is always the workflow run number;
`ApplicationDisplayVersion` comes from the tag or input, defaulting to `1.0.<run number>`.

### How environments differ

Defined in [Build/Environments.props](Build/Environments.props), imported by the app head and
selected with `-p:AppEnvironment=…`. UAT and STAGE get a suffixed app id so all three install side
by side on one device; RELEASE keeps the unsuffixed id.

| | Display name | Application id | API base URL | Constant |
| --- | --- | --- | --- | --- |
| UAT | AppName UAT | `…appname.maui.uat` | `https://api-uat.companyname.com/` | `ENV_UAT` |
| STAGE | AppName STAGE | `…appname.maui.stage` | `https://api-stage.companyname.com/` | `ENV_STAGE` |
| RELEASE | AppName | `…appname.maui` | `https://api.companyname.com/` | `ENV_RELEASE` |

**Point these at your real hosts** — the URLs above are placeholders.

The values are stamped into the assembly as `AssemblyMetadata` and read back through
[IAppEnvironment](CompanyName.AppName.Core/Configuration/IAppEnvironment.cs), registered as a
singleton in `MauiProgram`. Inject it wherever the environment matters:

```csharp
public MyViewModel(IAppEnvironment environment) => _baseUrl = environment.ApiBaseUrl;
```

Building locally without `-p:AppEnvironment` yields RELEASE, so IDE behaviour is unchanged.

```bash
dotnet build CompanyName.AppName.MAUI/CompanyName.AppName.MAUI.csproj \
  -f net9.0-android -c Release -p:AppEnvironment=UAT
```

### Signing

**Signing is optional and the pipeline is green without it** — with no secrets it emits an
unsigned APK and compiles iOS for the simulator. Add the secrets and the same jobs start producing
a signed AAB/APK and an IPA.

Create GitHub Environments named `UAT`, `STAGE` and `RELEASE` (Settings → Environments) — these
scope the secrets per environment and are where you add approval gates for RELEASE. Then set:

| Secret | Platform |
| --- | --- |
| `ANDROID_KEYSTORE_BASE64` | Android — `base64 -i release.keystore` |
| `ANDROID_KEYSTORE_PASSWORD`, `ANDROID_KEY_ALIAS`, `ANDROID_KEY_PASSWORD` | Android |
| `IOS_CERTIFICATE_BASE64`, `IOS_CERTIFICATE_PASSWORD` | iOS — distribution `.p12` |
| `IOS_PROVISIONING_PROFILE_BASE64`, `IOS_PROVISIONING_PROFILE_NAME` | iOS |
| `IOS_CODESIGN_KEY` | iOS — e.g. `Apple Distribution: Company (TEAMID)` |

Each environment needs its **own** provisioning profile, matching that environment's application
id. The keystore is decoded to `$RUNNER_TEMP` (outside the workspace, so no artifact glob can pick
it up) and deleted in an `always()` step; iOS certs go into a throwaway keychain.

---

## Getting started

```bash
dotnet restore CompanyName.AppName.MAUI.sln
dotnet build   CompanyName.AppName.MAUI.sln
dotnet test    CompanyName.AppName.Core.Tests
```

Build with the official .NET install at `/usr/local/share/dotnet` — a Homebrew .NET does not carry
the MAUI workload.
