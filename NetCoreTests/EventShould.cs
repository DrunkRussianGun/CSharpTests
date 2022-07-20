using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace NetCoreTests;

[TestFixture]
public class EventShould
{
    private class Class
    {
        public event Action<string> BeforeProcess;

        public event Func<string, Task> BeforeProcessAsync;

        public event Func<int> BeforeGetValue;

        public void Process(string value) => BeforeProcess?.Invoke(value);

        public Task ProcessAsync(string value) => BeforeProcessAsync?.Invoke(value);

        public int GetValue()
        {
            return BeforeGetValue?.Invoke() ?? 5;
        }
    }

    [Test]
    public void BeCalled()
    {
        var instance = new Class();
        var isCalled = false;
        instance.BeforeProcess += _ => isCalled = true;

        instance.Process(null);

        isCalled.Should().BeTrue();
    }

    [Test]
    public async Task BeCalledAsync()
    {
        var instance = new Class();
        var isCalled = false;
        instance.BeforeProcessAsync += _ =>
        {
            isCalled = true;
            return Task.CompletedTask;
        };

        await instance.ProcessAsync(null);

        isCalled.Should().BeTrue();
    }

    [Test]
    public void BeCalledWithReturnValue()
    {
        var instance = new Class();
        const int expectedValue = 7;
        instance.BeforeGetValue += () => expectedValue;

        var actualValue = instance.GetValue();

        actualValue.Should().Be(expectedValue);
    }

    [Test]
    public void WhenMultipleEvents_BeCalledWithLastReturnValue()
    {
        var instance = new Class();
        const int expectedValue = 7;
        instance.BeforeGetValue += () => 3;
        instance.BeforeGetValue += () => expectedValue;

        var actualValue = instance.GetValue();

        actualValue.Should().Be(expectedValue);
    }

    [Test]
    public async Task WhenMultipleEvents_BeCalledAsyncAndAwaitLastTask()
    {
        var instance = new Class();
        var cancellationTokenSource = new CancellationTokenSource();
        // ReSharper disable once MethodSupportsCancellation
        var fastTask = Task.Run(() => { });
        var longTask = Task.Delay(-1, cancellationTokenSource.Token);
        instance.BeforeProcessAsync += _ => longTask;
        instance.BeforeProcessAsync += _ => fastTask;

        await instance.ProcessAsync(null);

        try
        {
            fastTask.IsCompleted.Should().BeTrue();
            longTask.IsCompleted.Should().BeFalse();
        }
        finally
        {
            cancellationTokenSource.Cancel();
        }
    }
}