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
    }

    // Computes feature flags for the current compilation by querying type availability.
    // The resulting booleans are merged into definedSymbols so that #if FEATURE_* and
    // #if ALLOW_UNSAFE_BLOCKS directives in polyfill files are evaluated correctly.
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
        // All source files in a C# project share the same preprocessor symbols (they are
        // project-level settings), so reading from the first tree is sufficient.
        var definedSymbols = new HashSet<string>(
            (compilation.SyntaxTrees.FirstOrDefault()?.Options as CSharpParseOptions)?
                .PreprocessorSymbolNames ?? Array.Empty<string>()
        );
        // Merge generator-computed feature flags into definedSymbols so that #if FEATURE_*
        // and #if ALLOW_UNSAFE_BLOCKS conditions are evaluated alongside TFM symbols.
        if (features.ALLOW_UNSAFE_BLOCKS) definedSymbols.Add(nameof(features.ALLOW_UNSAFE_BLOCKS));
        if (features.FEATURE_ARRAYPOOL) definedSymbols.Add(nameof(features.FEATURE_ARRAYPOOL));
        if (features.FEATURE_ASYNCINTERFACES) definedSymbols.Add(nameof(features.FEATURE_ASYNCINTERFACES));
        if (features.FEATURE_HASHCODE) definedSymbols.Add(nameof(features.FEATURE_HASHCODE));
        if (features.FEATURE_HTTPCLIENT) definedSymbols.Add(nameof(features.FEATURE_HTTPCLIENT));
        if (features.FEATURE_INDEXRANGE) definedSymbols.Add(nameof(features.FEATURE_INDEXRANGE));
        if (features.FEATURE_MANAGEMENT) definedSymbols.Add(nameof(features.FEATURE_MANAGEMENT));
        if (features.FEATURE_MEMORY) definedSymbols.Add(nameof(features.FEATURE_MEMORY));
        if (features.FEATURE_PROCESS) definedSymbols.Add(nameof(features.FEATURE_PROCESS));
        if (features.FEATURE_RUNTIMEINFORMATION) definedSymbols.Add(nameof(features.FEATURE_RUNTIMEINFORMATION));
        if (features.FEATURE_TASK) definedSymbols.Add(nameof(features.FEATURE_TASK));
        if (features.FEATURE_VALUETASK) definedSymbols.Add(nameof(features.FEATURE_VALUETASK));
        if (features.FEATURE_VALUETUPLE) definedSymbols.Add(nameof(features.FEATURE_VALUETUPLE));
        if (features.FEATURE_TIMEPROVIDER) definedSymbols.Add(nameof(features.FEATURE_TIMEPROVIDER));

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

            context.AddSource(baseName + ".g.cs", SourceText.From(ApplyConditions(content, definedSymbols), Encoding.UTF8));
        }
    }

    // Evaluates all #if/#elif/#else/#endif directives in a polyfill file against the
    // defined preprocessor symbols (TFM symbols, feature flags, and any others), stripping
    // all directive lines and inactive branches from the emitted source. Unknown symbols
    // evaluate to false (matching C# preprocessor semantics). #pragma, #nullable, and other
    // non-conditional directives are passed through unchanged.
    private static string ApplyConditions(string content, HashSet<string> definedSymbols)
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

        // Stack: (anyBranchTaken: bool, currentlyExcluding: bool)
        //   anyBranchTaken=true    → a previous branch was already active; remaining branches are skipped
        //   currentlyExcluding=true → this branch incremented excludeDepth (must decrement on #elif/#else/#endif)
        var stack = new Stack<(bool anyBranchTaken, bool currentlyExcluding)>();
        // Number of false #if branches we're nested inside
        int excludeDepth = 0;

        for (var i = 0; i < lines.Length; i++)
        {
            if (!directiveAtLine.TryGetValue(i, out var directive))
            {
                if (excludeDepth == 0)
                    sb.Append(lines[i]).Append('\n');
            }
            else if (directive is IfDirectiveTriviaSyntax ifDir)
            {
                var result = excludeDepth == 0 && EvaluateCondition(ifDir.Condition, definedSymbols);
                stack.Push((result, !result));
                if (!result)
                    excludeDepth++;
                // Suppress the directive line
            }
            else if (directive is ElifDirectiveTriviaSyntax elifDir)
            {
                if (stack.Count > 0)
                {
                    var (anyBranchTaken, currentlyExcluding) = stack.Pop();
                    if (currentlyExcluding)
                        excludeDepth--;
                    if (anyBranchTaken)
                    {
                        stack.Push((true, true));
                        excludeDepth++;
                    }
                    else
                    {
                        var result = excludeDepth == 0 && EvaluateCondition(elifDir.Condition, definedSymbols);
                        stack.Push((result, !result));
                        if (!result)
                            excludeDepth++;
                    }
                }
                // Suppress the directive line
            }
            else if (directive is ElseDirectiveTriviaSyntax)
            {
                if (stack.Count > 0)
                {
                    var (anyBranchTaken, currentlyExcluding) = stack.Pop();
                    if (currentlyExcluding)
                        excludeDepth--;
                    if (anyBranchTaken)
                    {
                        stack.Push((true, true));
                        excludeDepth++;
                    }
                    else
                    {
                        stack.Push((true, false));
                    }
                }
                // Suppress the directive line
            }
            else if (directive is EndIfDirectiveTriviaSyntax)
            {
                if (stack.Count > 0)
                {
                    var (_, currentlyExcluding) = stack.Pop();
                    if (currentlyExcluding)
                        excludeDepth--;
                }
                // Suppress the directive line
            }
            else
            {
                // #pragma, #nullable, etc.: pass through unchanged if not inside an excluded branch
                if (excludeDepth == 0)
                    sb.Append(lines[i]).Append('\n');
            }
        }

        return sb.ToString();
    }

    // Evaluates a preprocessor condition expression against the defined preprocessor symbols.
    // Undefined symbols evaluate to false, matching C# preprocessor semantics.
    // Parenthesized subexpressions are unwrapped recursively.
    private static bool EvaluateCondition(ExpressionSyntax condition, HashSet<string> definedSymbols)
    {
        if (condition is IdentifierNameSyntax ident)
            return definedSymbols.Contains(ident.Identifier.Text);
        if (condition is PrefixUnaryExpressionSyntax prefix && prefix.IsKind(SyntaxKind.LogicalNotExpression))
            return !EvaluateCondition(prefix.Operand, definedSymbols);
        if (condition is BinaryExpressionSyntax binary)
        {
            if (binary.IsKind(SyntaxKind.LogicalAndExpression))
                return EvaluateCondition(binary.Left, definedSymbols) && EvaluateCondition(binary.Right, definedSymbols);
            if (binary.IsKind(SyntaxKind.LogicalOrExpression))
                return EvaluateCondition(binary.Left, definedSymbols) || EvaluateCondition(binary.Right, definedSymbols);
        }
        if (condition is ParenthesizedExpressionSyntax paren)
            return EvaluateCondition(paren.Expression, definedSymbols);
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
