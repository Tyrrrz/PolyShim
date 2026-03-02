// Compiler-support polyfills for C# language features not available in netstandard2.0.
// These are consumed by the C# compiler itself and are not part of the emitted polyfill API.

#pragma warning disable CS0436

namespace System.Runtime.CompilerServices
{
    // Required for C# 9 'init' property accessors.
    internal static class IsExternalInit { }

    // Required for C# 11 'required' member modifier.
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = false)]
    internal sealed class RequiredMemberAttribute : Attribute { }

    // Required for C# 11 'required' member modifier.
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor,
        AllowMultiple = true,
        Inherited = false)]
    internal sealed class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string featureName)
        {
            FeatureName = featureName;
        }

        public string FeatureName { get; }
        public bool IsOptional { get; init; }
    }
}
