using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace NetCoreTests
{
    [TestFixture]
    public class FunctionDictionaryShould
    {
        private Dictionary<Func<int, int>, int> dictionary;

        [SetUp]
        public void SetUp()
        {
            dictionary = new Dictionary<Func<int, int>, int>();
        }
        
        [Test]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void ContainTwoEquivalentLambdas()
        {
            Func<int, int> lambda1 = x => x * 2;
            Func<int, int> lambda2 = x => x * 2;
            
            dictionary.Add(lambda1, 1);
            dictionary
                .Invoking(x => x.Add(lambda2, 2))
                .Should().NotThrow<ArgumentException>();
        }
        
        [Test]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void NotContainSameLambdaTwoTimes()
        {
            Func<int, int> lambda = x => x * 2;
            
            dictionary.Add(lambda, 1);
            dictionary
                .Invoking(x => x.Add(lambda, 2))
                .Should().Throw<ArgumentException>();
        }
        
        [Test]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void NotContainSameFunctionTwoTimes()
        {
            dictionary.Add(TestFunction, 1);
            dictionary
                .Invoking(x => x.Add(TestFunction, 2))
                .Should().Throw<ArgumentException>();
        }

        private static int TestFunction(int x) => x * 3;
    }
}
