#if (NETFRAMEWORK && !NET35_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.extensionattribute
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
[ExcludeFromCodeCoverage]
internal class ExtensionAttribute : Attribute { }
#endif
