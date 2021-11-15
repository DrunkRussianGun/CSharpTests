using System;
using System.Threading.Tasks;

namespace TestNetFramework
{
    public static class WrapSuccessfulTaskResult
    {
        public static async Task RunAsync()
        {
            var stringToParse = "pffff";
            var stringMap = await SuccessfulTask(stringToParse)
                .ContinueWith(task => (stringToParse, task.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            Console.WriteLine(stringMap.stringToParse);
            Console.WriteLine(stringMap.Result);
        }

        private static Task<int> SuccessfulTask(string str) => Task.FromResult(int.Parse(str));
    }
}