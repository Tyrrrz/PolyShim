# PolyShim

[![Made in Ukraine](https://img.shields.io/badge/made_in-ukraine-ffd700.svg?labelColor=0057b7)](https://tyrrrz.me/ukraine)
[![Build](https://img.shields.io/github/actions/workflow/status/Tyrrrz/PolyShim/main.yml?branch=master)](https://github.com/Tyrrrz/PolyShim/actions)
[![Coverage](https://img.shields.io/codecov/c/github/Tyrrrz/PolyShim/master)](https://codecov.io/gh/Tyrrrz/PolyShim)
[![Version](https://img.shields.io/nuget/v/PolyShim.svg)](https://nuget.org/packages/PolyShim)
[![Downloads](https://img.shields.io/nuget/dt/PolyShim.svg)](https://nuget.org/packages/PolyShim)
[![Discord](https://img.shields.io/discord/869237470565392384?label=discord)](https://discord.gg/2SUWKFnHSm)
[![Donate](https://img.shields.io/badge/donate-$$$-8a2be2.svg)](https://tyrrrz.me/donate)
[![Fuck Russia](https://img.shields.io/badge/fuck-russia-e4181c.svg?labelColor=000000)](https://twitter.com/tyrrrz/status/1495972128977571848)

> 🟢 **Project status**: active<sup>[[?]](https://github.com/Tyrrrz/.github/blob/master/docs/project-status.md)</sup>

**PolyShim** is a collection of polyfills that enable many modern BCL and compiler features for projects targeting older versions of .NET.
It's distributed as a source-only package that can be referenced without imposing any run-time dependencies.

## Terms of use<sup>[[?]](https://github.com/Tyrrrz/.github/blob/master/docs/why-so-political.md)</sup>

By using this project or its source code, for any purpose and in any shape or form, you grant your **implicit agreement** to all the following statements:

- You **condemn Russia and its military aggression against Ukraine**
- You **recognize that Russia is an occupant that unlawfully invaded a sovereign state**
- You **support Ukraine's territorial integrity, including its claims over temporarily occupied territories of Crimea and Donbas**
- You **reject false narratives perpetuated by Russian state propaganda**

To learn more about the war and how you can help, [click here](https://tyrrrz.me/ukraine). Glory to Ukraine! 🇺🇦

## Install

- 📦 [NuGet](https://nuget.org/packages/PolyShim): `dotnet add package PolyShim`

> **Warning**:
> To use this package, you must have the latest version of the .NET SDK installed.
> This is only required for the build process, and does not affect which version of the runtime you can target.

## Features

- Enables compiler support for:
  - Nullable reference types
  - Init-only properties and records
  - Required properties
  - Named tuples
  - Module initializers
  - & more...
- Provides type polyfills for:
  - `ValueTuple<...>`
  - `Index` and `Range`
  - `HashCode`
  - `SkipLocalsInitAttribute`
  - `CallerArgumentExpressionAttribute`
  - `ExcludeFromCodeCoverageAttribute`
  - & more...
- Provides method shims for many built-in types
- Adjusts polyfills based on available capabilities
- Targets .NET Standard 1.0+, .NET Core 1.0+, .NET Framework 3.5+
- Imposes no run-time dependencies

## Usage

**PolyShim** polyfills come in two forms:
- Type polyfills, which define missing built-in types
- Method polyfills, which define extension methods that shim missing methods on existing built-in types

Once the package is installed, the polyfills will be automatically added to your project as internal source files.
You can then use them in your code by referencing the corresponding types or methods as if they were defined natively.

### Type polyfills

**PolyShim** provides various types that are not available natively on older target frameworks.
These types are defined within the corresponding `System.*` namespaces and mimic the behavior of their original implementations as closely as possible.

For example, with **PolyShim** you can use the `Index` and `Range` structs (added in .NET Core 3.0) on any older version of .NET:

```csharp
using System;

// On older framworks, these are replaced by polyfills
var index = new Index(1, fromEnd: true);
var range = new Range(
    new Index(3),
    new Index(1, true)
);
```

You can also use compiler features that rely on these types, such as the advanced indexing and slicing operators:

```csharp
var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// On older framworks, these are replaced by polyfills
var last = array[^1];
var part = array[3..^1];
```

### Method polyfills

**PolyShim** provides a number of extension methods that act as shims for instance methods that are not available natively on older target frameworks.
These extension methods are typically defined within the global namespace, so they can be called on the corresponding types just like instance methods, without any additional `using` directives.

For example, with **PolyShim** you can use the `String.Contains(char)` method (added in .NET Core 2.0) on any older version of .NET:

```csharp
var str = "Hello world";

// On older framworks, this is replaced by a polyfill
var contains = str.Contains('w');
```

### Compatibility packages

Some features from newer versions of .NET can also be made available on older frameworks using official compatibility packages published by Microsoft.
**PolyShim** automatically detects if any of these packages are installed and adjusts its polyfill coverage accordingly — either by enabling additional polyfills that build upon those features, or by disabling polyfills for APIs that are already provided in the compatibility packages. 

Currently, **PolyShim** has integration with the following packages:
- [`System.Diagnostics.Process`](https://nuget.org/packages/System.Diagnostics.Process) — `Process`, `ProcessStartInfo`, etc.
- [`System.Management`](https://nuget.org/packages/System.Management) — `ManagementObjectSearcher`, etc.
- [`System.Memory`](https://nuget.org/packages/System.Memory) — `Memory<T>`, `Span<T>`, etc.
- [`System.Net.Http`](https://nuget.org/packages/System.Net.Http) — `HttpClient`, `HttpContent`, etc.
- [`System.Runtime.InteropServices.RuntimeInformation`](https://nuget.org/packages/System.Runtime.InteropServices.RuntimeInformation) — `RuntimeInformation`, `OSPlatform`, etc.
- [`System.Threading.Tasks`](https://nuget.org/packages/System.Threading.Tasks) — `Task`, `Task<T>`, etc.
- [`System.Threading.Tasks.Extensions`](https://nuget.org/packages/System.Threading.Tasks.Extensions) — `ValueTask`, `ValueTask<T>`, etc.
- [`System.ValueTuple`](https://nuget.org/packages/System.ValueTuple) — `ValueTuple<...>`, etc.
- [`Microsoft.Bcl.Async`](https://nuget.org/packages/Microsoft.Bcl.Async) — `Task`, `Task<T>`, etc (wider support than the `System.*` variant).
- [`Microsoft.Bcl.AsyncInterfaces`](https://nuget.org/packages/Microsoft.Bcl.AsyncInterfaces) — `IAsyncEnumerable<T>`, `IAsyncDisposable`, etc.
- [`Microsoft.Bcl.HashCode`](https://nuget.org/packages/Microsoft.Bcl.HashCode) — `HashCode`, etc.
- [`Microsoft.Net.Http`](https://nuget.org/packages/Microsoft.Net.Http) — `HttpClient`, `HttpContent`, etc (wider support than the `System.*` variant).

For example, adding a reference to the `System.Memory` package will enable **PolyShim**'s polyfills that offer `Span<T>` and `Memory<T>`-based method overloads on various built-in types, such as `Stream`:

```xml
<Project>

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="PolyShim" Version="1.0.0" />
    <PackageReference Include="System.Memory" Version="4.5.5" />
  </ItemGroup>

</Project>
```

```csharp
using System.Buffers;
using System.IO;

using var stream = /* ... */;
using var buffer = MemoryPool<byte>.Shared.Rent();

// On older framworks, this is replaced by a polyfill
var bytesRead = await stream.ReadAsync(buffer.Memory);
```

Conversely, adding a reference to the `System.ValueTuple` package will disable **PolyShim**'s own polyfills for `ValueTuple<...>` and related types.
You can use this approach to prioritize the official implementation where possible, while still relying on the polyfilled version for older target frameworks:

```xml
<Project>

  <PropertyGroup>
    <TargetFramework>netstandard1.6;net35;net50</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="PolyShim" Version="1.0.0" />
    <PackageReference
            Include="System.ValueTuple"
            Version="4.5.0"
            Condition="'$(TargetFramework)' == 'netstandard1.6'" />
  </ItemGroup>

</Project>
```

```csharp
// On .NET 5.0, this will be provided natively
// On .NET Standard 1.6, this will be provided by the System.ValueTuple package
// On .NET Framework 3.5, this will be provided by PolyShim
var (x, y) = ("hello world", 42);
```

### Limitations

Despite best efforts, **PolyShim** is not able to polyfill all the missing APIs due to limitations in the C# language.
At least until some form of the [Extension Everything](https://github.com/dotnet/csharplang/discussions/5498) feature is implemented, below are some of the things that currently cannot be polyfilled:
- Properties
- Indexers
- Interface implementations
- Static members