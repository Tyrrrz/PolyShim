// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

// Define dummy classes for several built-in namespaces to allow using declarations
// even on frameworks that don't have the corresponding types.
// This lets us write using declarations without worrying about #if guards.

namespace System.Net.Http
{
    internal static class __Dummy { }
}

namespace System.Threading.Tasks
{
    internal static class __Dummy { }
}