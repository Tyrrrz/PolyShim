#if FEATURE_TASK
#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore20_Task
{
    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.iscompletedsuccessfully
        public bool IsCompletedSuccessfully => task.Status == TaskStatus.RanToCompletion;
    }
}
#endif
#endif
