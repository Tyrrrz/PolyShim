#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.IO;

internal static partial class PolyfillExtensions
{
    extension(Path)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.path.endsindirectoryseparator#system-io-path-endsindirectoryseparator(system-string)
        public static bool EndsInDirectorySeparator(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var lastChar = path![^1];

#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
            return lastChar == Path.DirectorySeparatorChar
                || lastChar == Path.AltDirectorySeparatorChar;
#else
            return lastChar is '\\' or '/';
#endif
        }

        // https://learn.microsoft.com/dotnet/api/system.io.path.trimendingdirectoryseparator#system-io-path-trimendingdirectoryseparator(system-string)
        public static string TrimEndingDirectorySeparator(string path) =>
            EndsInDirectorySeparator(path)
            && !Path.GetPathRoot(path).Equals(path, System.StringComparison.Ordinal)
                ? path[..^1]
                : path;
    }
}
#endif
