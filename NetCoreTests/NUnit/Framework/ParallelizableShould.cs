using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetCoreTests.NUnit.Framework.Helpers;
using NUnit.Framework;

namespace NetCoreTests.NUnit.Framework
{
    // See https://docs.nunit.org/articles/nunit/technical-notes/usage/Framework-Parallel-Test-Execution.html

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ParallelizableShould
    {
        private ParallelizableTestHelper helper = new();
        private ExecutionEntry[] expectedExecutionEntries;

        public ParallelizableShould()
        {
            ParallelizableSetUpFixture.OnTearDown += FixtureTearDown;
        }

        public void FixtureTearDown()
        {
            helper.ExecutionEntries.Should().Equal(expectedExecutionEntries);
        }

        ~ParallelizableShould()
        {
            ParallelizableSetUpFixture.OnTearDown -= FixtureTearDown;
        }

        public class WhenParallelizableSelf : ParallelizableShould
        {
            private new static readonly ParallelizableTestHelper helper = new();
            
            public WhenParallelizableSelf()
            {
                base.helper = helper;
                expectedExecutionEntries = new[]
                {
                    ExecutionEntry.Beginning(1.GetMethodName()),
                    ExecutionEntry.Beginning(2.GetMethodName()),
                    ExecutionEntry.Ending(1.GetMethodName()),
                    ExecutionEntry.Ending(2.GetMethodName()),
                    ExecutionEntry.Beginning(3.GetMethodName()),
                    ExecutionEntry.Beginning(4.GetMethodName()),
                    ExecutionEntry.Ending(3.GetMethodName()),
                    ExecutionEntry.Ending(4.GetMethodName())
                };
            }

            [Order(1)]
            [Parallelizable(ParallelScope.Self)]
            public class FirstFixture : WhenParallelizableSelf
            {
                [Test, Order(1)]
                public Task First() => helper.RunTestAsync(1);

                [Test, Order(3)]
                public Task Third() => helper.RunTestAsync(3);
            }

            [Order(2)]
            [Parallelizable(ParallelScope.Self)]
            public class SecondFixture : WhenParallelizableSelf
            {
                [Test, Order(2)]
                public Task Second() => helper.RunTestAsync(2);

                [Test, Order(4)]
                public Task Fourth() => helper.RunTestAsync(4);
            }
        }
        
        [Parallelizable(ParallelScope.Children)]
        public class WhenParallelizableChildren : ParallelizableShould
        {
            public WhenParallelizableChildren()
            {
                expectedExecutionEntries = new[]
                {
                    ExecutionEntry.Beginning(1.GetMethodName()),
                    ExecutionEntry.Beginning(2.GetMethodName()),
                    ExecutionEntry.Ending(1.GetMethodName()),
                    ExecutionEntry.Ending(2.GetMethodName())
                };
            }

            [Test, Order(1)]
            public Task First() => helper.RunTestAsync(1);

            [Test, Order(2)]
            public Task Second() => helper.RunTestAsync(2);
        }
        
        public class WhenParallelizableFixtures : ParallelizableShould
        {
            private new static readonly ParallelizableTestHelper helper = new();
            
            public WhenParallelizableFixtures()
            {
                base.helper = helper;
                expectedExecutionEntries = new[]
                {
                    ExecutionEntry.Beginning(1.GetMethodName()),
                    ExecutionEntry.Beginning(2.GetMethodName()),
                    ExecutionEntry.Ending(1.GetMethodName()),
                    ExecutionEntry.Ending(2.GetMethodName()),
                    ExecutionEntry.Beginning(3.GetMethodName()),
                    ExecutionEntry.Beginning(4.GetMethodName()),
                    ExecutionEntry.Ending(3.GetMethodName()),
                    ExecutionEntry.Ending(4.GetMethodName())
                };
            }

            [Order(1)]
            [Parallelizable(ParallelScope.Fixtures)]
            public class FirstFixture : WhenParallelizableFixtures
            {
                [Test, Order(1)]
                public Task First() => helper.RunTestAsync(1);

                [Test, Order(3)]
                public Task Third() => helper.RunTestAsync(3);
            }

            [Order(2)]
            [Parallelizable(ParallelScope.Fixtures)]
            public class SecondFixture : WhenParallelizableFixtures
            {
                [Test, Order(2)]
                public Task Second() => helper.RunTestAsync(2);

                [Test, Order(4)]
                public Task Fourth() => helper.RunTestAsync(4);
            }
        }
        
        public class WhenParallelizableNone : ParallelizableShould
        {
            private new static readonly ParallelizableTestHelper helper = new();
            
            public WhenParallelizableNone()
            {
                base.helper = helper;
                expectedExecutionEntries = new[]
                {
                    ExecutionEntry.Beginning(1.GetMethodName()),
                    ExecutionEntry.Ending(1.GetMethodName()),
                    ExecutionEntry.Beginning(3.GetMethodName()),
                    ExecutionEntry.Ending(3.GetMethodName()),
                    ExecutionEntry.Beginning(2.GetMethodName()),
                    ExecutionEntry.Ending(2.GetMethodName()),
                    ExecutionEntry.Beginning(4.GetMethodName()),
                    ExecutionEntry.Ending(4.GetMethodName())
                };
            }

            [Order(1)]
            [NonParallelizable]
            public class FirstFixture : WhenParallelizableNone
            {
                [Test, Order(1)]
                public Task First() => helper.RunTestAsync(1);

                [Test, Order(3)]
                public Task Third() => helper.RunTestAsync(3);
            }

            [Order(2)]
            [NonParallelizable]
            public class SecondFixture : WhenParallelizableNone
            {
                [Test, Order(2)]
                public Task Second() => helper.RunTestAsync(2);

                [Test, Order(4)]
                public Task Fourth() => helper.RunTestAsync(4);
            }
        }
    }

    [SetUpFixture]
    public class ParallelizableSetUpFixture
    {
        public static event Action OnTearDown;

        [OneTimeTearDown]
        public void TearDown()
        {
            OnTearDown?.Invoke();
        }
    }

    public static class ParallelizableTestExtensions
    {
        public static string GetMethodName(this int order) => "method " + order;
        
        public static async Task RunTestAsync(this ParallelizableTestHelper helper, int order)
        {
            await Task.Delay(order * 20 + 100);

            var methodName = order.GetMethodName();
            helper.BeginExecution(methodName);
            await Task.Delay(100.Milliseconds());
            helper.EndExecution(methodName);
        }
    }
}
