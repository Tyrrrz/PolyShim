#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

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

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_Path
{
    extension(Path)
    {
        // Can only detect the platform on .NET Standard 1.3+
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.io.path.ispathfullyqualified#system-io-path-ispathfullyqualified(system-string)
        public static bool IsPathFullyQualified(string path)
        {
            if (!Path.IsPathRooted(path))
                return false;

            // On non-Windows platforms, a rooted path is always fully qualified
            if (!OperatingSystem.IsWindows())
                return true;

            // If the path starts with a directory separator, it's fully qualified only if
            // it's followed by another directory separator (UNC path) or a question mark (extended-length path).
            if (PathEx.IsDirectorySeparator(path[0]))
            {
                return path.Length >= 2 && (PathEx.IsDirectorySeparator(path[1]) || path[1] == '?');
            }

            // If the path starts with a drive letter and a volume separator, it's fully qualified only if
            // it's then followed by a directory separator. Otherwise, it's a drive-relative path.
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
#endif
    }
}
#endif
