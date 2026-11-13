#if !FEATURE_MEMORY
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.sequenceposition
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct SequencePosition(object? @object, int integer)
    : IEquatable<SequencePosition>
{
    private readonly object? _object = @object;
    private readonly int _integer = integer;

    public object? GetObject() => _object;

    public int GetInteger() => _integer;

    public override bool Equals(object? obj) => obj is SequencePosition other && Equals(other);

    public bool Equals(SequencePosition other) =>
        ReferenceEquals(_object, other._object) && _integer == other._integer;

    public override int GetHashCode() => HashCode.Combine(_object, _integer);

    public static bool operator ==(SequencePosition left, SequencePosition right) =>
        left.Equals(right);

    public static bool operator !=(SequencePosition left, SequencePosition right) =>
        !left.Equals(right);
}
#endif
