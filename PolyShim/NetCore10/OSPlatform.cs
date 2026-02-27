#if !FEATURE_RUNTIMEINFORMATION
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.osplatform
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly partial struct OSPlatform(string name) : IEquatable<OSPlatform>
{
    private readonly string _name = name;

    public override bool Equals(object? obj) => obj is OSPlatform other && Equals(other);

    public bool Equals(OSPlatform other) => _name == other._name;

    public override int GetHashCode() => _name.GetHashCode();

    public override string ToString() => _name;

    public static bool operator ==(OSPlatform left, OSPlatform right) => left.Equals(right);

    public static bool operator !=(OSPlatform left, OSPlatform right) => !left.Equals(right);
}

internal partial struct OSPlatform
{
    public static OSPlatform Create(string osPlatform) => new(osPlatform);

    public static OSPlatform Linux { get; } = new("Linux");

    public static OSPlatform OSX { get; } = new("OSX");

    public static OSPlatform Windows { get; } = new("Windows");
}
#endif
