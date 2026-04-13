#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Globalization;

// https://learn.microsoft.com/dotnet/api/system.globalization.timespanstyles
[Flags]
internal enum TimeSpanStyles
{
    None = 0,
    AssumeNegative = 1,
}
#endif
