#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.IO;

file static class PathEx
{
    public static bool IsDirectorySeparator(char c) =>
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
        c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
#else
        c is '\\' or '/';
#endif

    public static bool IsVolumeSeparator(char c) =>
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
        c == Path.VolumeSeparatorChar;
#else
        c == ':';
#endif
}

internal static partial class PolyfillExtensions
{
    extension(Path)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.path.ispathfullyqualified#system-io-path-ispathfullyqualified(system-string)
        public static bool IsPathFullyQualified(string path)
        {
            if (!Path.IsPathRooted(path))
                return false;

            // On Windows, a path starting with a directory separator is fully qualified only if
            // it's followed by another directory separator (UNC path) or a '?' (extended-length path).
#if NETSTANDARD && !NETSTANDARD1_3_OR_GREATER
            if (path[0] == '\\')
#else
            if (OperatingSystem.IsWindows() && PathEx.IsDirectorySeparator(path[0]))
#endif
            {
                return path.Length >= 2 && (PathEx.IsDirectorySeparator(path[1]) || path[1] == '?');
            }

            // On Windows, a path starting with a drive letter is fully qualified only if
            // it's followed by a volume separator and a directory separator.
            if (
                path.Length >= 3
                && char.IsLetter(path[0])
                && PathEx.IsVolumeSeparator(path[1])
                && PathEx.IsDirectorySeparator(path[2])
            )
            {
                return true;
            }

            return false;
        }
    }
}
#endif
