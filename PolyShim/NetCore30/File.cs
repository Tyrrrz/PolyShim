#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.IO;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_File
{
    // No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
    extension(File)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.file.move#system-io-file-move(system-string-system-string-system-boolean)
        public static void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
            File.Delete(sourceFileName);
        }
    }
#endif
}
#endif
