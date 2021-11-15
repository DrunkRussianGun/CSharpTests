using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestNUnitNetFramework
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class Tests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Parallelizable(ParallelScope.Self)]
        public async Task Test1(int i)
        {
            await TestContext.Out.WriteLineAsync($"{DateTime.Now} Start1 {i}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            await TestContext.Out.WriteLineAsync($"{DateTime.Now} Stop1 {i}");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task Test2(int i)
        {
            await TestContext.Out.WriteLineAsync($"{DateTime.Now} Start2 {i}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            await TestContext.Out.WriteLineAsync($"{DateTime.Now} Stop2 {i}");
        }
    }
}