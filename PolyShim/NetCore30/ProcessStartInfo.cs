#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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
internal static class MemberPolyfills_NetCore30_ProcessStartInfo
{
    private static readonly ConditionalWeakTable<
        ProcessStartInfo,
        ArgumentListCollection
    > ArgumentListTable = new();

    private sealed class ArgumentListCollection : Collection<string>
    {
        private readonly WeakReference<ProcessStartInfo> _startInfo;

        internal ArgumentListCollection(ProcessStartInfo startInfo) =>
            _startInfo = new WeakReference<ProcessStartInfo>(startInfo);

        private void UpdateArguments()
        {
            if (!_startInfo.TryGetTarget(out var startInfo))
                return;

            if (Count == 0)
            {
                startInfo.Arguments = string.Empty;
                return;
            }

            var sb = new StringBuilder();
            foreach (var arg in this)
            {
                if (sb.Length > 0)
                    sb.Append(' ');

                AppendArgument(sb, arg);
            }

            startInfo.Arguments = sb.ToString();
        }

        private static void AppendArgument(StringBuilder sb, string arg)
        {
            // Implementation reference:
            // https://github.com/Tyrrrz/CliWrap/blob/66323cb0cd636a9e0acc7822e99b6b44062fd9f6/CliWrap/Builders/ArgumentsBuilder.cs#L155-L207
            // https://github.com/dotnet/runtime/blob/9a50493f9f1125fda5e2212b9d6718bc7cdbc5c0/src/libraries/System.Private.CoreLib/src/System/PasteArguments.cs#L10-L79

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
                    sb.Append(arg);
                    return;
                }
            }

            sb.Append('"');

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
                        sb.Append('\\', backslashCount * 2);
                    }
                    else if (arg[i] == '"')
                    {
                        // Backslashes before a quote: double them, then escape the quote
                        sb.Append('\\', backslashCount * 2 + 1).Append('"');
                        i++;
                    }
                    else
                    {
                        // Backslashes not before a quote: leave them as-is
                        sb.Append('\\', backslashCount);
                    }
                }
                else if (c == '"')
                {
                    sb.Append('\\').Append('"');
                }
                else
                {
                    sb.Append(c);
                }
            }

            sb.Append('"');
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

    extension(ProcessStartInfo startInfo)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.argumentlist
        public Collection<string> ArgumentList =>
            ArgumentListTable.GetValue(startInfo, key => new ArgumentListCollection(key));
    }
}
#endif
#endif
