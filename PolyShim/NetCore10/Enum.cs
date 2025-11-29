#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    extension(Enum enumValue)
    {
        // https://learn.microsoft.com/dotnet/api/system.enum.hasflag
        public bool HasFlag(Enum flag)
        {
            var thisValue = Convert.ToUInt64(enumValue);
            var flagValue = Convert.ToUInt64(flag);

            return (thisValue & flagValue) == flagValue;
        }
    }

    extension(Enum)
    {
        // https://learn.microsoft.com/dotnet/api/system.enum.tryparse#system-enum-tryparse-1(system-string-system-boolean-0@)
        public static bool TryParse<T>(string value, bool ignoreCase, out T result)
            where T : struct, Enum
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), value, ignoreCase);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.enum.tryparse#system-enum-tryparse-1(system-string-0@)
        public static bool TryParse<T>(string value, out T result)
            where T : struct, Enum
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), value);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
#endif
