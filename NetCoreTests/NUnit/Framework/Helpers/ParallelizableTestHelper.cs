using System.Collections.Generic;

namespace NetCoreTests.NUnit.Framework.Helpers;

public class ParallelizableTestHelper
{
    private readonly List<ExecutionEntry> executionEntries = new();
    // ReSharper disable once InconsistentlySynchronizedField
    public IReadOnlyList<ExecutionEntry> ExecutionEntries => executionEntries;

    public void BeginExecution(string methodName)
    {
        lock (executionEntries)
        {
            executionEntries.Add(ExecutionEntry.Beginning(methodName));
        }
    }

    public void EndExecution(string methodName)
    {
        lock (executionEntries)
        {
            executionEntries.Add(ExecutionEntry.Ending(methodName));
        }
    }
}