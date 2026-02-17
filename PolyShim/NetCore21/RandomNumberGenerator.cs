#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
#if NETSTANDARD && !NETSTANDARD1_3_OR_GREATER
using System.Reflection;
#endif

#if NETSTANDARD && !NETSTANDARD1_3_OR_GREATER
// Polyfill for RandomNumberGenerator class on older .NET Standard versions
namespace System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal abstract class RandomNumberGenerator : IDisposable
{
    public static RandomNumberGenerator Create() => new RNGCryptoServiceProviderWrapper();

    public abstract void GetBytes(byte[] data);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }

    private sealed class RNGCryptoServiceProviderWrapper : RandomNumberGenerator
    {
        private readonly object _rng;
        private readonly System.Reflection.MethodInfo _getBytesMethod;

        public RNGCryptoServiceProviderWrapper()
        {
            var rngType = Type.GetType(
                "System.Security.Cryptography.RNGCryptoServiceProvider, mscorlib"
            );
            if (rngType == null)
                throw new PlatformNotSupportedException("RNGCryptoServiceProvider not available");

            _rng = Activator.CreateInstance(rngType);
            _getBytesMethod = rngType.GetTypeInfo().GetDeclaredMethod("GetBytes");
            if (_getBytesMethod == null)
                throw new PlatformNotSupportedException("GetBytes method not found");
        }

        public override void GetBytes(byte[] data)
        {
            _getBytesMethod.Invoke(_rng, new object[] { data });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _rng is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
#else
using System.Security.Cryptography;
#endif

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore21_RandomNumberGenerator
{
    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.fill
        public static void Fill(Span<byte> data)
        {
            if (data.Length == 0)
                return;

            var buffer = new byte[data.Length];
            var rng = RandomNumberGenerator.Create();
            try
            {
                rng.GetBytes(buffer);
                buffer.CopyTo(data);
            }
            finally
            {
                ((IDisposable)rng).Dispose();
            }
        }
    }
}
#endif
