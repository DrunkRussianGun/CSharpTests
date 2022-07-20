using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

// ReSharper disable RedundantOverriddenMember

namespace NetCoreTests.NUnit.Framework;

[TestFixture]
public class SetUpShould
{
    private readonly List<string> actualRunSetUpNames = new List<string>();
    private List<string> expectedRunSetUpNames;

    public SetUpShould()
    {
        expectedRunSetUpNames = new List<string>
        {
            nameof(SetUp),
            nameof(SetUpAsync)
        };
    }

    [SetUp]
    public void ClearSetUpNames() => actualRunSetUpNames.Clear();

    [SetUp]
    public virtual void SetUp() => actualRunSetUpNames.Add(nameof(SetUp));

    [SetUp]
    public virtual Task SetUpAsync() => actualRunSetUpNames.AddAsync(nameof(SetUpAsync));

    [Test]
    public void RunInExpectedOrder()
    {
        actualRunSetUpNames.Should().Equal(expectedRunSetUpNames);
    }

    public class WhenSyncBeforeAsync : SetUpShould
    {
        public WhenSyncBeforeAsync()
        {
            expectedRunSetUpNames = new List<string>
            {
                nameof(SetUp),
                nameof(SetUpAsync)
            };
        }

        public override void SetUp() => base.SetUp();

        public override Task SetUpAsync() => base.SetUpAsync();
    }

    public class WhenAsyncBeforeSync : SetUpShould
    {
        public WhenAsyncBeforeSync()
        {
            expectedRunSetUpNames = new List<string>
            {
                nameof(SetUpAsync),
                nameof(SetUp)
            };
        }

        public override Task SetUpAsync() => base.SetUpAsync();

        public override void SetUp() => base.SetUp();
    }

    public class WhenChildAsyncBeforeAsync : SetUpShould
    {
        public WhenChildAsyncBeforeAsync()
        {
            expectedRunSetUpNames = new List<string>
            {
                nameof(SetUp),
                nameof(ChildSetUp),
                nameof(ChildSetUpAsync),
                nameof(SetUpAsync)
            };
        }

        [SetUp]
        public void ChildSetUp() => actualRunSetUpNames.Add(nameof(ChildSetUp));

        [SetUp]
        public Task ChildSetUpAsync() => actualRunSetUpNames.AddAsync(nameof(ChildSetUpAsync));

        public override Task SetUpAsync() => base.SetUpAsync();
    }

    public class WhenChildAsyncBeforeSync : SetUpShould
    {
        public WhenChildAsyncBeforeSync()
        {
            expectedRunSetUpNames = new List<string>
            {
                nameof(SetUpAsync),
                nameof(ChildSetUp),
                nameof(ChildSetUpAsync),
                nameof(SetUp)
            };
        }

        [SetUp]
        public void ChildSetUp() => actualRunSetUpNames.Add(nameof(ChildSetUp));

        [SetUp]
        public Task ChildSetUpAsync() => actualRunSetUpNames.AddAsync(nameof(ChildSetUpAsync));

        public override void SetUp() => base.SetUp();
    }

    public class WhenChildSyncBeforeChildAsync : SetUpShould
    {
        public WhenChildSyncBeforeChildAsync()
        {
            expectedRunSetUpNames = new List<string>
            {
                nameof(SetUp),
                nameof(SetUpAsync),
                nameof(ChildSetUp),
                nameof(ChildSetUpAsync)
            };
        }

        [SetUp]
        public void ChildSetUp() => actualRunSetUpNames.Add(nameof(ChildSetUp));

        [SetUp]
        public Task ChildSetUpAsync() => actualRunSetUpNames.AddAsync(nameof(ChildSetUpAsync));
    }

    public class WhenChildAsyncBeforeChildSync : SetUpShould
    {
        public WhenChildAsyncBeforeChildSync()
        {
            expectedRunSetUpNames = new List<string>
            {
                nameof(SetUp),
                nameof(SetUpAsync),
                nameof(ChildSetUpAsync),
                nameof(ChildSetUp)
            };
        }

        [SetUp]
        public Task ChildSetUpAsync() => actualRunSetUpNames.AddAsync(nameof(ChildSetUpAsync));

        [SetUp]
        public void ChildSetUp() => actualRunSetUpNames.Add(nameof(ChildSetUp));
    }
}

public static class ListExtensions
{
    public static Task AddAsync<T>(this List<T> list, T item) => Task.Run(() => list.Add(item));
}