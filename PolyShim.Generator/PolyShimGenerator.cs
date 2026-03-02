using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace PolyShim;

[Generator]
internal sealed class PolyShimGenerator : IIncrementalGenerator
{
    // Maps each FEATURE_* constant name to the fully-qualified type name that represents it.
    // A feature is considered "available" when its representative type is present in the compilation.
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

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, Execute);
    }

    private static void Execute(SourceProductionContext context, Compilation compilation)
    {
        // Determine which features are available in the current compilation
        var availableFeatures = new HashSet<string>(StringComparer.Ordinal);
        foreach (var kvp in FeatureTypes)
        {
            if (compilation.GetTypeByMetadataName(kvp.Value) is not null)
                availableFeatures.Add(kvp.Key);
        }

        // Also check for unsafe blocks support
        bool allowUnsafe = compilation is CSharpCompilation cs && cs.Options.AllowUnsafe;
        if (allowUnsafe)
            availableFeatures.Add("ALLOW_UNSAFE_BLOCKS");

        // Build a prefix with #define for every available feature.
        // These are prepended to each emitted file so that intra-file #if FEATURE_* checks work.
        var sb = new StringBuilder();
        foreach (var feature in availableFeatures.OrderBy(f => f, StringComparer.Ordinal))
            sb.AppendLine($"#define {feature}");
        sb.AppendLine();
        var definePrefix = sb.ToString();

        // Emit each embedded polyfill file, conditionally based on its outer #if guard
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

            // Read the first non-empty line to check for an outer FEATURE_ guard
            var firstLine = string.Empty;
            foreach (var line in content.Split('\n'))
            {
                var trimmed = line.Trim();
                if (trimmed.Length > 0)
                {
                    firstLine = trimmed;
                    break;
                }
            }

            bool shouldEmit;
            if (firstLine.StartsWith("#if !FEATURE_", StringComparison.Ordinal))
            {
                // Type polyfill: emit only when the type is NOT available in the compilation
                var feature = firstLine.Substring("#if !FEATURE_".Length).Trim();
                shouldEmit = !availableFeatures.Contains("FEATURE_" + feature);
            }
            else if (firstLine.StartsWith("#if FEATURE_", StringComparison.Ordinal))
            {
                // Member polyfill: emit only when the base type IS available in the compilation
                var feature = firstLine.Substring("#if FEATURE_".Length).Trim();
                shouldEmit = availableFeatures.Contains("FEATURE_" + feature);
            }
            else
            {
                // No feature guard — always emit (TFM-based guards inside handle correctness)
                shouldEmit = true;
            }

            if (!shouldEmit)
                continue;

            // Build the hint name: strip the namespace prefix and only the trailing .cs suffix
            var baseName = resourceName.Substring(polyfillsPrefix.Length);
            if (baseName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                baseName = baseName.Substring(0, baseName.Length - 3);
            var hintName = baseName + ".g.cs";

            context.AddSource(hintName, SourceText.From(definePrefix + content, Encoding.UTF8));
        }
    }
}
