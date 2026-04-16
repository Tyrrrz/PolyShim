#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

// No file I/O on .NET Standard prior to 1.3, and ConditionalWeakTable unavailable on .NET Framework 3.5
#if (!NETSTANDARD || NETSTANDARD1_3_OR_GREATER) && (!NETFRAMEWORK || NET40_OR_GREATER)

using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_FileStreamOptions
{
    private sealed class UnixFileModeBox
    {
        public UnixFileMode? Value;
    }

    private static readonly ConditionalWeakTable<
        FileStreamOptions,
        UnixFileModeBox
    > _unixCreateModes = new();

    extension(FileStreamOptions options)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.filestreamoptions.unixcreatemode
        public UnixFileMode? UnixCreateMode
        {
            get => _unixCreateModes.TryGetValue(options, out var box) ? box.Value : null;
            set => _unixCreateModes.GetOrCreateValue(options).Value = value;
        }
    }
}

#endif
#endif
