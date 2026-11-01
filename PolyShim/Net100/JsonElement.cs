#if FEATURE_JSON
#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net100_JsonElement
{
    extension(JsonElement)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement.parse#system-text-json-jsonelement-parse(system-string-system-text-json-jsondocumentoptions)
        public static JsonElement Parse(string json, JsonDocumentOptions options = default)
        {
            using var document = JsonDocument.Parse(json, options);
            return document.RootElement.Clone();
        }

        // https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement.parse#system-text-json-jsonelement-parse(system-readonlyspan((system-char))-system-text-json-jsondocumentoptions)
        public static JsonElement Parse(
            ReadOnlySpan<char> json,
            JsonDocumentOptions options = default
        )
        {
            using var document = JsonDocument.Parse(json.ToString(), options);
            return document.RootElement.Clone();
        }

        // https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement.parse#system-text-json-jsonelement-parse(system-readonlyspan((system-byte))-system-text-json-jsondocumentoptions)
        public static JsonElement Parse(
            ReadOnlySpan<byte> utf8Json,
            JsonDocumentOptions options = default
        )
        {
            using var document = JsonDocument.Parse(utf8Json.ToArray(), options);
            return document.RootElement.Clone();
        }
    }
}
#endif
#endif
