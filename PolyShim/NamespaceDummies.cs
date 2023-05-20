// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

// Define dummy classes for several built-in namespaces to allow using declarations
// even on frameworks that don't have the corresponding types.
// This lets us write using declarations without worrying about #if guards.

namespace System.IO
{
    internal static partial class PolyfillExtensions { }
}

namespace System.Management
{
    internal static partial class PolyfillExtensions { }
}

namespace System.Net.Http
{
    internal static partial class PolyfillExtensions { }
}

namespace System.Threading.Tasks
{
    internal static partial class PolyfillExtensions { }
}