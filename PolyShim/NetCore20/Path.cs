#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.IO;
using System.Linq;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_Path
{
    extension(Path)
    {
        // Can only detect the platform on .NET Standard 1.3+
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.io.path.getrelativepath
        public static string GetRelativePath(string relativeTo, string path)
        {
            var pathStringComparison = OperatingSystem.IsWindows()
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            var basePathSegments = relativeTo
                .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();

            var pathSegments = path.Split(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar
                )
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();

            var commonSegmentsCount = 0;
            for (var i = 0; i < basePathSegments.Length && i < pathSegments.Length; i++)
            {
                if (!string.Equals(basePathSegments[i], pathSegments[i], pathStringComparison))
                    break;

                commonSegmentsCount++;
            }

            // All segments are common, return "."
            if (
                commonSegmentsCount == basePathSegments.Length
                && commonSegmentsCount == pathSegments.Length
            )
            {
                return ".";
            }

            // No segments are common and the first segment is a disk label, return the original path
            if (
                commonSegmentsCount == 0
                && pathSegments.Length > 0
                && pathSegments[0].EndsWith(":", StringComparison.Ordinal)
            )
            {
                return path;
            }

            // Some segments are common, build the relative path
            return string.Join(
                Path.DirectorySeparatorChar.ToString(),
                Enumerable
                    .Repeat("..", basePathSegments.Length - commonSegmentsCount)
                    .Concat(pathSegments.Skip(commonSegmentsCount))
                    .ToArray()
            );
        }
#endif
    }
}
#endif
