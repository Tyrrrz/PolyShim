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
    // Handles class, struct, interface, record, enum (BaseTypeDeclarationSyntax) and delegate
    // (DelegateDeclarationSyntax) — the latter is not a BaseTypeDeclarationSyntax in Roslyn.
    private static bool ShouldEmitTypePolyfill(SyntaxNode root, Compilation compilation)
    {
        bool foundAnyBclType = false;

        foreach (var node in root.DescendantNodes())
        {
            string? typeName;
            int arity;
            SyntaxNode? parent;

            if (node is BaseTypeDeclarationSyntax typeDecl)
            {
                typeName = typeDecl.Identifier.Text;
                arity = (typeDecl as TypeDeclarationSyntax)?.TypeParameterList?.Parameters.Count ?? 0;
                parent = typeDecl.Parent;
            }
            else if (node is DelegateDeclarationSyntax delegateDecl)
            {
                typeName = delegateDecl.Identifier.Text;
                arity = delegateDecl.TypeParameterList?.Parameters.Count ?? 0;
                parent = delegateDecl.Parent;
            }
            else
            {
                continue;
            }

            // All PolyShim type polyfills declare their types inside a named namespace.
            // Skip any type not directly inside a namespace (shouldn't happen in practice).
            if (parent is not BaseNamespaceDeclarationSyntax nsDecl)
                continue;

            foundAnyBclType = true;
            var ns = nsDecl.Name.ToString();
            var fullName = arity > 0 ? $"{ns}.{typeName}`{arity}" : $"{ns}.{typeName}";

            // Use GetTypesByMetadataName instead of GetTypeByMetadataName: the latter returns
            // null when a type is defined in multiple assemblies (e.g., due to type forwarding
            // in newer .NET versions), which would cause false-positive polyfill emission.
            if (compilation.GetTypesByMetadataName(fullName).IsEmpty)
                return true; // At least one declared type is missing → emit
        }

        // No namespaced types found (e.g., NamespaceDummies.cs) → always emit
        return !foundAnyBclType;
    }

    // For member polyfills: emit the file if any method/property in any extension block is
    // absent from the target type in the compilation.
    //
    // Uses a naming-convention heuristic to determine what to check:
    //   1. MemberPolyfills_*_{Type} in a System.* namespace:
    //      → the polyfill extends an existing BCL class (e.g., System.Linq.Enumerable);
    //        look up {ns}.{Type} and check if each method exists as a public static member.
    //   2. MemberPolyfills_*_{Type} in the global namespace:
    //      → the polyfill adds members to a BCL type (e.g., HttpClient);
    //        resolve {Type} via using directives and check instance/extension members.
    private static bool ShouldEmitMemberPolyfill(SyntaxNode root, Compilation compilation)
    {
        var usingNamespaces = CollectUsingNamespaces(root);

        foreach (var ext in root.DescendantNodes().OfType<ExtensionBlockDeclarationSyntax>())
        {
            // Find the enclosing MemberPolyfills_* class to determine the check strategy.
            var polyfillClass = ext.Ancestors()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text.StartsWith("MemberPolyfills_", StringComparison.Ordinal));

            // For Case 1 to apply, the class must be in a System.* namespace AND follow the
            // MemberPolyfills_{VERSION}_{Type} naming convention (at least 3 underscore parts).
            var parts = polyfillClass?.Identifier.Text.Split('_');
            if (polyfillClass?.Parent is BaseNamespaceDeclarationSyntax nsDecl && parts?.Length >= 3)
            {
                // Case 1: MemberPolyfills_*_{Type} in a System.* namespace.
                // The {Type} portion (third underscore-separated part, index 2) names the BCL static
                // class that hosts these extension methods. Look it up directly and check public static
                // methods. Using parts[2] handles disambiguator suffixes like:
                //   MemberPolyfills_NetCore21_MemoryExtensions_Contains → type = "MemoryExtensions"
                var typeSegment = parts[2];
                // Use GetTypesByMetadataName to handle type-forwarded types correctly.
                var hostTypes = compilation.GetTypesByMetadataName($"{nsDecl.Name}.{typeSegment}");
                var hostType = hostTypes.IsEmpty ? null : hostTypes[0];

                foreach (var member in ext.Members)
                {
                    if (member is MethodDeclarationSyntax meth &&
                        meth.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                    {
                        // The extension block's receiver is implicit; the native static method
                        // has it as an explicit first parameter, so add 1 to the polyfill count.
                        if (IsExtensionHostMethodMissing(hostType, meth.Identifier.Text, meth.ParameterList.Parameters.Count + 1))
                            return true;
                    }
                }
            }
            else
            {
                // Case 2: MemberPolyfills_*_{Type} in the global namespace (or no MemberPolyfills_*
                // class found). Resolve the target type from the extension block's parameter type
                // and check instance members and BCL extension methods in the imported namespaces.
                var param0 = ext.ParameterList?.Parameters.FirstOrDefault();
                if (param0 is null) continue;

                // Determine if the extension parameter is an array type (T[]) — a special case
                // where the resolved targetType may be null but the polyfill is still needed.
                var rawType = param0.Type is NullableTypeSyntax nts ? nts.ElementType : param0.Type;
                var isArrayTarget = rawType is ArrayTypeSyntax;

                var targetType = ResolveExtensionTarget(param0.Type, usingNamespaces, compilation);

                // If the direct target type is absent from this compilation, the polyfill body
                // would reference a non-existent type and fail to compile — skip this block.
                // Exception: array targets (T[]) use a representative type lookup; null there
                // simply means the representative (MemoryExtensions) is absent, not the array.
                if (targetType is null && !isArrayTarget)
                    continue;

                foreach (var member in ext.Members)
                {
                    if (member is MethodDeclarationSyntax meth &&
                        meth.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                    {
                        if (IsMemberMissing(targetType, meth.Identifier.Text, meth.ParameterList.Parameters.Count, usingNamespaces, compilation))
                            return true;
                    }
                    else if (member is PropertyDeclarationSyntax prop &&
                             prop.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                    {
                        if (IsMemberMissing(targetType, prop.Identifier.Text, paramCount: -1, usingNamespaces, compilation))
                            return true;
                    }
                }
            }
        }

        return false;
    }

    // Returns true if the named public static method is absent from the extension host class,
    // or if the host class itself doesn't exist in the compilation.
    // minParamCount includes the implicit receiver parameter that becomes explicit on the static method
    // (so minParamCount = polyfill_param_count + 1 for extension methods).
    private static bool IsExtensionHostMethodMissing(INamedTypeSymbol? hostType, string methodName, int minParamCount)
    {
        if (hostType is null)
            return true; // Extension host class absent → polyfill is needed

        return !hostType.GetMembers(methodName)
            .OfType<IMethodSymbol>()
            .Any(m => m.DeclaredAccessibility == Accessibility.Public
                   && m.IsStatic
                   && m.Parameters.Length >= minParamCount);
    }

    // Returns true if the named member is absent from the resolved target type.
    // paramCount == -1 means a property check (existence only, no parameter matching).
    // Also checks for BCL extension methods in static types of the imported namespaces so
    // that polyfills for interface targets (e.g., IEnumerable<T>) correctly detect native
    // LINQ/collection extensions that live in separate static classes (e.g., Enumerable).
    private static bool IsMemberMissing(INamedTypeSymbol? targetType, string memberName, int paramCount,
        IReadOnlyList<string> usingNamespaces, Compilation compilation)
    {
        // If the target type is absent (e.g., T[] representative MemoryExtensions not in compilation),
        // fall back to checking for BCL extension methods in the imported namespaces.
        if (targetType is null)
            return !HasPublicExtensionMethodInNamespaces(memberName, paramCount, usingNamespaces, compilation);

        var members = GetPublicMembersNamed(targetType, memberName).ToList();
        if (members.Count == 0)
        {
            // Not found as instance/static member — also check for BCL extension methods
            // (e.g., IEnumerable<T>.FirstOrDefault lives in System.Linq.Enumerable).
            return !HasPublicExtensionMethodInNamespaces(memberName, paramCount, usingNamespaces, compilation);
        }

        if (paramCount < 0)
            return false; // Property: presence is sufficient

        // Method: verify at least one overload accepts at least as many parameters as the polyfill.
        // This distinguishes new CT-accepting overloads from pre-existing ones (e.g. ReadLineAsync(CT)
        // vs ReadLineAsync() which existed long before the CT overload).
        if (!members.All(m => m is IMethodSymbol ms && ms.Parameters.Length < paramCount))
            return false;

        // All instance overloads have too few params — check for a matching BCL extension method.
        return !HasPublicExtensionMethodInNamespaces(memberName, paramCount, usingNamespaces, compilation);
    }

    // Returns true if any public static type in the given namespaces declares a public extension
    // method with the given name and at least paramCount non-receiver parameters.
    // This detects BCL extension methods that extend interface types (e.g., Enumerable.FirstOrDefault
    // extends IEnumerable<T>) or array/string types (e.g., MemoryExtensions.AsSpan extends T[]).
    private static bool HasPublicExtensionMethodInNamespaces(
        string methodName, int paramCount,
        IReadOnlyList<string> usingNamespaces, Compilation compilation)
    {
        if (paramCount < 0)
            return false; // No BCL extension properties

        foreach (var nsName in usingNamespaces)
        {
            var nsSymbol = GetNamespaceSymbol(compilation, nsName);
            if (nsSymbol is null) continue;

            foreach (var type in nsSymbol.GetTypeMembers())
            {
                if (!type.IsStatic) continue;
                foreach (var method in type.GetMembers(methodName).OfType<IMethodSymbol>())
                {
                    if (!method.IsExtensionMethod) continue;
                    if (method.DeclaredAccessibility != Accessibility.Public) continue;
                    // Extension method has a 'this' receiver as first parameter;
                    // the remaining parameters must be >= paramCount.
                    if (method.Parameters.Length - 1 >= paramCount)
                        return true;
                }
            }
        }

        return false;
    }

    // Traverses the compilation's global namespace hierarchy to find the symbol for a
    // dotted namespace name (e.g., "System.Linq" → INamespaceSymbol for System.Linq).
    private static INamespaceSymbol? GetNamespaceSymbol(Compilation compilation, string namespaceName)
    {
        INamespaceSymbol? current = compilation.GlobalNamespace;
        foreach (var part in namespaceName.Split('.'))
            current = current?.GetMembers(part).OfType<INamespaceSymbol>().FirstOrDefault();
        return current;
    }

    // Resolves the extension parameter's TypeSyntax to the Roslyn type symbol whose members we check.
    // For array (T[]) targets, returns null — extension method lookup via the imported namespaces
    // is used instead (e.g., System.MemoryExtensions provides T[] span/memory methods).
    // For string targets, returns System.String so both instance members and MemoryExtensions
    // extension methods (AsSpan, AsMemory, etc.) are correctly considered.
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

        // Array type (T[]): no single concrete type symbol; extension methods in the imported
        // namespaces (e.g., System.MemoryExtensions) are checked via HasPublicExtensionMethodInNamespaces.
        if (typeSyntax is ArrayTypeSyntax)
            return null;

        // Predefined keyword type (string, byte, int, etc.)
        if (typeSyntax is PredefinedTypeSyntax pre)
        {
            // string: check System.String for instance/static members; MemoryExtensions
            // extension methods (AsSpan, AsMemory, etc.) are found via namespace lookup.
            if (pre.Keyword.IsKind(SyntaxKind.StringKeyword))
                return compilation.GetTypeByMetadataName("System.String");

            // Other predefined types (byte, int, etc.): conservatively emit
            return null;
        }

        // Simple name: IdentifierNameSyntax (e.g., "Path", "Random", "HttpClient")
        if (typeSyntax is IdentifierNameSyntax ident)
            return ResolveTypeName(ident.Identifier.Text, arity: 0, usingNamespaces, compilation);

        // Generic name: GenericNameSyntax (e.g., "IEnumerable<T>", "Task<T>", "ArraySegment<T>")
        if (typeSyntax is GenericNameSyntax generic)
        {
            // ArraySegment<T>: resolve to the actual type; MemoryExtensions extension methods
            // (e.g., AsSpan(ArraySegment<T>)) are found via HasPublicExtensionMethodInNamespaces.
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
