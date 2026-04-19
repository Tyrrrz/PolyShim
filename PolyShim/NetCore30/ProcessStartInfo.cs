#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#if FEATURE_PROCESS
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file sealed class ProcessStartInfoArgumentListCollection(ProcessStartInfo startInfo)
    : Collection<string>
{
    public static readonly ConditionalWeakTable<
        ProcessStartInfo,
        ProcessStartInfoArgumentListCollection
    > BindingTable = new();

    private readonly WeakReference<ProcessStartInfo> _startInfo = new(startInfo);

    private void UpdateArguments()
    {
        static void AppendArgument(StringBuilder buffer, string arg)
        {
            // Short-circuit if no escaping is needed
            if (arg.Length > 0)
            {
                var needsEscaping = false;
                foreach (var c in arg)
                {
                    if (char.IsWhiteSpace(c) || c == '"')
                    {
                        needsEscaping = true;
                        break;
                    }
                }

                if (!needsEscaping)
                {
                    buffer.Append(arg);
                    return;
                }
            }

            buffer.Append('"');

            for (var i = 0; i < arg.Length; )
            {
                var c = arg[i++];

                if (c == '\\')
                {
                    var backslashCount = 1;
                    while (i < arg.Length && arg[i] == '\\')
                    {
                        backslashCount++;
                        i++;
                    }

                    if (i == arg.Length)
                    {
                        // Backslashes at end of string: double them
                        buffer.Append('\\', backslashCount * 2);
                    }
                    else if (arg[i] == '"')
                    {
                        // Backslashes before a quote: double them, then escape the quote
                        buffer.Append('\\', backslashCount * 2 + 1).Append('"');
                        i++;
                    }
                    else
                    {
                        // Backslashes not before a quote: leave them as-is
                        buffer.Append('\\', backslashCount);
                    }
                }
                else if (c == '"')
                {
                    buffer.Append('\\').Append('"');
                }
                else
                {
                    buffer.Append(c);
                }
            }

            buffer.Append('"');
        }

        if (!_startInfo.TryGetTarget(out var startInfo))
            return;

        if (Count == 0)
        {
            startInfo.Arguments = string.Empty;
            return;
        }

        var buffer = new StringBuilder();
        foreach (var arg in this)
        {
            if (buffer.Length > 0)
                buffer.Append(' ');

            AppendArgument(buffer, arg);
        }

        startInfo.Arguments = buffer.ToString();
    }

    protected override void InsertItem(int index, string item)
    {
        base.InsertItem(index, item);
        UpdateArguments();
    }

    protected override void RemoveItem(int index)
    {
        base.RemoveItem(index);
        UpdateArguments();
    }

    protected override void SetItem(int index, string item)
    {
        base.SetItem(index, item);
        UpdateArguments();
    }

    protected override void ClearItems()
    {
        base.ClearItems();
        UpdateArguments();
    }
}

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_ProcessStartInfo
{
    extension(ProcessStartInfo startInfo)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.argumentlist
        public Collection<string> ArgumentList =>
            ProcessStartInfoArgumentListCollection.BindingTable.GetValue(
                startInfo,
                key => new ProcessStartInfoArgumentListCollection(key)
            );
    }
}
#endif
#endif
