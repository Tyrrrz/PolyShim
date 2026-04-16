#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

// No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.IO;

// https://learn.microsoft.com/dotnet/api/system.io.filestreamoptions
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class FileStreamOptions
{
    public FileMode Mode { get; set; } = FileMode.Open;

    public FileAccess Access { get; set; } = FileAccess.Read;

    public FileShare Share { get; set; } = FileShare.Read;

    private int _bufferSize = 4096;

    public int BufferSize
    {
        get => _bufferSize;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _bufferSize = value;
        }
    }

    public FileOptions Options { get; set; } = FileOptions.None;

    private long _preallocationSize;

    public long PreallocationSize
    {
        get => _preallocationSize;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _preallocationSize = value;
        }
    }

    public UnixFileMode? UnixCreateMode { get; set; }
}

#endif
#endif
