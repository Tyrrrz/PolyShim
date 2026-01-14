#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.IO;
using System.Linq;
using System.Text;

file static class PathEx
{
    public static bool IsDirectorySeparator(char c) =>
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
        c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
#else
        c is '\\' or '/';
#endif
}

internal static partial class PolyfillExtensions
{
    extension(Path)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.path.endsindirectoryseparator#system-io-path-endsindirectoryseparator(system-string)
        public static bool EndsInDirectorySeparator(string? path) =>
            !string.IsNullOrEmpty(path) && PathEx.IsDirectorySeparator(path![^1]);

        // https://learn.microsoft.com/dotnet/api/system.io.path.trimendingdirectoryseparator#system-io-path-trimendingdirectoryseparator(system-string)
        public static string TrimEndingDirectorySeparator(string path) =>
            EndsInDirectorySeparator(path)
            && !Path.GetPathRoot(path).Equals(path, System.StringComparison.Ordinal)
                ? path[..^1]
                : path;

        // https://learn.microsoft.com/dotnet/api/system.io.path.join#system-io-path-join(system-string())
        public static string Join(params string?[] paths)
        {
            if (paths.Length <= 0)
                return string.Empty;

            var maxSize = paths.Sum(path => path?.Length ?? 0);
            maxSize += paths.Length - 1;

            var builder = new StringBuilder(maxSize);

            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path))
                    continue;

                if (builder.Length == 0)
                {
                    builder.Append(path);
                }
                else
                {
                    if (
                        !PathEx.IsDirectorySeparator(builder[^1])
                        && !PathEx.IsDirectorySeparator(path![0])
                    )
                    {
                        builder.Append(
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
                            Path.DirectorySeparatorChar
#else
                            '/'
#endif
                        );
                    }

                    builder.Append(path);
                }
            }

            return builder.ToString();
        }
    }
}
#endif
