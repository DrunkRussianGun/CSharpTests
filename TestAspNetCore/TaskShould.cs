using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MoreLinq;
using NUnit.Framework;

namespace TestNetCore
{
    [TestFixture]
    public class TaskShould
    {
        private const TaskContinuationOptions onSuccess = TaskContinuationOptions.OnlyOnRanToCompletion;

        [Test]
        public void WhenJustCreatedDelayed_NotBeRunning()
        {
            var task = Task.Delay(-1);
            task.Status.Should().NotBe(TaskStatus.Running);
        }
        
        [Test]
        public async Task WhenNoExceptions_Complete()
        {
            var result = await Task.FromResult(1);

            result.Should().Be(1);
        }
        
        [Test]
        public void WhenCanceled_NotNecessarilyHaveException()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var taskToCancel = Task.Delay(-1, cancellationTokenSource.Token);

            cancellationTokenSource.Cancel();

            taskToCancel.Exception.Should().BeNull();
        }

        [Test]
        public async Task WithContinuation_Complete()
        {
            var result = await Task.FromResult(1).ContinueWith(task => task.Result);

            result.Should().Be(1);
        }

        [Test]
        public async Task WithContinuationOnlyOnRanToCompletion_Complete()
        {
            var result = await Task.FromResult(1)
                .ContinueWith(task => task.Result, onSuccess);

            result.Should().Be(1);
        }

        [TestCase(null, null)]
        [TestCase(null, onSuccess)]
        [TestCase(onSuccess, null)]
        [TestCase(onSuccess, onSuccess)]
        public async Task WithChainedContinuation_Complete(
            TaskContinuationOptions? firstContinuationOptions,
            TaskContinuationOptions? secondContinuationOptions)
        {
            var result = await Continue(Continue(
                Task.FromResult(0),
                1, firstContinuationOptions),
                2, secondContinuationOptions);

            result.Should().Be(2);
        }

        [TestCase(null, null, 2)]
        [TestCase(null, onSuccess, 2)]
        [TestCase(onSuccess, null, 2)]
        [TestCase(onSuccess, onSuccess, null)]
        public async Task WithChainedContinuationWhenHasExceptions_BehaveAsExpected(
            TaskContinuationOptions? firstContinuationOptions,
            TaskContinuationOptions? secondContinuationOptions,
            int? lastSuccessfulContinuationNumber)
        {
            var task = Continue(Continue(
                Task.FromException<int>(new ArithmeticException()),
                1, firstContinuationOptions),
                2, secondContinuationOptions);

            if (lastSuccessfulContinuationNumber.HasValue)
                (await task).Should().Be(lastSuccessfulContinuationNumber.Value);
            else
                task.Awaiting(x => x).Should().Throw<TaskCanceledException>();
        }

        [Test]
        public void WhenHasExceptions_ThrowOriginal()
        {
            Task.FromException(new ArithmeticException())
                .Awaiting(x => x)
                .Should().Throw<ArithmeticException>();
        }

        [Test]
        public async Task OnWhenAll_OnExceptions_ThrowFirst()
        {
            var tasks = new[]
            {
                Task.Delay(100).ContinueWith(_ => throw new NullReferenceException()),
                Task.FromException(new ArithmeticException())
            };

            Func<Task> awaitingTasks = () => Task.WhenAll(tasks);

            await awaitingTasks.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task OnWhenAll_OnException_CompleteOtherTasks()
        {
            var faultedTask = Task.FromException(new ArithmeticException());
            var task = Task.Delay(100).ContinueWith(_ => 1);

            Func<Task> awaitingTasks = () => Task.WhenAll(faultedTask, task);

            await awaitingTasks.Should().ThrowAsync<ArithmeticException>();
            task.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        [Test]
        public void WithConfigureAwaitWhenHasExceptions_ThrowOriginal()
        {
            Enumerable.Range(1, 5)
                .Select(_ => Task.FromException(new ArithmeticException()))
                .ForEach(
                    task => task
                        .Invoking(async x => await x.ConfigureAwait(false))
                        .Should().Throw<ArithmeticException>());
        }

        [Test]
        public void WithContinuationWhenHasExceptions_ThrowOriginal()
        {
            Task.FromException<int>(new ArithmeticException())
                .ContinueWith(task => task.Result)
                .Awaiting(task => task)
                .Should().Throw<ArithmeticException>();
        }

        [Test]
        public void WithContinuationOnlyOnCompletedWhenHasExceptions_ThrowTaskCanceledException()
        {
            Task.FromException<int>(new ArithmeticException())
                .ContinueWith(task => task.Result, onSuccess)
                .Awaiting(task => task)
                .Should().Throw<TaskCanceledException>()
                .Which.InnerException.Should().BeNull();
        }

        [Test]
        public void WhenReturningCompletedTask_RunSameTask()
        {
            var tasks = new[]
            {
                DelayWithCompletedTask(30),
                DelayWithCompletedTask(20),
                DelayWithCompletedTask(10)
            };

            tasks.Should().OnlyContain(task => ReferenceEquals(task, tasks[0]));
        }

        [Test]
        public void WhenReturnedFromNonAsyncMethodWithException_ThrowOriginal()
        {
            Func<Task> nonAsyncMethod = () => throw new ArithmeticException();

            nonAsyncMethod
                .Awaiting(x => x.Invoke())
                .Should().Throw<ArithmeticException>();
        }

        [Test]
        public void WhenReturnedFromAsyncMethodWithException_ThrowOriginal()
        {
            #pragma warning disable 1998
            Func<Task> asyncMethod = async () => throw new ArithmeticException();
            #pragma warning restore 1998

            asyncMethod
                .Awaiting(x => x.Invoke())
                .Should().Throw<ArithmeticException>();
        }

        [Test]
        public void WhenReturnedFromNonAsyncMethodWithException_ThrowOriginal()
        {
            Func<Task> nonAsyncMethod = () => throw new ArithmeticException();

            nonAsyncMethod
                .Awaiting(x => x.Invoke())
                .Should().Throw<ArithmeticException>();
        }

        [Test]
        public void WhenReturnedFromAsyncMethodWithException_ThrowOriginal()
        {
            #pragma warning disable 1998
            Func<Task> asyncMethod = async () => throw new ArithmeticException();
            #pragma warning restore 1998

            asyncMethod
                .Awaiting(x => x.Invoke())
                .Should().Throw<ArithmeticException>();
        }

        private static Task DelayWithCompletedTask(int delayInMilliseconds)
        {
            Thread.Sleep(delayInMilliseconds);
            return Task.CompletedTask;
        }

        private static Task<int> Continue(Task<int> task, int newResult, TaskContinuationOptions? continuationOptions)
            => continuationOptions.HasValue
                ? task.ContinueWith(_ => newResult, continuationOptions.Value)
                : task.ContinueWith(_ => newResult);
    }
}
