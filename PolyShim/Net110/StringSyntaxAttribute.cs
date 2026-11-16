#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_StringSyntaxAttribute
{
    extension(StringSyntaxAttribute)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.stringsyntaxattribute.csharp
        public static string CSharp => "C#";

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.stringsyntaxattribute.fsharp
        public static string FSharp => "F#";

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.stringsyntaxattribute.visualbasic
        public static string VisualBasic => "Visual Basic";
    }
}
#endif
