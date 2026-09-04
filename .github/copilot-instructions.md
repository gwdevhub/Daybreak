# Copilot instructions for Daybreak

Daybreak is a cross-platform Guild Wars launcher built on .NET 10 and
Photino.Blazor. Read the
[Architecture Overview](../README.md#architecture-overview) for the project
layout and [CONTRIBUTING.md](../CONTRIBUTING.md) for the branching and release
process.

## Workflow

- Branch off the current `release/[version]` branch, never off `master`.
- One feature or fix per branch and per PR.
- Never push to `master` and never edit the version in `Directory.Build.props`.
  Both are owned by the release workflows.
- Title PRs `Short description (Closes #123)` so the linked issue gets labelled.

## Build and test

CI does not run on PRs targeting a release branch, so validate locally. Run the
smallest command that covers the change:

```bash
dotnet build Daybreak.Linux/Daybreak.Linux.csproj      # or Daybreak.Windows
dotnet test Daybreak.Tests/Daybreak.Tests.csproj --filter "FullyQualifiedName~YourTests"
```

Keep the build warning-free.

## Where code goes

| Change                                             | Project                              |
| -------------------------------------------------- | ------------------------------------ |
| Models, utilities, interfaces shared by everything | `Daybreak.Shared`                    |
| Blazor views and services                          | `Daybreak.Core`                      |
| Platform-specific implementations                  | `Daybreak.Windows`, `Daybreak.Linux` |

`Daybreak.Tests` only references `Daybreak.Core`. When adding platform code, put
the logic worth testing in `Daybreak.Shared` and leave the platform project as a
thin caller.

Add NuGet packages through `Directory.Packages.props`.

## C# conventions

`.editorconfig` is the source of truth. The projects are set up to treat
warnings as errors.

## Verify before claiming

Prefer evidence over reasoning about behaviour, especially for the Wine and
injection paths. Wine is scriptable, so reproduce the actual failure and confirm
the fix against it rather than inferring what a Win32 call does.

## Writing style

Applies to docs, comments, PR descriptions and answers.

- Be short. Cut filler, recap and process narration.
- No AI-isms and no inflated language.
- Do not paste code into documentation. Link to the file so there is only one
  copy of it.
- Explain the reason for a change, not a summary of the diff.

## Markdown

- No inline HTML. Use `![alt][ref]` with a reference definition instead of an
  `img` tag.
- Start a mermaid fence with the diagram type, for example `flowchart LR`. YAML
  frontmatter inside the fence is rejected by some renderers.
- Long URLs belong in reference-style link definitions at the bottom of the
  file.
