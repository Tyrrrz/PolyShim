#!/usr/bin/dotnet --
#:package Microsoft.CodeAnalysis.CSharp
#:package CliFx

using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[Command(Description = "Generates Signatures.md from PolyShim polyfill source files.")]
public partial class GenerateSignaturesCommand : ICommand
{
    [CommandOption(
        "project-dir",
        'p',
        Description = "Path to the PolyShim project directory containing polyfill source files. Defaults to the 'PolyShim' subdirectory relative to the script."
    )]
    public DirectoryInfo? ProjectDir { get; set; }

    [CommandOption(
        "output",
        'o',
        Description = "Path to the output Signatures.md file. Defaults to 'Signatures.md' in the script's directory."
    )]
    public FileInfo? OutputFile { get; set; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        var scriptDir =
            Path.GetDirectoryName(GetScriptPath())
            ?? throw new InvalidOperationException(
                "Could not resolve script directory from script path."
            );

        var sourceDir = ProjectDir?.FullName ?? Path.Combine(scriptDir, "PolyShim");
        var outputPath = OutputFile?.FullName ?? Path.Combine(scriptDir, "Signatures.md");

        var signatures = new List<Signature>();

        var codeFiles = Directory
            .EnumerateFiles(sourceDir, "*.cs", SearchOption.AllDirectories)
            .Where(f =>
                !f.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar)
                && !f.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar)
                && !f.Contains(
                    Path.AltDirectorySeparatorChar + "obj" + Path.AltDirectorySeparatorChar
                )
                && !f.Contains(
                    Path.AltDirectorySeparatorChar + "bin" + Path.AltDirectorySeparatorChar
                )
            );

        var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);

        foreach (var file in codeFiles)
        {
            var relativePath = Path.GetRelativePath(sourceDir, file);

            // Extract framework directory name (first segment of relative path)
            var dirName = relativePath
                .Split(
                    new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                    StringSplitOptions.RemoveEmptyEntries
                )
                .FirstOrDefault();

            if (dirName is null)
                continue;

            var framework = GetFrameworkName(dirName);
            if (framework is null)
                continue;

            var source = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(StripDirectives(source), parseOptions);
            var root = tree.GetRoot();

            // Extension block members
            foreach (
                var extBlock in root.DescendantNodes().OfType<ExtensionBlockDeclarationSyntax>()
            )
            {
                var typeName = GetExtensionTypeName(extBlock);
                if (typeName is null)
                {
                    console.Error.WriteLine(
                        $"Warning: Unable to extract type from extension block in '{relativePath}'."
                    );
                    continue;
                }

                foreach (var member in extBlock.Members)
                {
                    string? sig = null;
                    string? url = null;
                    bool isStatic = false;

                    if (
                        member is MethodDeclarationSyntax method
                        && method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                    )
                    {
                        sig = FormatMethodSignature(method);
                        url = ExtractDocUrl(member);
                        isStatic = method.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));
                    }
                    else if (
                        member is PropertyDeclarationSyntax prop
                        && prop.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                    )
                    {
                        sig = FormatPropertySignature(prop);
                        url = ExtractDocUrl(member);
                        isStatic = prop.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));
                    }

                    if (sig is null)
                        continue;

                    signatures.Add(new Signature(typeName, sig, "Extension", framework, url, isStatic));
                }
            }

            // Internal type declarations (not MemberPolyfills_*)
            foreach (var typeDecl in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
            {
                if (!typeDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.InternalKeyword)))
                    continue;

                if (
                    typeDecl.Identifier.Text.StartsWith(
                        "MemberPolyfills_",
                        StringComparison.Ordinal
                    )
                )
                    continue;

                var typeName = FormatTypeDeclarationName(typeDecl);
                var typeKind = GetTypeKind(typeDecl);
                var url = ExtractDocUrl(typeDecl);

                signatures.Add(new Signature(typeName, "", typeKind, framework, url, false));
            }
        }

        // Deduplicate type declarations: keep the one with a URL.
        var deduplicated = new List<Signature>();
        var seenTypes = new Dictionary<string, Signature>();

        foreach (
            var record in signatures
                .OrderBy(r => r.TypeName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(r => r.Kind == "Extension" ? 1 : 0)
                .ThenBy(r => string.IsNullOrEmpty(r.Member) ? 0 : 1)
                .ThenBy(r => r.IsStatic ? 0 : 1)
                .ThenBy(r => r.Member, StringComparer.OrdinalIgnoreCase)
        )
        {
            if (record.Kind == "Extension")
            {
                deduplicated.Add(record);
                continue;
            }

            var typeKey = $"{record.TypeName}|{record.Kind}";
            if (!seenTypes.TryGetValue(typeKey, out var existing))
            {
                seenTypes[typeKey] = record;
                deduplicated.Add(record);
            }
            else if (record.Url is not null && existing.Url is null)
            {
                // Replace with the one that has a URL
                var idx = deduplicated.IndexOf(existing);
                deduplicated[idx] = record;
                seenTypes[typeKey] = record;
            }
        }

        // Warn for any types or members that ended up without a documentation URL
        foreach (var record in deduplicated.Where(r => r.Url is null))
        {
            if (record.Kind == "Extension")
                console.Error.WriteLine(
                    $"Warning: Missing documentation URL for member '{record.TypeName}.{record.Member}'."
                );
            else
                console.Error.WriteLine(
                    $"Warning: Missing documentation URL for type '{record.TypeName}'."
                );
        }

        // Statistics
        var totalTypes = deduplicated.Count(r => r.Kind != "Extension");
        var totalMembers = deduplicated.Count(r => r.Kind == "Extension");
        var total = deduplicated.Count;

        // Generate Markdown
        var sb = new StringBuilder();
        sb.AppendLine("# Signatures");
        sb.AppendLine();
        sb.AppendLine($"- **Total:** {total}");
        sb.AppendLine($"- **Types:** {totalTypes}");
        sb.AppendLine($"- **Members:** {totalMembers}");
        sb.AppendLine();
        sb.AppendLine("___");
        sb.AppendLine();

        var grouped = deduplicated
            .GroupBy(r => r.TypeName.TrimEnd('?'))
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var group in grouped)
        {
            sb.AppendLine($"- `{group.Key}`");

            foreach (var item in group)
            {
                var frameworkTag = $" <sup><sub>{item.Framework}</sub></sup>";
                var content = item.Member.Length > 0 ? $"`{item.Member}`" : $"**[{item.Kind}]**";

                if (item.Url is not null)
                    sb.AppendLine($"  - [{content}]({item.Url}){frameworkTag}");
                else
                    sb.AppendLine($"  - {content}{frameworkTag}");
            }
        }

        File.WriteAllText(
            outputPath,
            sb.ToString(),
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
        );

        console.Output.WriteLine($"Generated signature list: {outputPath}");
        console.Output.WriteLine($"Total: {total}");
        console.Output.WriteLine($"Types: {totalTypes}");
        console.Output.WriteLine($"Members: {totalMembers}");

        return default;
    }

    private static string GetScriptPath([CallerFilePath] string path = "") => path;

    private static string? GetFrameworkName(string dirName)
    {
        // Net100 -> .NET 10.0, Net70 -> .NET 7.0, etc.
        var m = Regex.Match(dirName, @"^Net(\d+)(\d)$");
        if (m.Success)
            return $".NET {m.Groups[1].Value}.{m.Groups[2].Value}";

        // NetCore20 -> .NET Core 2.0, etc.
        m = Regex.Match(dirName, @"^NetCore(\d)(\d)$");
        if (m.Success)
            return $".NET Core {m.Groups[1].Value}.{m.Groups[2].Value}";

        return null;
    }

    private static string StripDirectives(string source)
    {
        // Process line-by-line:
        // - Replace preprocessor directive lines with blank lines (preserves line count).
        // - Keep all non-directive lines, regardless of #if/#else/#elif branches.
        // This ensures declarations in inactive conditional branches are not discarded.
        var lines = source.Split('\n');
        var result = new StringBuilder();

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd('\r');
            var trimmed = line.TrimStart();

            if (trimmed.StartsWith("#", StringComparison.Ordinal))
            {
                // Strip directive lines but preserve line count for diagnostics.
                result.AppendLine();
            }
            else
            {
                result.AppendLine(line);
            }
        }

        return result.ToString();
    }

    private static string? GetExtensionTypeName(ExtensionBlockDeclarationSyntax extBlock)
    {
        var param = extBlock.ParameterList?.Parameters.FirstOrDefault();
        if (param?.Type is null)
            return null;

        return FormatType(param.Type);
    }

    private static string FormatType(TypeSyntax type) =>
        type switch
        {
            // Tuple types: strip element names -> (int, T) instead of (int index, T value)
            TupleTypeSyntax tuple => "("
                + string.Join(", ", tuple.Elements.Select(e => FormatType(e.Type)))
                + ")",

            // Generic types: recursively format type args
            GenericNameSyntax generic => generic.Identifier.Text
                + "<"
                + string.Join(", ", generic.TypeArgumentList.Arguments.Select(FormatType))
                + ">",

            // Nullable types: T?
            NullableTypeSyntax nullable => FormatType(nullable.ElementType) + "?",

            // Array types: T[]
            ArrayTypeSyntax array => FormatType(array.ElementType)
                + string.Concat(array.RankSpecifiers.Select(r => r.ToString())),

            // ref T
            RefTypeSyntax refType => "ref " + FormatType(refType.Type),

            // Qualified names: use the right-most identifier/generic name
            QualifiedNameSyntax qualified => qualified.Right is GenericNameSyntax gn
                ? FormatType(gn)
                : qualified.Right.ToString(),

            // Predefined types, identifier names, etc.
            _ => NormalizeWhitespace(type.ToString()),
        };

    private static string FormatMethodSignature(MethodDeclarationSyntax method)
    {
        var sb = new StringBuilder();

        if (method.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
        {
            sb.Append("static ");
        }

        sb.Append(FormatType(method.ReturnType));
        sb.Append(' ');
        sb.Append(method.Identifier.Text);

        if (method.TypeParameterList is { Parameters.Count: > 0 } tpl)
        {
            sb.Append('<');
            sb.Append(string.Join(", ", tpl.Parameters.Select(p => p.Identifier.Text)));
            sb.Append('>');
        }

        sb.Append('(');
        sb.Append(string.Join(", ", method.ParameterList.Parameters.Select(FormatParameter)));
        sb.Append(')');

        foreach (var constraint in method.ConstraintClauses)
        {
            sb.Append(' ');
            sb.Append(FormatConstraintClause(constraint));
        }

        return sb.ToString();
    }

    private static string FormatPropertySignature(PropertyDeclarationSyntax prop)
    {
        var prefix = prop.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)) ? "static " : "";
        return $"{prefix}{FormatType(prop.Type)} {prop.Identifier.Text}";
    }

    private static string FormatParameter(ParameterSyntax param)
    {
        var sb = new StringBuilder();

        foreach (var mod in param.Modifiers)
        {
            if (
                mod.IsKind(SyntaxKind.OutKeyword)
                || mod.IsKind(SyntaxKind.RefKeyword)
                || mod.IsKind(SyntaxKind.InKeyword)
                || mod.IsKind(SyntaxKind.ParamsKeyword)
            )
            {
                sb.Append(mod.Text);
                sb.Append(' ');
            }
        }

        sb.Append(FormatType(param.Type!));
        return sb.ToString();
    }

    private static string FormatConstraintClause(TypeParameterConstraintClauseSyntax clause)
    {
        var name = clause.Name.Identifier.Text;
        var constraints = clause.Constraints.Select<TypeParameterConstraintSyntax, string>(c =>
            c switch
            {
                ClassOrStructConstraintSyntax cls => cls.ClassOrStructKeyword.Text,
                TypeConstraintSyntax tc => FormatType(tc.Type),
                ConstructorConstraintSyntax => "new()",
                DefaultConstraintSyntax => "default",
                _ => NormalizeWhitespace(c.ToString()),
            }
        );

        return $"where {name} : {string.Join(", ", constraints)}";
    }

    private static string FormatTypeDeclarationName(BaseTypeDeclarationSyntax typeDecl)
    {
        if (
            typeDecl is TypeDeclarationSyntax td
            && td.TypeParameterList is { Parameters.Count: > 0 } tpl
        )
        {
            return td.Identifier.Text
                + "<"
                + string.Join(", ", tpl.Parameters.Select(p => p.Identifier.Text))
                + ">";
        }

        return typeDecl.Identifier.Text;
    }

    private static string GetTypeKind(BaseTypeDeclarationSyntax typeDecl) =>
        typeDecl switch
        {
            ClassDeclarationSyntax => "class",
            StructDeclarationSyntax => "struct",
            EnumDeclarationSyntax => "enum",
            InterfaceDeclarationSyntax => "interface",
            RecordDeclarationSyntax rec => rec.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword)
                ? "record struct"
                : "record",
            _ => typeDecl.Kind().ToString().ToLowerInvariant(),
        };

    private static string? ExtractDocUrl(SyntaxNode node)
    {
        // Scan for a // https://... URL comment in the leading trivia.
        // For type declarations with attributes, the URL may appear between
        // the attribute list and the 'internal' modifier, so check both positions.
        return ScanTriviaForUrl(node.GetLeadingTrivia())
            ?? (
                node is BaseTypeDeclarationSyntax typeDecl && typeDecl.Modifiers.Count > 0
                    ? ScanTriviaForUrl(typeDecl.Modifiers[0].LeadingTrivia)
                    : null
            );
    }

    private static string? ScanTriviaForUrl(SyntaxTriviaList triviaList)
    {
        // Scan leading trivia in reverse order for a // https://... URL comment.
        // Continue scanning through all comment lines (not stopping at non-URL comments),
        // stopping only at non-comment, non-whitespace trivia.
        foreach (var trivia in triviaList.Reverse())
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            {
                var text = trivia.ToString().Trim();
                var match = Regex.Match(text, @"^//\s*(https://(?:learn|docs)\.microsoft\.com\S+)");
                if (match.Success)
                    return match.Groups[1].Value;
                // Non-URL comment – keep scanning upward (may have URL above notes)
            }
            else if (
                trivia.IsKind(SyntaxKind.WhitespaceTrivia)
                || trivia.IsKind(SyntaxKind.EndOfLineTrivia)
            )
            {
                // Skip whitespace / newlines
            }
            else
            {
                // Any other trivia (attributes, directives) – stop
                break;
            }
        }

        return null;
    }

    private static string NormalizeWhitespace(string text) =>
        Regex.Replace(text.Trim(), @"\s+", " ");

    private record Signature(
        string TypeName,
        string Member,
        string Kind,
        string Framework,
        string? Url,
        bool IsStatic
    );
}
