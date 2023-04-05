#if !FEATURE_RUNTIMEINFORMATION
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.osplatform
[ExcludeFromCodeCoverage]
internal readonly partial struct OSPlatform : IEquatable<OSPlatform>
{
    private readonly string _name;

    private OSPlatform(string name) => _name = name;

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

    public static OSPlatform FreeBSD { get; } = new("FreeBSD");

    public static OSPlatform Linux { get; } = new("Linux");

    public static OSPlatform OSX { get; } = new("OSX");

    public static OSPlatform Windows { get; } = new("Windows");
}
#endif