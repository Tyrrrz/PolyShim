using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace PolyShim;

[Generator]
internal sealed class PolyShimGenerator : IIncrementalGenerator
{
    // Maps FEATURE_* names to representative types; used only to build the #define prefix
    // prepended to every emitted file so that intra-file #if FEATURE_* guards work correctly
    // (e.g. "#if FEATURE_ASYNCINTERFACES" inside TimeProvider.cs).
    private static readonly IReadOnlyDictionary<string, string> FeatureTypes =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["FEATURE_ARRAYPOOL"] = "System.Buffers.ArrayPool`1",
            ["FEATURE_ASYNCINTERFACES"] = "System.Collections.Generic.IAsyncEnumerable`1",
            ["FEATURE_HASHCODE"] = "System.HashCode",
            ["FEATURE_HTTPCLIENT"] = "System.Net.Http.HttpClient",
            ["FEATURE_INDEXRANGE"] = "System.Index",
            ["FEATURE_MANAGEMENT"] = "System.Management.ManagementObjectSearcher",
            ["FEATURE_MEMORY"] = "System.Memory`1",
            ["FEATURE_PROCESS"] = "System.Diagnostics.Process",
            ["FEATURE_RUNTIMEINFORMATION"] = "System.Runtime.InteropServices.RuntimeInformation",
            ["FEATURE_TASK"] = "System.Threading.Tasks.Task",
            ["FEATURE_VALUETASK"] = "System.Threading.Tasks.ValueTask",
            ["FEATURE_VALUETUPLE"] = "System.ValueTuple",
            ["FEATURE_TIMEPROVIDER"] = "System.TimeProvider",
        };

    // Detects any extension(...) block declaration line within a file
    private static readonly Regex ExtensionBlockRegex = new Regex(
        @"(?m)^\s+extension\s*(?:<[^>]+>)?\s*\(",
        RegexOptions.Compiled);

    // Matches a single extension block declaration line, capturing the inner argument list
    private static readonly Regex ExtensionDeclRegex = new Regex(
        @"^\s+extension\s*(?:<[^>]+>)?\s*\(\s*(.*?)\s*\)\s*\r?$",
        RegexOptions.Compiled);

    // Matches a namespace declaration (file-scoped or block-style); group 1 = namespace name
    private static readonly Regex NamespaceDeclRegex = new Regex(
        @"^namespace\s+([\w.]+)\s*[;{]?\s*\r?$",
        RegexOptions.Compiled);

    // Matches a type declaration line; group 1 = type keyword, group 2 = name, group 3 = generic params
    private static readonly Regex TypeDeclRegex = new Regex(
        @"^\s*(?:internal|public)\s+(?:partial\s+|readonly\s+|ref\s+|abstract\s+|sealed\s+|static\s+)*" +
        @"(class|struct|interface|enum|record)\s+(\w+)(?:<([^>]*)>)?",
        RegexOptions.Compiled);

    // Matches a public method declaration at 8-space indent and captures the method name
    private static readonly Regex PublicMethodRegex = new Regex(
        @"^        public\s+(?:(?:static|async|override|virtual|abstract|new|sealed)\s+)*" +
        @"(?:[\w<>()?.,\[\]\s]+?\s)(\w+)\s*(?:<[^>]*>)?\s*\(",
        RegexOptions.Compiled);

    // Matches a public property declaration at 8-space indent (no '(' on the line)
    private static readonly Regex PublicPropertyRegex = new Regex(
        @"^        public\s+(?:(?:static|override|virtual|abstract|new)\s+)*\S+\s+(\w+)\s*(?:=>|\{|;|$)",
        RegexOptions.Compiled);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, Execute);
    }

    private static void Execute(SourceProductionContext context, Compilation compilation)
    {
        // Build the available-features set for #define prefixes on emitted files
        var availableFeatures = new HashSet<string>(StringComparer.Ordinal);
        foreach (var kvp in FeatureTypes)
        {
            if (compilation.GetTypeByMetadataName(kvp.Value) is not null)
                availableFeatures.Add(kvp.Key);
        }

        bool allowUnsafe = compilation is CSharpCompilation cs && cs.Options.AllowUnsafe;
        if (allowUnsafe)
            availableFeatures.Add("ALLOW_UNSAFE_BLOCKS");

        var sb = new StringBuilder();
        foreach (var feature in availableFeatures.OrderBy(f => f, StringComparer.Ordinal))
            sb.AppendLine($"#define {feature}");
        sb.AppendLine();
        var definePrefix = sb.ToString();

        var assembly = typeof(PolyShimGenerator).Assembly;
        foreach (var resourceName in assembly.GetManifestResourceNames().OrderBy(n => n, StringComparer.Ordinal))
        {
            const string polyfillsPrefix = "PolyShim.Generator.Polyfills.";
            if (!resourceName.StartsWith(polyfillsPrefix, StringComparison.Ordinal))
                continue;

            string content;
            using (var stream = assembly.GetManifestResourceStream(resourceName)!)
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                content = reader.ReadToEnd();

            // Strip UTF-8 BOM if present
            if (content.Length > 0 && content[0] == '\uFEFF')
                content = content.Substring(1);

            if (!ShouldEmitFile(content, compilation))
                continue;

            var baseName = resourceName.Substring(polyfillsPrefix.Length);
            if (baseName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                baseName = baseName.Substring(0, baseName.Length - 3);

            context.AddSource(baseName + ".g.cs", SourceText.From(definePrefix + content, Encoding.UTF8));
        }
    }

    // Determines whether a polyfill file should be emitted by inspecting its content:
    // - Files with extension(...) blocks add members to existing types (member polyfills)
    // - All other files declare types that shadow native BCL types (type polyfills)
    private static bool ShouldEmitFile(string content, Compilation compilation)
    {
        if (ExtensionBlockRegex.IsMatch(content))
            return ShouldEmitMemberPolyfill(content, compilation);

        return ShouldEmitTypePolyfill(content, compilation);
    }

    // For type polyfills: emit the file if any type it declares is absent from the compilation.
    // Uses regex-based parsing so that C# 12+ primary constructor syntax is handled correctly.
    private static bool ShouldEmitTypePolyfill(string content, Compilation compilation)
    {
        string? currentNamespace = null;
        bool foundAnyBclType = false;

        foreach (var rawLine in content.Split('\n'))
        {
            var line = rawLine.TrimEnd('\r');

            // Track namespace changes (file-scoped "namespace X;" or block "namespace X {")
            var nsMatch = NamespaceDeclRegex.Match(line);
            if (nsMatch.Success)
            {
                currentNamespace = nsMatch.Groups[1].Value;
                continue;
            }

            if (currentNamespace is null)
                continue;

            // Look for a type declaration: internal/public [modifiers] class/struct/... Name[<T>]
            var typeMatch = TypeDeclRegex.Match(line);
            if (!typeMatch.Success)
                continue;

            foundAnyBclType = true;
            var name = typeMatch.Groups[2].Value;
            var genericParams = typeMatch.Groups[3].Value;
            var arity = string.IsNullOrEmpty(genericParams) ? 0 : CountDepth0Commas(genericParams) + 1;
            var fullName = arity > 0 ? $"{currentNamespace}.{name}`{arity}" : $"{currentNamespace}.{name}";

            if (compilation.GetTypeByMetadataName(fullName) is null)
                return true; // At least one declared type is missing → emit
        }

        // No namespaced types found (e.g., NamespaceDummies.cs) → always emit
        return !foundAnyBclType;
    }

    // For member polyfills: emit the file if any method/property in any extension block is
    // absent from the target type in the compilation.
    private static bool ShouldEmitMemberPolyfill(string content, Compilation compilation)
    {
        var usingNamespaces = ParseUsingNamespaces(content);
        var lines = content.Split('\n');
        string? currentTargetRaw = null;

        foreach (var line in lines)
        {
            // New extension block declaration — update the current target type
            var extMatch = ExtensionDeclRegex.Match(line);
            if (extMatch.Success)
            {
                currentTargetRaw = extMatch.Groups[1].Value.Trim();
                continue;
            }

            if (currentTargetRaw is null)
                continue;

            if (line.Contains("("))
            {
                // Potential public method declaration
                var methodMatch = PublicMethodRegex.Match(line);
                if (methodMatch.Success)
                {
                    var memberName = methodMatch.Groups[1].Value;
                    var openParen = line.IndexOf('(', methodMatch.Index + methodMatch.Length - 1);
                    var paramCount = CountParamsOnLine(line, openParen);
                    if (IsMemberMissing(currentTargetRaw, usingNamespaces, compilation, memberName, paramCount))
                        return true;
                }
            }
            else if (line.StartsWith("        public ", StringComparison.Ordinal))
            {
                // Potential public property declaration (no '(' on the line)
                var propMatch = PublicPropertyRegex.Match(line);
                if (propMatch.Success)
                {
                    var memberName = propMatch.Groups[1].Value;
                    if (IsMemberMissing(currentTargetRaw, usingNamespaces, compilation, memberName, paramCount: -1))
                        return true;
                }
            }
        }

        return false;
    }

    // Returns true if the named member is absent from the resolved target type.
    // paramCount == -1 means a property check (existence only, no parameter matching).
    private static bool IsMemberMissing(
        string targetRaw,
        IReadOnlyList<string> usingNamespaces,
        Compilation compilation,
        string memberName,
        int paramCount)
    {
        var targetType = ResolveExtensionTarget(targetRaw, usingNamespaces, compilation);

        // If the target type is absent from the compilation, conservatively emit the polyfill
        if (targetType is null)
            return true;

        var members = GetPublicMembersNamed(targetType, memberName).ToList();
        if (members.Count == 0)
            return true; // Member doesn't exist at all

        if (paramCount < 0)
            return false; // Property: presence is sufficient

        // Method: verify at least one overload accepts at least as many parameters as the polyfill.
        // This distinguishes new CT-accepting overloads from pre-existing ones (e.g. ReadLineAsync(CT)
        // vs ReadLineAsync() which existed long before the CT overload).
        return members.All(m => m is IMethodSymbol ms && ms.Parameters.Length < paramCount);
    }

    // Resolves the raw extension target string to the Roslyn type symbol whose members we check.
    // Special cases: array (T[]), string, and ArraySegment<T> extensions add methods that live
    // natively in System.MemoryExtensions, so we check that type instead.
    private static INamedTypeSymbol? ResolveExtensionTarget(
        string rawTarget,
        IReadOnlyList<string> usingNamespaces,
        Compilation compilation)
    {
        var typePart = ExtractTypePart(rawTarget);
        var bare = typePart.TrimEnd('?').Trim();

        // Array extensions (e.g. T[]?) — native methods live in System.MemoryExtensions
        if (bare.EndsWith("[]"))
            return compilation.GetTypeByMetadataName("System.MemoryExtensions");

        // string extensions — native methods live in System.MemoryExtensions
        if (bare == "string")
            return compilation.GetTypeByMetadataName("System.MemoryExtensions");

        var (baseName, arity) = ExtractBaseNameAndArity(bare);

        // ArraySegment<T> extensions — native methods live in System.MemoryExtensions
        if (baseName == "ArraySegment")
            return compilation.GetTypeByMetadataName("System.MemoryExtensions");

        var metadataName = arity > 0 ? $"{baseName}`{arity}" : baseName;

        foreach (var ns in usingNamespaces)
        {
            var sym = compilation.GetTypeByMetadataName($"{ns}.{metadataName}");
            if (sym is not null) return sym;
        }

        return null;
    }

    // Extracts the type portion from an extension target argument:
    //   "HttpClient httpClient" → "HttpClient"
    //   "Task<T> task"         → "Task<T>"
    //   "T[]? array"           → "T[]?"
    //   "Parallel"             → "Parallel"  (static extension, no parameter name)
    private static string ExtractTypePart(string inner)
    {
        if (string.IsNullOrEmpty(inner)) return inner;
        var parts = inner.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length <= 1) return inner;
        var last = parts[parts.Length - 1];
        return Regex.IsMatch(last, @"^\w+$")
            ? string.Join(" ", parts, 0, parts.Length - 1)
            : inner;
    }

    // Splits a type name into base name and generic arity:
    //   "Task<T>"               → ("Task", 1)
    //   "EqualityComparer<T>"   → ("EqualityComparer", 1)
    //   "Parallel"              → ("Parallel", 0)
    private static (string name, int arity) ExtractBaseNameAndArity(string typeName)
    {
        var idx = typeName.IndexOf('<');
        if (idx < 0) return (typeName, 0);

        var baseName = typeName.Substring(0, idx);
        var innerLen = typeName.Length - idx - 2;
        if (innerLen <= 0) return (baseName, 0); // malformed or empty generic params
        var inner = typeName.Substring(idx + 1, innerLen);
        int arity = 1, depth = 0;
        foreach (var c in inner)
        {
            if (c == '<') depth++;
            else if (c == '>') depth--;
            else if (c == ',' && depth == 0) arity++;
        }
        return (baseName, arity);
    }

    // Parses using directives and the file-scoped namespace declaration from file content
    private static List<string> ParseUsingNamespaces(string content)
    {
        var result = new List<string>();
        foreach (Match m in Regex.Matches(content, @"(?m)^\s*using\s+([\w.]+)\s*;"))
            result.Add(m.Groups[1].Value);

        var nsMatch = Regex.Match(content, @"(?m)^namespace\s+([\w.]+)\s*;");
        if (nsMatch.Success)
        {
            var ns = nsMatch.Groups[1].Value;
            if (!result.Contains(ns))
                result.Add(ns);
        }

        return result;
    }

    // Counts parameters visible on the opening line of a method signature.
    // For multi-line signatures only the first line is scanned; when no content follows
    // the opening '(' (count = 0) the check "Parameters.Length >= 0" is trivially true, so
    // emission falls back to a plain name-existence check.
    private static int CountParamsOnLine(string line, int openParenIdx)
    {
        if (openParenIdx < 0 || openParenIdx >= line.Length) return 0;
        var rest = line.Substring(openParenIdx + 1);
        var restTrimmed = rest.TrimStart();
        if (restTrimmed.Length == 0 || restTrimmed.StartsWith(")")) return 0;

        int commas = 0, depth = 0;
        foreach (var c in rest)
        {
            if (c == ')' && depth == 0) break;
            if (c == '(' || c == '<' || c == '[') depth++;
            else if (c == ')' || c == '>' || c == ']') depth--;
            else if (c == ',' && depth == 0) commas++;
        }
        return commas + 1;
    }

    // Counts commas at nesting depth 0 in a string (used to compute generic arity)
    private static int CountDepth0Commas(string s)
    {
        int commas = 0, depth = 0;
        foreach (var c in s)
        {
            if (c == '<' || c == '(') depth++;
            else if (c == '>' || c == ')') depth--;
            else if (c == ',' && depth == 0) commas++;
        }
        return commas;
    }

    // Enumerates all public members with the given name from the type and its base types
    private static IEnumerable<ISymbol> GetPublicMembersNamed(INamedTypeSymbol type, string name)
    {
        var current = (INamedTypeSymbol?)type;
        while (current is not null)
        {
            foreach (var m in current.GetMembers(name))
                if (m.DeclaredAccessibility == Accessibility.Public)
                    yield return m;
            current = current.BaseType;
        }
    }
}
