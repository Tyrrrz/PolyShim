#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.dynamicallyaccessedmembertypes
[Flags]
public enum DynamicallyAccessedMemberTypes
{
    All = -1,
    None = 0,
    PublicParameterlessConstructor = 1,
    PublicConstructors = 3,
    NonPublicConstructors = 4,
    PublicMethods = 8,
    NonPublicMethods = 16,
    PublicFields = 32,
    NonPublicFields = 64,
    PublicNestedTypes = 128,
    NonPublicNestedTypes = 256,
    PublicProperties = 512,
    NonPublicProperties = 1024,
    PublicEvents = 2048,
    NonPublicEvents = 4096,
    Interfaces = 8192,
}
#endif
