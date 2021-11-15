using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace TestNetCore
{
    [TestFixture]
    public class ConstructorInfoShould
    {
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private class Class
        {
            public Class() { }
            
            public Class(object _) { }
        }

        [TestCaseSource(nameof(HaveCorrectName_TestCaseSource))]
        public void HaveCorrectName(Type[] constructorParameters, string expectedName)
        {
            var constructorInfo = typeof(Class).GetConstructor(constructorParameters);

            constructorInfo.Should().NotBeNull();
            constructorInfo!.Name.Should().Be(expectedName);
        }

        public static IEnumerable<TestCaseData> HaveCorrectName_TestCaseSource()
        {
            yield return new TestCaseData(Array.Empty<Type>(), ".ctor");
            yield return new TestCaseData(new[] { typeof(object) }, ".ctor");
        }
    }
}
