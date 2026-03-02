using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PolyShim;

[Generator]
internal sealed class PolyShimGenerator : IIncrementalGenerator
{
    // Strongly-typed feature flags for the current compilation.
    // Each property name matches the preprocessor symbol evaluated by ApplyFeatureConditions.
    private sealed class PolyfillFeatures
    {
        public required bool ALLOW_UNSAFE_BLOCKS { get; init; }
        public required bool FEATURE_ARRAYPOOL { get; init; }
        public required bool FEATURE_ASYNCINTERFACES { get; init; }
        public required bool FEATURE_HASHCODE { get; init; }
        public required bool FEATURE_HTTPCLIENT { get; init; }
        public required bool FEATURE_INDEXRANGE { get; init; }
        public required bool FEATURE_MANAGEMENT { get; init; }
        public required bool FEATURE_MEMORY { get; init; }
        public required bool FEATURE_PROCESS { get; init; }
        public required bool FEATURE_RUNTIMEINFORMATION { get; init; }
        public required bool FEATURE_TASK { get; init; }
        public required bool FEATURE_VALUETASK { get; init; }
        public required bool FEATURE_VALUETUPLE { get; init; }
        public required bool FEATURE_TIMEPROVIDER { get; init; }

        // Returns true when the given preprocessor symbol name is a generator-controlled feature.
        public bool Contains(string name) => name is
            nameof(ALLOW_UNSAFE_BLOCKS) or
            nameof(FEATURE_ARRAYPOOL) or
            nameof(FEATURE_ASYNCINTERFACES) or
            nameof(FEATURE_HASHCODE) or
            nameof(FEATURE_HTTPCLIENT) or
            nameof(FEATURE_INDEXRANGE) or
            nameof(FEATURE_MANAGEMENT) or
            nameof(FEATURE_MEMORY) or
            nameof(FEATURE_PROCESS) or
            nameof(FEATURE_RUNTIMEINFORMATION) or
            nameof(FEATURE_TASK) or
            nameof(FEATURE_VALUETASK) or
            nameof(FEATURE_VALUETUPLE) or
            nameof(FEATURE_TIMEPROVIDER);

        // Returns the value of the feature flag identified by preprocessor symbol name.
        public bool this[string name] => name switch
        {
            nameof(ALLOW_UNSAFE_BLOCKS) => ALLOW_UNSAFE_BLOCKS,
            nameof(FEATURE_ARRAYPOOL) => FEATURE_ARRAYPOOL,
            nameof(FEATURE_ASYNCINTERFACES) => FEATURE_ASYNCINTERFACES,
            nameof(FEATURE_HASHCODE) => FEATURE_HASHCODE,
            nameof(FEATURE_HTTPCLIENT) => FEATURE_HTTPCLIENT,
            nameof(FEATURE_INDEXRANGE) => FEATURE_INDEXRANGE,
            nameof(FEATURE_MANAGEMENT) => FEATURE_MANAGEMENT,
            nameof(FEATURE_MEMORY) => FEATURE_MEMORY,
            nameof(FEATURE_PROCESS) => FEATURE_PROCESS,
            nameof(FEATURE_RUNTIMEINFORMATION) => FEATURE_RUNTIMEINFORMATION,
            nameof(FEATURE_TASK) => FEATURE_TASK,
            nameof(FEATURE_VALUETASK) => FEATURE_VALUETASK,
            nameof(FEATURE_VALUETUPLE) => FEATURE_VALUETUPLE,
            nameof(FEATURE_TIMEPROVIDER) => FEATURE_TIMEPROVIDER,
            _ => false,
        };
    }

