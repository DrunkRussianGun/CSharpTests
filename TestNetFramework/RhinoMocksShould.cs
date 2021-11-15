using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace TestNetFramework
{
    [TestFixture]
    public class RhinoMocksShould
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public interface IIntContainer
        {
            int Value { get; }

            int MockedValue { get; }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public class IntContainer : IIntContainer
        {
            public int Value { get; }

            public int MockedValue { get; }

            public IntContainer(int value, int mockedValue)
            {
                Value = value;
                MockedValue = mockedValue;
            }
        }

        private readonly MockRepository mocks = new();

        [TestCaseSource(nameof(NotBeAbleToGenerateMockOfIntContainer_TestCaseSource))]
        public void NotBeAbleToGenerateMockOfIntContainer(Func<MockRepository, IntContainer> generator)
        {
            mocks.Invoking(generator).Should().Throw<ArgumentException>();
        }

        public static IEnumerable<TestCaseData> NotBeAbleToGenerateMockOfIntContainer_TestCaseSource()
        {
            yield return GetTestCase("As strict mock", x => x.StrictMock<IntContainer>());
            yield return GetTestCase("As dynamic mock", x => x.DynamicMock<IntContainer>());
            
            static TestCaseData GetTestCase(string name, Func<MockRepository, IntContainer> generator)
                => new TestCaseData(generator).SetName(name);
        }

        [Test]
        public void NotBeAbleToGeneratePartialMockOfIIntContainer()
        {
            mocks
                .Invoking(x => x.PartialMock<IIntContainer>())
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void NotBeAbleToCallAbstractOriginalMethod()
        {
            var mock = mocks.StrictMock<IIntContainer>();
            using var _ = mocks.Record();
            
            mock.Expect(x => x.Value)
                .Invoking(x => x.CallOriginalMethod(OriginalCallOptions.NoExpectation))
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void BeAbleToMockOnlyOneProperty()
        {
            var container = new IntContainer(1, 2);
            var mock = mocks.StrictMock<IIntContainer>();
            using (mocks.Record())
            {
                mock.Expect(x => x.Value).Do((Func<int>)(() => container.Value));
                mock.Expect(x => x.MockedValue).Return(3);
            }

            mock.Value.Should().Be(1);
            mock.MockedValue.Should().Be(3);
        }
    }
}
