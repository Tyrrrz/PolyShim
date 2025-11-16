// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

// Define dummy types for several built-in namespaces to allow using declarations
// even on frameworks that don't have any types in those namespaces.
// This lets us write using declarations without worrying about #if guards.

namespace System.IO
{
    internal static partial class __Dummy;
}

namespace System.Management
{
    internal static partial class __Dummy;
}

namespace System.Net.Http
{
    internal static partial class __Dummy;
}

namespace System.Threading.Tasks
{
    internal static partial class __Dummy;
}