    // Computes feature flags for the current compilation by querying type availability.
    // These are used by ApplyFeatureConditions to evaluate and strip #if FEATURE_* /
    // #if !FEATURE_* / #if ALLOW_UNSAFE_BLOCKS directives from polyfill files.
    private static PolyfillFeatures ComputeFeatures(Compilation compilation)
    {
        bool allowUnsafe = compilation is CSharpCompilation cs && cs.Options.AllowUnsafe;
        return new PolyfillFeatures
        {
            ALLOW_UNSAFE_BLOCKS = allowUnsafe,
            FEATURE_ARRAYPOOL = compilation.GetTypeByMetadataName("System.Buffers.ArrayPool`1") is not null,
            FEATURE_ASYNCINTERFACES = compilation.GetTypeByMetadataName("System.Collections.Generic.IAsyncEnumerable`1") is not null,
            FEATURE_HASHCODE = compilation.GetTypeByMetadataName("System.HashCode") is not null,
            FEATURE_HTTPCLIENT = compilation.GetTypeByMetadataName("System.Net.Http.HttpClient") is not null,
            FEATURE_INDEXRANGE = compilation.GetTypeByMetadataName("System.Index") is not null,
            FEATURE_MANAGEMENT = compilation.GetTypeByMetadataName("System.Management.ManagementObjectSearcher") is not null,
            FEATURE_MEMORY = compilation.GetTypeByMetadataName("System.Memory`1") is not null,
            FEATURE_PROCESS = compilation.GetTypeByMetadataName("System.Diagnostics.Process") is not null,
            FEATURE_RUNTIMEINFORMATION = compilation.GetTypeByMetadataName("System.Runtime.InteropServices.RuntimeInformation") is not null,
            FEATURE_TASK = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task") is not null,
            FEATURE_VALUETASK = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask") is not null,
            FEATURE_VALUETUPLE = compilation.GetTypeByMetadataName("System.ValueTuple") is not null,
            FEATURE_TIMEPROVIDER = compilation.GetTypeByMetadataName("System.TimeProvider") is not null,
        };
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, Execute);
    }

    private static void Execute(SourceProductionContext context, Compilation compilation)
    {
        var features = ComputeFeatures(compilation);

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

            context.AddSource(baseName + ".g.cs", SourceText.From(ApplyFeatureConditions(content, features), Encoding.UTF8));
        }
    }

    // Evaluates all generator-controlled #if conditions (FEATURE_* and ALLOW_UNSAFE_BLOCKS) in
    // the file content and returns the source with those directive lines stripped and inactive
    // blocks removed.  Non-generator directives (#if NETCOREAPP, #if !POLYFILL_COVERAGE, etc.)
    // are left intact.
    private static string ApplyFeatureConditions(string content, PolyfillFeatures features)
    {
        content = content.Replace("\r\n", "\n").Replace("\r", "\n");
        var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);
        var tree = CSharpSyntaxTree.ParseText(content, parseOptions);

        // Map line number → directive syntax node (one directive per line in practice)
        var directiveAtLine = new Dictionary<int, DirectiveTriviaSyntax>();
        foreach (var trivia in tree.GetRoot().DescendantTrivia(descendIntoTrivia: true))
        {
            if (!trivia.IsDirective) continue;
            if (trivia.GetStructure() is not DirectiveTriviaSyntax dir) continue;
            directiveAtLine[tree.GetLineSpan(trivia.Span).StartLinePosition.Line] = dir;
        }

        var lines = content.Split('\n');
        var sb = new StringBuilder(content.Length);

        // Stack: (isFeature, result)
        //   isFeature=true  → this #if uses generator-controlled conditions
        //   result=false    → this branch contributed +1 to excludeDepth
        var stack = new Stack<(bool isFeature, bool result)>();
        // Number of false generator #if blocks we're currently inside
        int excludeDepth = 0;

        for (var i = 0; i < lines.Length; i++)
        {
            if (!directiveAtLine.TryGetValue(i, out var directive))
            {
                // Regular content line: emit unless we're inside a false feature block
                if (excludeDepth == 0)
                    sb.Append(lines[i]).Append('\n');
            }
            else if (directive is IfDirectiveTriviaSyntax ifDir && IsFeatureCondition(ifDir.Condition, features))
            {
                // Generator-controlled #if: evaluate (always false when already excluding)
                var result = excludeDepth == 0 && EvaluateFeatureCondition(ifDir.Condition, features);
                stack.Push((true, result));
                if (!result)
                    excludeDepth++;
                // Always suppress the directive line itself
            }
            else if (directive is EndIfDirectiveTriviaSyntax)
            {
                if (stack.Count > 0)
                {
                    var (isFeature, result) = stack.Pop();
                    if (isFeature)
                    {
                        if (!result)
                            excludeDepth--;
                        // Suppress the #endif for generator-controlled blocks
                    }
                    else
                    {
                        // Non-generator #endif: emit unless we're in an excluded zone
                        if (excludeDepth == 0)
                            sb.Append(lines[i]).Append('\n');
                    }
                }
                else
                {
                    // Orphaned #endif (unbalanced): treat as pass-through
                    if (excludeDepth == 0)
                        sb.Append(lines[i]).Append('\n');
                }
            }
            else if (directive is IfDirectiveTriviaSyntax)
            {
                // Non-generator #if: push to stack and pass through.
                // Its #elif / #else / #endif are handled by the catch-all below (which emits
                // them unchanged), preserving the directive block for the C# compiler.
                stack.Push((false, false));
                if (excludeDepth == 0)
                    sb.Append(lines[i]).Append('\n');
            }
            else
            {
                // All other directives (#elif, #else for non-generator blocks, #pragma,
                // #nullable, etc.): pass through unchanged if not inside an excluded zone.
                if (excludeDepth == 0)
                    sb.Append(lines[i]).Append('\n');
            }
        }

        return sb.ToString();
    }

    // Returns true if the condition expression consists solely of generator-controlled feature
    // identifiers (all names present in PolyfillFeatures) and logical operators.
    private static bool IsFeatureCondition(ExpressionSyntax condition, PolyfillFeatures features)
    {
        if (condition is IdentifierNameSyntax ident)
            return features.Contains(ident.Identifier.Text);
        if (condition is PrefixUnaryExpressionSyntax prefix && prefix.IsKind(SyntaxKind.LogicalNotExpression))
            return IsFeatureCondition(prefix.Operand, features);
        if (condition is BinaryExpressionSyntax binary &&
            (binary.IsKind(SyntaxKind.LogicalAndExpression) || binary.IsKind(SyntaxKind.LogicalOrExpression)))
            return IsFeatureCondition(binary.Left, features) && IsFeatureCondition(binary.Right, features);
        return false;
    }

    // Evaluates a generator-controlled condition expression against the feature flags.
    private static bool EvaluateFeatureCondition(ExpressionSyntax condition, PolyfillFeatures features)
    {
        if (condition is IdentifierNameSyntax ident)
            return features[ident.Identifier.Text];
        if (condition is PrefixUnaryExpressionSyntax prefix && prefix.IsKind(SyntaxKind.LogicalNotExpression))
            return !EvaluateFeatureCondition(prefix.Operand, features);
        if (condition is BinaryExpressionSyntax binary)
        {
            if (binary.IsKind(SyntaxKind.LogicalAndExpression))
                return EvaluateFeatureCondition(binary.Left, features) && EvaluateFeatureCondition(binary.Right, features);
            if (binary.IsKind(SyntaxKind.LogicalOrExpression))
                return EvaluateFeatureCondition(binary.Left, features) || EvaluateFeatureCondition(binary.Right, features);
        }
        return false;
    }

    // Determines whether a polyfill file should be emitted by inspecting its content:
    // - Files with extension(...) blocks add members to existing types (member polyfills)
    // - All other files declare types that shadow native BCL types (type polyfills)
    private static bool ShouldEmitFile(string content, Compilation compilation)
    {
        var root = ParseStrippingDirectives(content);

        if (root.DescendantNodes().OfType<ExtensionBlockDeclarationSyntax>().Any())
            return ShouldEmitMemberPolyfill(root, compilation);

        return ShouldEmitTypePolyfill(root, compilation);
    }

    // Parses file content after blanking out all preprocessor directive lines so that extension
    // blocks and type declarations inside #if bodies are always visible in the resulting syntax
    // tree, regardless of which platform guard (#if NETFRAMEWORK, etc.) wraps them.
    // Directive line positions are located via a first parse of the file's directive trivia
    // (IfDirectiveTriviaSyntax, EndIfDirectiveTriviaSyntax, etc.) to avoid any string-based
    // pattern matching.
    private static SyntaxNode ParseStrippingDirectives(string content)
    {
        var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);

        // Normalize line endings so both the split and trivia line numbers stay consistent.
        // String.ReplaceLineEndings("\n") is .NET 6+; handle \r\n and lone \r manually here.
        content = content.Replace("\r\n", "\n").Replace("\r", "\n");

        // First parse: discover the line numbers of all directive trivia nodes.
        var firstTree = CSharpSyntaxTree.ParseText(content, parseOptions);
        var directiveLines = new HashSet<int>();
        foreach (var trivia in firstTree.GetRoot().DescendantTrivia(descendIntoTrivia: true))
        {
            if (!trivia.IsDirective) continue;
            var span = firstTree.GetLineSpan(trivia.Span);
            for (var l = span.StartLinePosition.Line; l <= span.EndLinePosition.Line; l++)
                directiveLines.Add(l);
        }

        // Second parse: re-parse with directive lines replaced by blank lines so that the
        // conditional content is always parsed as ordinary syntax.
        var lines = content.Split('\n');
        var sb = new StringBuilder(content.Length);
        for (var i = 0; i < lines.Length; i++)
        {
            if (directiveLines.Contains(i))
                sb.AppendLine();
            else
                sb.Append(lines[i]).Append('\n');
        }
        return CSharpSyntaxTree.ParseText(sb.ToString(), parseOptions).GetRoot();
    }

    // For type polyfills: emit the file if any type it declares is absent from the compilation.
    // Uses Roslyn's own syntax parser so all valid C# type declarations are handled correctly.
    private static bool ShouldEmitTypePolyfill(SyntaxNode root, Compilation compilation)
    {
        bool foundAnyBclType = false;

        foreach (var node in root.DescendantNodes())
        {
            if (node is not BaseTypeDeclarationSyntax typeDecl)
                continue;

            // All PolyShim type polyfills declare their types inside a named namespace.
            // Skip any type not directly inside a namespace (shouldn't happen in practice).
            if (typeDecl.Parent is not BaseNamespaceDeclarationSyntax nsDecl)
                continue;

            foundAnyBclType = true;
            var ns = nsDecl.Name.ToString();
            var name = typeDecl.Identifier.Text;
            // Enums cannot be generic in C#, so the null-coalesce to 0 is always correct.
            var arity = (typeDecl as TypeDeclarationSyntax)?.TypeParameterList?.Parameters.Count ?? 0;
            var fullName = arity > 0 ? $"{ns}.{name}`{arity}" : $"{ns}.{name}";

            if (compilation.GetTypeByMetadataName(fullName) is null)
                return true; // At least one declared type is missing → emit
        }

        // No namespaced types found (e.g., NamespaceDummies.cs) → always emit
        return !foundAnyBclType;
    }

    // For member polyfills: emit the file if any method/property in any extension block is
    // absent from the target type in the compilation.
    private static bool ShouldEmitMemberPolyfill(SyntaxNode root, Compilation compilation)
    {
        var usingNamespaces = CollectUsingNamespaces(root);

        foreach (var ext in root.DescendantNodes().OfType<ExtensionBlockDeclarationSyntax>())
        {
            var param0 = ext.ParameterList?.Parameters.FirstOrDefault();
            if (param0 is null) continue;

            var targetType = ResolveExtensionTarget(param0.Type, usingNamespaces, compilation);

            foreach (var member in ext.Members)
            {
                if (member is MethodDeclarationSyntax meth &&
                    meth.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                {
                    if (IsMemberMissing(targetType, meth.Identifier.Text, meth.ParameterList.Parameters.Count))
                        return true;
                }
                else if (member is PropertyDeclarationSyntax prop &&
                         prop.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                {
                    if (IsMemberMissing(targetType, prop.Identifier.Text, paramCount: -1))
                        return true;
                }
            }
        }

        return false;
    }

    // Returns true if the named member is absent from the resolved target type.
    // paramCount == -1 means a property check (existence only, no parameter matching).
    private static bool IsMemberMissing(INamedTypeSymbol? targetType, string memberName, int paramCount)
    {
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

    // Resolves the extension parameter's TypeSyntax to the Roslyn type symbol whose members we check.
    // Special cases: array (T[]), string, and ArraySegment<T> extensions add methods that live
    // natively in System.MemoryExtensions, so we check that type instead.
    private static INamedTypeSymbol? ResolveExtensionTarget(
        TypeSyntax? typeSyntax,
        IReadOnlyList<string> usingNamespaces,
        Compilation compilation)
    {
        if (typeSyntax is null)
            return null;

        // Strip nullability: T? → T
        if (typeSyntax is NullableTypeSyntax nullable)
            typeSyntax = nullable.ElementType;

        // Array type (T[]): span-related members live in System.MemoryExtensions
        if (typeSyntax is ArrayTypeSyntax)
            return compilation.GetTypeByMetadataName("System.MemoryExtensions");

        // Predefined keyword type (string, byte, int, etc.)
        if (typeSyntax is PredefinedTypeSyntax pre)
        {
            // string: span/memory members live in System.MemoryExtensions
            if (pre.Keyword.IsKind(SyntaxKind.StringKeyword))
                return compilation.GetTypeByMetadataName("System.MemoryExtensions");

            // Other predefined types (byte, int, etc.): conservatively emit
            return null;
        }

        // Simple name: IdentifierNameSyntax (e.g., "Path", "Random", "HttpClient")
        if (typeSyntax is IdentifierNameSyntax ident)
            return ResolveTypeName(ident.Identifier.Text, arity: 0, usingNamespaces, compilation);

        // Generic name: GenericNameSyntax (e.g., "IEnumerable<T>", "Task<T>", "ArraySegment<T>")
        if (typeSyntax is GenericNameSyntax generic)
        {
            // ArraySegment<T>: span-related members live in System.MemoryExtensions
            if (generic.Identifier.Text == "ArraySegment")
                return compilation.GetTypeByMetadataName("System.MemoryExtensions");

            return ResolveTypeName(
                generic.Identifier.Text,
                generic.TypeArgumentList.Arguments.Count,
                usingNamespaces,
                compilation);
        }

        // Fall back: return null for any unhandled type form (conservative — emit the polyfill).
        // Polyfill extension parameters never use qualified names or other exotic type forms.
        return null;
    }

    // Resolves a simple or generic type name to a symbol by trying each using namespace in order.
    private static INamedTypeSymbol? ResolveTypeName(
        string baseName,
        int arity,
        IReadOnlyList<string> usingNamespaces,
        Compilation compilation)
    {
        var metadataName = arity > 0 ? $"{baseName}`{arity}" : baseName;
        foreach (var ns in usingNamespaces)
        {
            var sym = compilation.GetTypeByMetadataName($"{ns}.{metadataName}");
            if (sym is not null) return sym;
        }
        return null;
    }

    // Collects all non-alias, non-static using namespaces plus the file-scoped namespace (if any).
    private static List<string> CollectUsingNamespaces(SyntaxNode root)
    {
        var result = new List<string>();

        foreach (var u in root.DescendantNodes().OfType<UsingDirectiveSyntax>())
        {
            if (u.StaticKeyword.IsKind(SyntaxKind.StaticKeyword)) continue;
            if (u.Alias is not null) continue;
            var name = u.Name?.ToString();
            if (name is not null && !result.Contains(name))
                result.Add(name);
        }

        var fileNs = root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
        if (fileNs is not null)
        {
            var name = fileNs.Name.ToString();
            if (!result.Contains(name))
                result.Add(name);
        }

        return result;
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
