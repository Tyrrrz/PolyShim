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
    // Each property name matches a preprocessor symbol prepended as a #define to emitted sources.
    private sealed class PolyfillFeatures
    {
        public required bool FEATURE_ASYNCINTERFACES { get; init; }
        public required bool FEATURE_MANAGEMENT { get; init; }
        public required bool FEATURE_PROCESS { get; init; }
        public required bool FEATURE_TASK { get; init; }
    }

    // Computes feature flags for the current compilation by querying type availability.
    private static PolyfillFeatures ComputeFeatures(Compilation compilation) =>
        new PolyfillFeatures
        {
            FEATURE_ASYNCINTERFACES =
                compilation.GetTypeByMetadataName("System.Collections.Generic.IAsyncEnumerable`1")
                    is not null,
            FEATURE_MANAGEMENT =
                compilation.GetTypeByMetadataName("System.Management.ManagementObjectSearcher")
                    is not null,
            FEATURE_PROCESS =
                compilation.GetTypeByMetadataName("System.Diagnostics.Process") is not null,
            FEATURE_TASK =
                compilation.GetTypeByMetadataName("System.Threading.Tasks.Task") is not null,
        };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, Execute);
    }

    private static void Execute(SourceProductionContext context, Compilation compilation)
    {
        var features = ComputeFeatures(compilation);

        // Build a preamble with #define lines for generator-computed feature flags.
        // FEATURE_* symbols are not standard project symbols, so the consumer's compiler
        // cannot evaluate them without this preamble. TFM symbols (e.g. NET5_0_OR_GREATER)
        // are standard and handled by the consumer's compiler directly.
        // ALLOW_UNSAFE_BLOCKS is also not automatically defined; read it from compilation options.
        var preamble = new StringBuilder();
        if (features.FEATURE_ASYNCINTERFACES)
            preamble.AppendLine("#define FEATURE_ASYNCINTERFACES");
        if (features.FEATURE_MANAGEMENT)
            preamble.AppendLine("#define FEATURE_MANAGEMENT");
        if (features.FEATURE_PROCESS)
            preamble.AppendLine("#define FEATURE_PROCESS");
        if (features.FEATURE_TASK)
            preamble.AppendLine("#define FEATURE_TASK");
        if (
            compilation.Options is CSharpCompilationOptions csharpOptions
            && csharpOptions.AllowUnsafe
        )
            preamble.AppendLine("#define ALLOW_UNSAFE_BLOCKS");
        var preambleStr = preamble.ToString();

        var assembly = typeof(PolyShimGenerator).Assembly;
        foreach (
            var resourceName in assembly
                .GetManifestResourceNames()
                .OrderBy(n => n, StringComparer.Ordinal)
        )
        {
            const string polyfillsPrefix = "PolyShim.Polyfills.";
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

            context.AddSource(
                baseName + ".g.cs",
                SourceText.From(preambleStr + content, Encoding.UTF8)
            );
        }
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
            if (!trivia.IsDirective)
                continue;
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
                arity =
                    (typeDecl as TypeDeclarationSyntax)?.TypeParameterList?.Parameters.Count ?? 0;
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
            // Only consider a type as "already available" if it's public (accessible from any
            // assembly) or defined in the current compilation's own assembly (e.g., source-
            // generated). Types that are internal to a foreign reference assembly (e.g.,
            // internal polyfills embedded in FluentAssertions.dll) are not accessible to
            // consumer code, so the polyfill must still be emitted.
            var foundTypes = compilation.GetTypesByMetadataName(fullName);
            var accessibleType = foundTypes.FirstOrDefault(t =>
                t.DeclaredAccessibility == Accessibility.Public
                || SymbolEqualityComparer.Default.Equals(t.ContainingAssembly, compilation.Assembly)
            );
            if (accessibleType is null)
                return true; // No accessible version exists → emit

            // Special case: RuntimeHelpers is a compiler-known type where the C# compiler
            // merges all definitions across assemblies when locating members it needs for
            // code generation (e.g., GetSubArray for array-slice syntax). The type exists on
            // .NET Framework but lacks GetSubArray, so emit if that specific method is absent.
            if (
                typeName == "RuntimeHelpers"
                && ns == "System.Runtime.CompilerServices"
                && !accessibleType.GetMembers("GetSubArray").OfType<IMethodSymbol>().Any()
            )
                return true;
        }

        // No BCL-replacement types found in this file → always emit
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
                .FirstOrDefault(c =>
                    c.Identifier.Text.StartsWith("MemberPolyfills_", StringComparison.Ordinal)
                );

            // For Case 1 to apply, the class must be in a System.* namespace AND follow the
            // MemberPolyfills_{VERSION}_{Type} naming convention (at least 3 underscore parts).
            var parts = polyfillClass?.Identifier.Text.Split('_');
            if (
                polyfillClass?.Parent is BaseNamespaceDeclarationSyntax nsDecl
                && parts?.Length >= 3
            )
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
                    if (
                        member is MethodDeclarationSyntax meth
                        && meth.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                    )
                    {
                        // The extension block's receiver is implicit; the native static method
                        // has it as an explicit first parameter, so add 1 to the polyfill count.
                        if (
                            IsMethodMissing(
                                hostType,
                                meth.Identifier.Text,
                                meth.ParameterList.Parameters.Count + 1
                            )
                        )
                            return true;
                    }
                    else if (
                        member is PropertyDeclarationSyntax prop
                        && prop.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                    )
                    {
                        // Check for the property as a public static property on the host type.
                        if (
                            hostType is null
                            || !hostType
                                .GetMembers(prop.Identifier.Text)
                                .OfType<IPropertySymbol>()
                                .Any(p =>
                                    p.DeclaredAccessibility == Accessibility.Public && p.IsStatic
                                )
                        )
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
                if (param0 is null)
                    continue;

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
                    if (
                        member is MethodDeclarationSyntax meth
                        && meth.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                    )
                    {
                        var paramCount = meth.ParameterList.Parameters.Count;
                        var paramTypeNames = meth
                            .ParameterList.Parameters.Select(p => GetPolyfillParamTypeName(p.Type))
                            .ToList();
                        if (
                            IsMemberMissing(
                                targetType,
                                meth.Identifier.Text,
                                paramCount,
                                usingNamespaces,
                                compilation,
                                paramTypeNames
                            )
                        )
                            return true;
                    }
                    else if (
                        member is PropertyDeclarationSyntax prop
                        && prop.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                    )
                    {
                        if (
                            IsMemberMissing(
                                targetType,
                                prop.Identifier.Text,
                                paramCount: -1,
                                usingNamespaces,
                                compilation
                            )
                        )
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
    private static bool IsMethodMissing(
        INamedTypeSymbol? hostType,
        string methodName,
        int minParamCount
    )
    {
        if (hostType is null)
            return true; // Extension host class absent → polyfill is needed

        return !hostType
            .GetMembers(methodName)
            .OfType<IMethodSymbol>()
            .Any(m =>
                m.DeclaredAccessibility == Accessibility.Public
                && m.IsStatic
                && m.Parameters.Length >= minParamCount
            );
    }

    // Returns true if the named member is absent from the resolved target type.
    // paramCount == -1 means a property check (existence only, no parameter matching).
    // polyfillParamTypeNames provides the polyfill method's parameter type base-names for
    // disambiguation when an overload with the same name and count but different types exists
    // (e.g., NextBytes(Span<byte>) vs NextBytes(byte[])).
    // Also checks for BCL extension methods in static types of the imported namespaces so
    // that polyfills for interface targets (e.g., IEnumerable<T>) correctly detect native
    // LINQ/collection extensions that live in separate static classes (e.g., Enumerable).
    private static bool IsMemberMissing(
        INamedTypeSymbol? targetType,
        string memberName,
        int paramCount,
        IReadOnlyList<string> usingNamespaces,
        Compilation compilation,
        IReadOnlyList<string>? polyfillParamTypeNames = null
    )
    {
        // If the target type is absent (e.g., T[] representative MemoryExtensions not in compilation),
        // fall back to checking for BCL extension methods in the imported namespaces.
        if (targetType is null)
            return !HasPublicExtensionMethodInNamespaces(
                memberName,
                paramCount,
                usingNamespaces,
                compilation
            );

        var members = GetPublicMembersNamed(targetType, memberName).ToList();
        if (members.Count == 0)
        {
            // Not found as instance/static member — also check for BCL extension methods
            // (e.g., IEnumerable<T>.FirstOrDefault lives in System.Linq.Enumerable).
            // Pass targetType so only receiver-compatible extensions are considered
            // (e.g., Enumerable.AsEnumerable<T>(IEnumerable<T>) does NOT apply to
            // MatchCollection on .NET Framework because it doesn't implement IEnumerable<T>).
            return !HasPublicExtensionMethodInNamespaces(
                memberName,
                paramCount,
                usingNamespaces,
                compilation,
                targetType
            );
        }

        if (paramCount < 0)
            return false; // Property: presence is sufficient

        // Method: look for an overload with exactly the same number of parameters.
        // Using exact count (not >=) avoids false negatives where a same-name overload with
        // more required params (e.g., TryParse(string, NumberStyles, IFP, out T) with 4 params)
        // would incorrectly suppress emission of the polyfill for the 3-param variant.
        var exactMatches = members
            .OfType<IMethodSymbol>()
            .Where(ms => ms.Parameters.Length == paramCount)
            .ToList();

        if (exactMatches.Count > 0)
        {
            if (polyfillParamTypeNames is null)
                return false; // No type info — exact count match is sufficient

            // Type names provided: verify at least one exact-count overload is type-compatible.
            // This distinguishes overloads that differ only in parameter types
            // (e.g., NextBytes(Span<byte>) vs NextBytes(byte[])).
            foreach (var method in exactMatches)
            {
                var compatible = true;
                for (var i = 0; i < paramCount && compatible; i++)
                    if (GetSymbolTypeName(method.Parameters[i].Type) != polyfillParamTypeNames[i])
                        compatible = false;
                if (compatible)
                    return false; // Compatible overload exists → don't emit
            }

            // Exact count match found but types differ — check BCL extension methods too.
            return !HasPublicExtensionMethodInNamespaces(
                memberName,
                paramCount,
                usingNamespaces,
                compilation,
                targetType
            );
        }

        // No exact-count match at all — check for a matching BCL extension method.
        return !HasPublicExtensionMethodInNamespaces(
            memberName,
            paramCount,
            usingNamespaces,
            compilation,
            targetType
        );
    }

    // Returns the base type name of a Roslyn type symbol for parameter-type comparison.
    // Arrays return "ElementName[]" (e.g., "Byte[]"), named types return their simple name.
    private static string GetSymbolTypeName(ITypeSymbol type) =>
        type switch
        {
            IArrayTypeSymbol arr => arr.ElementType.Name + "[]",
            _ => type.Name,
        };

    // Extracts the base type name from a polyfill method's parameter TypeSyntax.
    // Nullable wrappers (T?) are unwrapped; C# keyword types are normalized to their CLR names.
    private static string GetPolyfillParamTypeName(TypeSyntax? typeSyntax)
    {
        if (typeSyntax is NullableTypeSyntax nts)
            typeSyntax = nts.ElementType;
        return typeSyntax switch
        {
            PredefinedTypeSyntax pre => pre.Keyword.ValueText switch
            {
                "int" => "Int32",
                "long" => "Int64",
                "short" => "Int16",
                "byte" => "Byte",
                "float" => "Single",
                "double" => "Double",
                "decimal" => "Decimal",
                "bool" => "Boolean",
                "char" => "Char",
                "string" => "String",
                "object" => "Object",
                "sbyte" => "SByte",
                "uint" => "UInt32",
                "ulong" => "UInt64",
                "ushort" => "UInt16",
                var k => k,
            },
            IdentifierNameSyntax id => id.Identifier.Text,
            GenericNameSyntax gen => gen.Identifier.Text,
            ArrayTypeSyntax arr => GetPolyfillParamTypeName(arr.ElementType) + "[]",
            // Unknown syntax: return empty string so type comparison fails → conservative emit.
            _ => "",
        };
    }

    // Returns true if any public static type in the given namespaces declares a public extension
    // method with the given name and at least paramCount non-receiver parameters.
    // This detects BCL extension methods that extend interface types (e.g., Enumerable.FirstOrDefault
    // extends IEnumerable<T>) or array/string types (e.g., MemoryExtensions.AsSpan extends T[]).
    // When targetType is provided, only extension methods whose receiver type is compatible with
    // targetType are considered (e.g., Enumerable.AsEnumerable<T>(IEnumerable<T>) is NOT applicable
    // to MatchCollection on .NET Framework because it doesn't implement IEnumerable<T>).
    private static bool HasPublicExtensionMethodInNamespaces(
        string methodName,
        int paramCount,
        IReadOnlyList<string> usingNamespaces,
        Compilation compilation,
        INamedTypeSymbol? targetType = null
    )
    {
        if (paramCount < 0)
            return false; // No BCL extension properties

        foreach (var nsName in usingNamespaces)
        {
            var nsSymbol = GetNamespaceSymbol(compilation, nsName);
            if (nsSymbol is null)
                continue;

            foreach (var type in nsSymbol.GetTypeMembers())
            {
                if (!type.IsStatic)
                    continue;
                foreach (var method in type.GetMembers(methodName).OfType<IMethodSymbol>())
                {
                    if (!method.IsExtensionMethod)
                        continue;
                    if (method.DeclaredAccessibility != Accessibility.Public)
                        continue;
                    // Extension method has a 'this' receiver as first parameter;
                    // the remaining parameters must be >= paramCount.
                    if (method.Parameters.Length - 1 < paramCount)
                        continue;

                    // If targetType is provided, verify the receiver type is compatible.
                    if (
                        targetType is not null
                        && !IsReceiverTypeCompatible(targetType, method.Parameters[0].Type)
                    )
                        continue;

                    return true;
                }
            }
        }

        return false;
    }

    // Returns true if targetType is compatible with the extension method's receiver parameter type.
    // Compatibility means targetType is the same as, inherits from, or implements the receiver type.
    // For generic receiver types (e.g., IEnumerable<T>), the original definition is used so that
    // any constructed form of that generic interface satisfies the check.
    // Type-parameter receivers (e.g., T in where T : class) are conservatively treated as compatible.
    private static bool IsReceiverTypeCompatible(
        INamedTypeSymbol targetType,
        ITypeSymbol receiverType
    )
    {
        // Type-parameter receiver (e.g., void Foo<T>(this T value)): conservatively compatible.
        if (receiverType.TypeKind == TypeKind.TypeParameter)
            return true;

        var receiverOrigDef = receiverType is INamedTypeSymbol namedReceiver
            ? namedReceiver.OriginalDefinition
            : receiverType;

        // Check targetType itself and its base types
        INamedTypeSymbol? cur = targetType;
        while (cur is not null)
        {
            if (SymbolEqualityComparer.Default.Equals(cur.OriginalDefinition, receiverOrigDef))
                return true;
            cur = cur.BaseType;
        }

        // Check implemented interfaces
        foreach (var iface in targetType.AllInterfaces)
        {
            if (SymbolEqualityComparer.Default.Equals(iface.OriginalDefinition, receiverOrigDef))
                return true;
        }

        return false;
    }

    // Traverses the compilation's global namespace hierarchy to find the symbol for a
    // dotted namespace name (e.g., "System.Linq" → INamespaceSymbol for System.Linq).
    private static INamespaceSymbol? GetNamespaceSymbol(
        Compilation compilation,
        string namespaceName
    )
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
        Compilation compilation
    )
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

        // Predefined keyword type (string, int, long, etc.)
        // Use GetSpecialType so the compiler's own binding for the target framework is used,
        // which avoids assembly-resolution ambiguity (e.g., System.Private.CoreLib from the
        // host .NET runtime vs mscorlib.dll from the .NET Framework reference assemblies).
        if (typeSyntax is PredefinedTypeSyntax pre)
        {
            var specialType = pre.Keyword.Kind() switch
            {
                SyntaxKind.StringKeyword => SpecialType.System_String,
                SyntaxKind.IntKeyword => SpecialType.System_Int32,
                SyntaxKind.LongKeyword => SpecialType.System_Int64,
                SyntaxKind.ShortKeyword => SpecialType.System_Int16,
                SyntaxKind.ByteKeyword => SpecialType.System_Byte,
                SyntaxKind.SByteKeyword => SpecialType.System_SByte,
                SyntaxKind.UIntKeyword => SpecialType.System_UInt32,
                SyntaxKind.ULongKeyword => SpecialType.System_UInt64,
                SyntaxKind.UShortKeyword => SpecialType.System_UInt16,
                SyntaxKind.FloatKeyword => SpecialType.System_Single,
                SyntaxKind.DoubleKeyword => SpecialType.System_Double,
                SyntaxKind.DecimalKeyword => SpecialType.System_Decimal,
                SyntaxKind.BoolKeyword => SpecialType.System_Boolean,
                SyntaxKind.CharKeyword => SpecialType.System_Char,
                SyntaxKind.ObjectKeyword => SpecialType.System_Object,
                _ => SpecialType.None,
            };
            if (specialType == SpecialType.None)
                return null;
            var typeSymbol = compilation.GetSpecialType(specialType);
            return typeSymbol.TypeKind == TypeKind.Error ? null : typeSymbol;
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
                compilation
            );
        }

        // Fall back: return null for any unhandled type form (conservative — emit the polyfill).
        // Polyfill extension parameters never use qualified names or other exotic type forms.
        return null;
    }

    // Resolves a simple or generic type name to a symbol by trying each using namespace in order.
    // Uses GetTypesByMetadataName to handle type-forwarded types correctly (e.g., on .NET Framework
    // many BCL types are forwarded from facade assemblies, causing GetTypeByMetadataName to return null).
    private static INamedTypeSymbol? ResolveTypeName(
        string baseName,
        int arity,
        IReadOnlyList<string> usingNamespaces,
        Compilation compilation
    )
    {
        var metadataName = arity > 0 ? $"{baseName}`{arity}" : baseName;
        foreach (var ns in usingNamespaces)
        {
            var sym = GetFirstAccessibleType(compilation, $"{ns}.{metadataName}");
            if (sym is not null)
                return sym;
        }
        return null;
    }

    // Returns the first publicly accessible symbol matching the given metadata name, handling
    // the case where a type is defined in multiple assemblies (type forwarding).
    // Types that are internal to a foreign assembly are skipped; the current assembly's own types
    // and public types from referenced assemblies are preferred.
    // Falls back to types[0] if no public/own-assembly definition exists — this can happen for
    // rare internal-forwarded types, but in practice BCL types are always public.
    //
    // When multiple accessible types exist, priority is:
    //   1. mscorlib — always the authoritative .NET Framework assembly; its API surface accurately
    //      reflects what is available on .NET Framework without any modern additions.
    //   2. Non-host-runtime types — excludes System.Private.CoreLib (host .NET runtime) and
    //      netstandard (stub assembly) which may expose newer APIs not in the actual target
    //      framework (e.g., String.Contains(string, StringComparison) exists in both but is absent
    //      from .NET Framework's mscorlib.dll).
    //   3. Any remaining accessible type as a fallback.
    private static INamedTypeSymbol? GetFirstAccessibleType(
        Compilation compilation,
        string metadataName
    )
    {
        var types = compilation.GetTypesByMetadataName(metadataName);
        if (types.IsEmpty)
            return null;

        var accessible = types
            .Where(t =>
                t.DeclaredAccessibility == Accessibility.Public
                || SymbolEqualityComparer.Default.Equals(t.ContainingAssembly, compilation.Assembly)
            )
            .ToArray();

        if (accessible.Length == 0)
            return types[0];

        if (accessible.Length == 1)
            return accessible[0];

        // Priority 1: mscorlib — always use the actual .NET Framework assembly when present.
        // It has the most restrictive API surface for .NET Framework targets and is never present
        // on .NET Core/.NET 5+ compilations.
        var mscorlibType = accessible.FirstOrDefault(t => t.ContainingAssembly.Name == "mscorlib");
        if (mscorlibType is not null)
            return mscorlibType;

        // Priority 2: Prefer non-host-runtime types over System.Private.CoreLib and netstandard.
        // System.Private.CoreLib is the host .NET runtime (richer API surface than target).
        // netstandard.dll is a stub/compatibility assembly that may expose APIs not yet present
        // in the actual target framework (e.g., String.Contains(string, StringComparison) appears
        // in netstandard 2.1 stubs but is absent from .NET Framework mscorlib.dll).
        var targetFrameworkType = accessible.FirstOrDefault(t =>
            t.ContainingAssembly.Name != "System.Private.CoreLib"
            && t.ContainingAssembly.Name != "netstandard"
        );
        return targetFrameworkType ?? accessible[0];
    }

    // Collects all non-alias, non-static using namespaces plus the file-scoped namespace (if any).
    private static List<string> CollectUsingNamespaces(SyntaxNode root)
    {
        var result = new List<string>();

        foreach (var u in root.DescendantNodes().OfType<UsingDirectiveSyntax>())
        {
            if (u.StaticKeyword.IsKind(SyntaxKind.StaticKeyword))
                continue;
            if (u.Alias is not null)
                continue;
            var name = u.Name?.ToString();
            if (name is not null && !result.Contains(name))
                result.Add(name);
        }

        var fileNs = root.DescendantNodes()
            .OfType<FileScopedNamespaceDeclarationSyntax>()
            .FirstOrDefault();
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
