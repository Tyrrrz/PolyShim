# Contributing

## Local Development

This project relies on [DevContainers](https://containers.dev) for setting up the local development environment in a consistent and reproducible manner.
Make sure you use VS Code or any other [compatible IDE or tooling](https://containers.dev/supporting) for it to work.

## Authoring Polyfills

### Layout

- `PolyShim/` contains the actual polyfill source files
- Files are organized by the version of .NET (Core) where the original API is provided natively, for example:
  - `PolyShim/NetCore20/`
  - `PolyShim/NetCore30/`
  - `PolyShim/Net50/`
  - `PolyShim/Net60/`
  - `PolyShim/Net90/`
- The file name matches the type which is (or whose members are) being polyfilled (e.g., `String.cs`, `Queue.cs`, `File.cs`, `BitOperations.cs`)
- `PolyShim.Tests/` mirrors this layout with tests like `PolyShim.Tests/Net60/StringTests.cs`
- `PolyShim/PolyShim.props` and `PolyShim/PolyShim.targets` define framework/feature detection and compile-time constants used throughout the project

### Implementation patterns

- Type polyfills
  - Types added in newer frameworks are recreated in their corresponding `System.*` namespace
  - Example:
    - Original type: `System.Span`
    - Polyfill source: `PolyShim/NetCore21/Span.cs`
    - Definition: `namespace System;` and `internal readonly ref struct Span<T>`
    - Implementation: recreation of the original
  - Add a documentation link for the original type as a comment above the definition
- Member polyfills
  - Missing members are added as global extension members to existing types
  - These are defined as `internal static class MemberPolyfills_<Framework>_<Type>` and use the `extension(...)` block syntax.
  - Omit the namespace declaration to make the extension members available globally
    - Except when polyfilling other extension members (e.g., `System.Linq.EnumerableExtensions`), in which case keep the original namespace
  - Example:
    - Original type: `System.Collections.Generic.Queue`
    - Polyfill source: `PolyShim/NetCore20/Queue.cs`
    - Definition: `internal static class MemberPolyfills_NetCore20_Queue`
    - Implementation: `extension(Queue<T> queue)` and members such as `TryDequeue()`/`TryPeek()`
  - Add a documentation link for the original member as a comment above each definition
- All type declarations should be internal
- All relevant declarations should be marked with `[ExcludeFromCodeCoverage]`, guarded by the `POLYSHIM_INCLUDE_COVERAGE` compilation constant
- One source file should only contain polyfills related to one type and one framework version
- Use `#if` statements to prevent polyfills from showing up where they are not required
  - Filter by feature (i.e., `#if !FEATURE_MEMORY`) when polyfilling types or members provided by a feature
    - Example: polyfilling the `System.Span<T>` and `System.Memory<T>` types
  - Filter by TFMs (i.e., `#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)`) when polyfilling types or members added in a specific version of the framework
    - Always use the full format that includes all 3 separate frameworks, for consistency
    - No need to add TFM guards when the file already has feature guards, as those have TFM conditions defined in `PolyShim.targets`
    - Example: polyfilling the `Contains(char)` method on `System.String`
- Use `#if` statements to declare when polyfills have dependencies on other APIs
  - Filter by feature (i.e., `#if FEATURE_JSON`) when polyfilling types or members that depend on APIs provided by a feature
    - Example: polyfilling the `Parse(string, JsonDocumentOptions)` method on `System.Text.Json.JsonElement` (requires `System.Text.Json`)
  - Filter by TFMs (i.e., `#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER`) when polyfilling types or members that depend on APIs provided in a specific version of the framework
    - Example: polyfilling the `ProcessId` property on `System.Environment` (requires `System.Diagnostics.Process`)
  - Always precede with a short comment explaining why a filter is necessary
  - Keep the scope of the filter as narrow as possible
  - Don't add a guard when the polyfill depends on APIs that are themselves separately polyfilled
    - Example: no need for `#if FEATURE_MEMORY` because `System.Span` and `System.Memory` are already polyfilled
  - Try to avoid dependencies altogether if possible; ideally polyfills should be universal
- Try to use existing polyfills where implementing others; avoid duplication as much as possible
- Add `#nullable enable` to each source file to ensure nullability checks even if the consumer has them disabled

Typical skeletons:

- Type polyfill:
    ```csharp
    #if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
    #nullable enable
    #pragma warning disable CS0436

    using System.Diagnostics.CodeAnalysis;

    namespace System.Numerics;

    #if !POLYSHIM_INCLUDE_COVERAGE
    [ExcludeFromCodeCoverage]
    #endif
    internal static class BitOperations
    {
        public static int PopCount(uint value) { /* ... */ }
    }
    #endif
    ```
- Member polyfill:
    ```csharp
    #if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
    #nullable enable
    #pragma warning disable CS0436

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    #if !POLYSHIM_INCLUDE_COVERAGE
    [ExcludeFromCodeCoverage]
    #endif
    internal static class MemberPolyfills_NetCore20_Queue
    {
        extension<T>(Queue<T> queue)
        {
            public bool TryDequeue(out T? result) { /* ... */ }
        }
    }
    #endif
    ```

### Testing

Tests run against the latest version of .NET, as well as the oldest version of .NET that the test framework supports.
The former is used to verify the assertions themselves, while the latter is used to verify the polyfill implementations using those assertions.
