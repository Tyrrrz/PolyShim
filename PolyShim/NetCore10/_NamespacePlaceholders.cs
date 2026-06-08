// #ignore-signatures

// Define placeholder types for several built-in namespaces to allow using declarations
// even on frameworks that don't have any types in those namespaces.
// This lets us write using declarations without worrying about #if guards.

#if (NETCOREAPP) || (NETSTANDARD)
namespace System.Management
{
    internal static class __Placeholder;
}
#endif

#if (NETFRAMEWORK && !NETFRAMEWORK4_5_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_1_OR_GREATER)
namespace System.Net.Http
{
    internal static class __Placeholder;
}
#endif

#if (NETFRAMEWORK && !NETFRAMEWORK4_0_OR_GREATER)
namespace System.Threading.Tasks
{
    internal static class __Placeholder;
}
#endif
