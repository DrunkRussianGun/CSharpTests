namespace NetCoreTests.NUnit.Framework.Helpers
{
    public readonly struct ExecutionEntry
    {
        public enum EntryType { Beginning, Ending }
        
        public readonly string MethodName;
        public readonly EntryType Type;

        public ExecutionEntry(string methodName, EntryType type)
        {
            MethodName = methodName;
            Type = type;
        }

        public override string ToString() => $"{Type} {MethodName}";

        public static ExecutionEntry Beginning(string methodName)
            => new(methodName, EntryType.Beginning);

        public static ExecutionEntry Ending(string methodName)
            => new(methodName, EntryType.Ending);
    }
}
