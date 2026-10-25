#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK && !NETFRAMEWORK2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

namespace System;

// https://learn.microsoft.com/dotnet/api/system.base64formattingoptions
[Flags]
internal enum Base64FormattingOptions
{
    None = 0,
    InsertLineBreaks = 1,
}
#endif
