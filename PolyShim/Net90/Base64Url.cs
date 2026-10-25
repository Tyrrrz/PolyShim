#if !FEATURE_BASE64URL
#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETSTANDARD) || (NETFRAMEWORK)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.Buffers.Text;

// https://learn.microsoft.com/dotnet/api/system.buffers.text.base64url
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Base64Url
{
    public static int GetEncodedLength(int bytesLength)
    {
        var remainder = bytesLength % 3;
        return bytesLength / 3 * 4 + (remainder > 0 ? remainder + 1 : 0);
    }

    public static int GetMaxDecodedLength(int base64Length)
    {
        var remainder = (int)((uint)base64Length % 4);
        return (base64Length >> 2) * 3 + (remainder > 0 ? remainder - 1 : 0);
    }

    public static bool IsValid(ReadOnlySpan<char> base64UrlText, out int decodedLength)
    {
        foreach (var ch in base64UrlText)
        {
            if (
                !(ch >= 'A' && ch <= 'Z')
                && !(ch >= 'a' && ch <= 'z')
                && !(ch >= '0' && ch <= '9')
                && ch != '-'
                && ch != '_'
            )
            {
                decodedLength = 0;
                return false;
            }
        }

        decodedLength = GetMaxDecodedLength(base64UrlText.Length);
        return true;
    }

    public static bool IsValid(ReadOnlySpan<char> base64UrlText) => IsValid(base64UrlText, out _);

    public static bool IsValid(ReadOnlySpan<byte> utf8Base64UrlText, out int decodedLength) =>
        IsValid(Encoding.UTF8.GetString(utf8Base64UrlText).AsSpan(), out decodedLength);

    public static bool IsValid(ReadOnlySpan<byte> utf8Base64UrlText) =>
        IsValid(utf8Base64UrlText, out _);

    public static bool TryDecodeFromChars(
        ReadOnlySpan<char> source,
        Span<byte> destination,
        out int bytesWritten
    )
    {
        if (!IsValid(source, out var decodedLength))
        {
            bytesWritten = 0;
            return false;
        }

        if (destination.Length < decodedLength)
        {
            bytesWritten = 0;
            return false;
        }

        var data = Convert.FromBase64String(
            new string(source.ToArray()).Replace('-', '+').Replace('_', '/')
                + new string('=', (4 - source.Length % 4) % 4)
        );

        data.CopyTo(destination);
        bytesWritten = data.Length;

        return true;
    }

    public static byte[] DecodeFromChars(ReadOnlySpan<char> source)
    {
        var buffer = new byte[GetMaxDecodedLength(source.Length)];

        if (!TryDecodeFromChars(source, buffer, out var bytesWritten))
        {
            throw new ArgumentException("Destination buffer is too small.");
        }

        var result = new byte[bytesWritten];
        Array.Copy(buffer, result, bytesWritten);

        return result;
    }

    public static int DecodeFromChars(ReadOnlySpan<char> source, Span<byte> destination) =>
        TryDecodeFromChars(source, destination, out var bytesWritten)
            ? bytesWritten
            : throw new ArgumentException("Destination buffer is too small.");

    public static bool TryDecodeFromUtf8(
        ReadOnlySpan<byte> source,
        Span<byte> destination,
        out int bytesWritten
    ) =>
        TryDecodeFromChars(Encoding.UTF8.GetString(source).AsSpan(), destination, out bytesWritten);

    public static byte[] DecodeFromUtf8(ReadOnlySpan<byte> source) =>
        DecodeFromChars(Encoding.UTF8.GetString(source).AsSpan());

    public static int DecodeFromUtf8(ReadOnlySpan<byte> source, Span<byte> destination) =>
        TryDecodeFromUtf8(source, destination, out var bytesWritten)
            ? bytesWritten
            : throw new ArgumentException("Destination buffer is too small.");

    public static int DecodeFromUtf8InPlace(Span<byte> buffer) =>
        TryDecodeFromUtf8(buffer, buffer, out var bytesWritten)
            ? bytesWritten
            : throw new ArgumentException("Destination buffer is too small.");

    public static bool TryEncodeToChars(
        ReadOnlySpan<byte> source,
        Span<char> destination,
        out int charsWritten
    )
    {
        var base64 = Convert
            .ToBase64String(source)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=')
            .ToCharArray();

        if (destination.Length < base64.Length)
        {
            charsWritten = 0;
            return false;
        }

        base64.CopyTo(destination);
        charsWritten = base64.Length;

        return true;
    }

    public static char[] EncodeToChars(ReadOnlySpan<byte> source)
    {
        var buffer = new char[GetEncodedLength(source.Length)];

        if (!TryEncodeToChars(source, buffer, out var charsWritten))
        {
            throw new ArgumentException("Destination buffer is too small.");
        }

        var result = new char[charsWritten];
        Array.Copy(buffer, result, charsWritten);

        return result;
    }

    public static int EncodeToChars(ReadOnlySpan<byte> source, Span<char> destination) =>
        TryEncodeToChars(source, destination, out var charsWritten)
            ? charsWritten
            : throw new ArgumentException("Destination buffer is too small.");

    public static bool TryEncodeToUtf8(
        ReadOnlySpan<byte> source,
        Span<byte> destination,
        out int bytesWritten
    )
    {
        var buffer = new char[GetEncodedLength(source.Length)];

        if (!TryEncodeToChars(source, buffer, out bytesWritten))
        {
            return false;
        }

        Encoding.UTF8.GetBytes(buffer.AsSpan(0, bytesWritten), destination);

        return true;
    }

    public static byte[] EncodeToUtf8(ReadOnlySpan<byte> source)
    {
        var buffer = new byte[GetEncodedLength(source.Length)];

        if (!TryEncodeToUtf8(source, buffer, out var bytesWritten))
        {
            throw new ArgumentException("Destination buffer is too small.");
        }

        var result = new byte[bytesWritten];
        Array.Copy(buffer, result, bytesWritten);

        return result;
    }

    public static int EncodeToUtf8(ReadOnlySpan<byte> source, Span<byte> destination) =>
        TryEncodeToUtf8(source, destination, out var bytesWritten)
            ? bytesWritten
            : throw new ArgumentException("Destination buffer is too small.");

    public static string EncodeToString(ReadOnlySpan<byte> source) => new(EncodeToChars(source));
}
#endif
#endif
